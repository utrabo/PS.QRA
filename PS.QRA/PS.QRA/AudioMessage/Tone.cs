using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS.QRA.AudioMessage
{
    public struct Tone
    {
        public double Frequency { get; set; }

        public override string ToString()
        {
            return string.Format("{0:0.00}", Frequency);
        }
    }
}
