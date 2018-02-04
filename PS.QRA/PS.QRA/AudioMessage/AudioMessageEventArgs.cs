using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS.QRA.AudioMessage
{
    public class AudioMessageEventArgs : EventArgs
    {
        public AudioMessage AudioMessage { get; set; }
    }
}
