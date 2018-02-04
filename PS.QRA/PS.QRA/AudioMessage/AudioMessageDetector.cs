using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS.QRA.AudioMessage
{
    public class AudioMessageDetector
    {
        private PreludeConfiguration PreludeConfiguration;
        private int ToneStep;
        private int ToneVariationTolerance;
        private int ToneMinimumRepetition;
        private List<PreludeOccurrenceMatrixItem> PreludeOccurrenceMatrix;

        public AudioMessageDetectionState State { get; set; }

        public AudioMessageDetector(PreludeConfiguration preludeConfig, int toneStep, int toneVariationTolerance, int toneMinimumRepetition)
        {
            PreludeConfiguration = preludeConfig;
            ToneStep = toneStep;
            ToneVariationTolerance = toneVariationTolerance;
            ToneMinimumRepetition = toneMinimumRepetition;
            PreludeOccurrenceMatrix = GetPreludeOccurrenceMatrix();
            State = AudioMessageDetectionState.SearchingForPrelude;
        }

        public delegate void AudioMessageDetectedEventHandler(object sender, AudioMessageEventArgs e);
        public event AudioMessageDetectedEventHandler AudioMessageDetected;

        public void AnalyzeSample(double[] sampleFrequencies)
        {
            if (sampleFrequencies == null)
                throw new ArgumentNullException("sampleFrequencies");

            List<Tone> tones = DetectTonesInSample(sampleFrequencies);
            if (State == AudioMessageDetectionState.SearchingForPrelude)
            {
                SearchForPrelude(tones);
                return;
            }
        }

        private void SearchForPrelude(List<Tone> tones)
        {
            for (int index = 0; index < PreludeOccurrenceMatrix.Count; index++)
            {
                if (PreludeOccurrenceMatrix[index].Occurrences >= ToneMinimumRepetition)
                    continue;

                if (tones.Exists(t => t.Frequency == PreludeOccurrenceMatrix[index].Frequency))
                {
                    PreludeOccurrenceMatrix[index].Occurrences++;

                    // if it is the last iteration and prelude is completed, change status
                    if (index == PreludeOccurrenceMatrix.Count - 1 &&
                        PreludeOccurrenceMatrix[index].Occurrences == ToneMinimumRepetition)
                        State = AudioMessageDetectionState.ListeningMessage;
                    break;
                }

                // if we are looking for the first tone of the prelude and didn't find it, we gotta keep looking
                if (index == 0 && PreludeOccurrenceMatrix[index].Occurrences == 0)
                    break;

                // we are still receiving notes from the last tone of the prelude
                if (index > 0 && tones.Exists(t => t.Frequency == PreludeOccurrenceMatrix[index - 1].Frequency))
                    break;

                PreludeOccurrenceMatrix[index].Faults++;
                if (PreludeOccurrenceMatrix[index].Faults >= PreludeConfiguration.FaultTolerance)
                    ResetPreludeOcurrenceMatrix();
                break;
            }
        }

        private List<PreludeOccurrenceMatrixItem> GetPreludeOccurrenceMatrix()
        {
            List<PreludeOccurrenceMatrixItem> preludeOccurrenceMatrix = new List<PreludeOccurrenceMatrixItem>();
            foreach (var preludeFrequency in PreludeConfiguration.Frequencies)
            {
                preludeOccurrenceMatrix.Add(new PreludeOccurrenceMatrixItem() { Frequency = preludeFrequency, Occurrences = 0, Faults = 0  });
            }

            return preludeOccurrenceMatrix;
        }

        private void ResetPreludeOcurrenceMatrix()
        {
            PreludeOccurrenceMatrix = GetPreludeOccurrenceMatrix();
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
