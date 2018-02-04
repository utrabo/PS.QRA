using PS.QRA.AudioMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS.QRA.Test
{
    public static class AudioMessageTestSubscriber
    {
        public static List<AudioMessage.AudioMessage> AudioMessages { get; set; }
        public static void Subscribe(AudioMessageDetector audioMessageDetector)
        {
            audioMessageDetector.AudioMessageDetected += AudioMessageDetected;
        }

        private static void AudioMessageDetected(object sender, AudioMessageEventArgs e)
        {
            AudioMessages.Add(e.AudioMessage);
        }
    }
}
