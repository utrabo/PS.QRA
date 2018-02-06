using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS.QRA.AudioMessage
{
    public class ToneOccurrenceMatrixItem
    {
        public double Frequency { get; set; }
        public int Occurrences { get; set; }
        public int Faults { get; set; }

        public override string ToString()
        {
            return string.Format("{0:0.00}, Occur.:{1}, Faults: {2}", Frequency, Occurrences, Faults);
        }
    }
}
