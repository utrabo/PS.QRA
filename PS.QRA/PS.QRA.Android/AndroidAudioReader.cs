using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Media;
using PS.QRA.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidAudioReader))]
namespace PS.QRA.Droid
{

    public class AndroidAudioReader : IAudioReader
    {
        private AudioRecord _audioRecord;
        public void Initialize(int sampleRateInHz, int bufferSizeInBytes)
        {
            //_audioRecord = new AudioRecord(
            //    AudioSource.Mic,
            //    sampleRateInHz,
            //    ChannelIn.Mono,
            //    Android.Media.Encoding.Pcm16bit,
            //    bufferSizeInBytes);

            _audioRecord = findAudioRecord();
        }

        public void StartRecording()
        {
            if (_audioRecord == null)
                throw new ArgumentNullException();

            _audioRecord.StartRecording();
        }

        public void Read(short[] audioData, int offsetInShorts, int sizeInShorts, int readMode)
        {
            if (_audioRecord == null)
                throw new ArgumentNullException();
            
            _audioRecord.Read(audioData, offsetInShorts, sizeInShorts, readMode);
        }

        public void StopRecording()
        {
            if (_audioRecord == null)
                throw new ArgumentNullException();

            _audioRecord.Stop();
        }

        private static int[] mSampleRates = new int[] { 8000, 11025, 22050, 44100 };
        public AudioRecord findAudioRecord()
        {
            foreach (int rate in mSampleRates)
            {
                foreach (int audioFormat in new int[] { (int)Android.Media.Encoding.Pcm8bit, (int)Android.Media.Encoding.Pcm16bit })
                {
                    foreach (int channelConfig in new int[] { (int)ChannelIn.Mono, (int)ChannelIn.Stereo })
                    {
                        try
                        {
                            int bufferSize = AudioRecord.GetMinBufferSize(rate, (ChannelIn)channelConfig, (Android.Media.Encoding)audioFormat);

                            // antes tava checando por ERROR_BAD_VALUE mas não tem enum correspondente no Xamarin
                            // vi que o valor dessa constante era -2, mas estou checando maior que zero por segurança
                            // https://developer.android.com/reference/android/media/AudioRecord.html#ERROR_BAD_VALUE
                            if (bufferSize > 0)
                            {
                                // check if we can instantiate and have a success
                                AudioRecord recorder = new AudioRecord(AudioSource.Mic, rate, (ChannelIn)channelConfig, (Android.Media.Encoding)audioFormat, bufferSize);
                                
                                if (recorder.State == State.Initialized)
                                    return recorder;
                            }
                        }
                        catch (Exception e)
                        {
                          //  Log.e(C.TAG, rate + "Exception, keep trying.", e);
                        }
                    }
                }
            }
            return null;
        }

    }
}