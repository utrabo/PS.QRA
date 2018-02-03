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

[assembly: Xamarin.Forms.Dependency(typeof(AndroidAudioMediaUtil))]
namespace PS.QRA.Droid
{
    public class AndroidAudioMediaUtil : IAudioMediaUtil
    {
        public int GetMinBufferSize(int sampleRateInHz, int channelConfig, int audioFormat)
        {
            return AudioRecord.GetMinBufferSize(sampleRateInHz, (ChannelIn)channelConfig, (Android.Media.Encoding)audioFormat);
        }
    }
}