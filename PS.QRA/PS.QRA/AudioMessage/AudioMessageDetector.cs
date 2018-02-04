﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS.QRA.AudioMessage
{
    public class AudioMessageDetector
    {
        public delegate void AudioMessageDetectedEventHandler(object sender, AudioMessageEventArgs e);
        public event AudioMessageDetectedEventHandler AudioMessageDetected;

        private AudioPartConfiguration PreludeConfiguration;
        private AudioPartConfiguration FinaleConfiguration;
        private int ToneStep;
        private int ToneVariationTolerance;
        private int ToneMinimumRepetition;
        private int NumberOfSamplesFaultTolerance;

        private List<ToneOccurrenceMatrixItem> PreludeOccurrenceMatrix;
        private List<ToneOccurrenceMatrixItem> FinaleOccurrenceMatrix;
        private List<ToneOccurrenceMatrixItem> TonesBeingListened;
        private AudioMessage CurrentAudioMessage;

        public AudioMessageDetectionState State { get; set; }

        public AudioMessageDetector(
            AudioPartConfiguration preludeConfig, 
            AudioPartConfiguration finaleConfig, 
            int toneStep, 
            int toneVariationTolerance, 
            int toneMinimumRepetition,
            int numberOfSamplesFaultTolerance)
        {
            PreludeConfiguration = preludeConfig;
            FinaleConfiguration = finaleConfig;
            ToneStep = toneStep;
            ToneVariationTolerance = toneVariationTolerance;
            ToneMinimumRepetition = toneMinimumRepetition;
            NumberOfSamplesFaultTolerance = numberOfSamplesFaultTolerance;
            PreludeOccurrenceMatrix = GetToneOccurrenceMatrix(preludeConfig);
            FinaleOccurrenceMatrix = GetToneOccurrenceMatrix(finaleConfig);
            TonesBeingListened = new List<ToneOccurrenceMatrixItem>();
            CurrentAudioMessage = new AudioMessage();
            State = AudioMessageDetectionState.SearchingForPrelude;
        }

        public void AnalyzeSample(double[] sampleFrequencies)
        {
            if (sampleFrequencies == null)
                throw new ArgumentNullException("sampleFrequencies");

            List<Tone> tones = DetectTonesInSample(sampleFrequencies);
            switch(State)
            {
                case AudioMessageDetectionState.SearchingForPrelude:
                    SearchForAudioPart(tones, AudioPart.Prelude, PreludeOccurrenceMatrix, AudioMessageDetectionState.ListeningMessage);
                    break;
                case AudioMessageDetectionState.ListeningMessage:
                    ListenMessage(tones);

                    SearchForAudioPart(tones, AudioPart.Finale, FinaleOccurrenceMatrix, AudioMessageDetectionState.SearchingForPrelude);
                    break;
            }
        }

        private void ListenMessage(List<Tone> tones)
        {
            // if we are not listening to any tones yet
            // and the tone is equal to the last tone in prelude
            // ignore because we are stil receiving notes of the prelude
            if (TonesBeingListened.Count == 0 &&
                tones.Exists(t => t.Frequency == PreludeConfiguration.Frequencies.Last()))
                return;

            // if it is the first note of the finale or
            // if we already started to listen to the finale, ignore
            if (FinaleOccurrenceMatrix.Exists(t => t.Occurrences > 0) ||
                tones.Exists(t => t.Frequency == FinaleConfiguration.Frequencies.First()))
                return;

            // if it exceeds the fault tolerance, we ignore the tone
            for (int index = TonesBeingListened.Count; index >= 0; index--)
            {
                if (!tones.Exists(t => t.Frequency == TonesBeingListened[index].Frequency))
                {
                    TonesBeingListened[index].Faults++;
                    if (TonesBeingListened[index].Faults >= NumberOfSamplesFaultTolerance)
                        TonesBeingListened.RemoveAt(index);
                }
            }

            foreach (var tone in tones)
            {
                var toneOccurrence = TonesBeingListened.FirstOrDefault(t => t.Frequency == tone.Frequency);
                if (toneOccurrence == null)
                {
                    toneOccurrence = new ToneOccurrenceMatrixItem();
                    toneOccurrence.Frequency = tone.Frequency;
                    TonesBeingListened.Add(toneOccurrence);
                }
                toneOccurrence.Occurrences++;

                if (toneOccurrence.Occurrences == ToneMinimumRepetition)
                {
                    CurrentAudioMessage.Frequencies.Add(toneOccurrence.Frequency);
                }
            }
        }

        private void SearchForAudioPart(
            List<Tone> tones, 
            AudioPart audioPart,
            List<ToneOccurrenceMatrixItem> toneOccurrenceMatrix,
            AudioMessageDetectionState stateToChangeToWhenPartIsDetected)
        {

            for (int index = 0; index < toneOccurrenceMatrix.Count; index++)
            {
                if (toneOccurrenceMatrix[index].Occurrences >= ToneMinimumRepetition)
                    continue;

                if (tones.Exists(t => t.Frequency == toneOccurrenceMatrix[index].Frequency))
                {
                    toneOccurrenceMatrix[index].Occurrences++;
                    toneOccurrenceMatrix[index].Faults = 0;

                    if (index == toneOccurrenceMatrix.Count - 1 &&
                        toneOccurrenceMatrix[index].Occurrences == ToneMinimumRepetition)
                    {
                        State = stateToChangeToWhenPartIsDetected;

                        if (audioPart == AudioPart.Finale)
                            LaunchAudioMessageDetectedEvent();
                    }
                    break;
                }

                // if we are looking for the first tone of the prelude and didn't find it, we gotta keep looking
                if (audioPart == AudioPart.Prelude &&
                    index == 0 && toneOccurrenceMatrix[index].Occurrences == 0)
                    break;

                // we are still receiving notes from the last tone of the part
                if (index > 0 && tones.Exists(t => t.Frequency == toneOccurrenceMatrix[index - 1].Frequency))
                    break;

                // if no finale tone was listened, we are still listening to the message
                if (audioPart == AudioPart.Finale &&
                    index == 0 && toneOccurrenceMatrix.First().Occurrences == 0)
                    break;

                toneOccurrenceMatrix[index].Faults++;
                if (toneOccurrenceMatrix[index].Faults >= NumberOfSamplesFaultTolerance)
                    ResetOcurrenceMatrix(toneOccurrenceMatrix);
                break;
            }
        }

        private void LaunchAudioMessageDetectedEvent()
        {
            AudioMessageEventArgs args = new AudioMessageEventArgs();
            args.AudioMessage = CurrentAudioMessage;

            AudioMessageDetected?.Invoke(null, args);

            CurrentAudioMessage = new AudioMessage();
        }

        private List<ToneOccurrenceMatrixItem> GetToneOccurrenceMatrix(AudioPartConfiguration config)
        {
            List<ToneOccurrenceMatrixItem> toneOccurrenceMatrix = new List<ToneOccurrenceMatrixItem>();
            foreach (var frequency in config.Frequencies)
            {
                toneOccurrenceMatrix.Add(new ToneOccurrenceMatrixItem() { Frequency = frequency, Occurrences = 0, Faults = 0  });
            }

            return toneOccurrenceMatrix;
        }

        private void ResetOcurrenceMatrix(List<ToneOccurrenceMatrixItem> toneOccurrenceMatrix)
        {
            foreach(var item in toneOccurrenceMatrix)
            {
                item.Faults = 0;
                item.Occurrences = 0;
            }
        }

        public List<Tone> DetectTonesInSample(double[] sampleFrequencies)
        {
            List<Tone> tones = new List<Tone>();

            foreach(int frequency in sampleFrequencies)
            {
                int mod = frequency % ToneStep;
                if (mod + ToneVariationTolerance >= ToneStep)
                {
                    tones.Add(new Tone() { Frequency = frequency + ToneStep - mod });
                }
                else if (mod < ToneVariationTolerance)
                {
                    tones.Add(new Tone() { Frequency = frequency - mod });
                }
            }

            return tones;
        }



    }
}
