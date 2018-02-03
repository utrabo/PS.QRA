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
            _audioRecord = new AudioRecord(
                AudioSource.Mic,
                sampleRateInHz,
                ChannelIn.Mono,
                Android.Media.Encoding.Pcm16bit,
                bufferSizeInBytes);
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

    }
}