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
    }
}
