using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS.QRA.AudioMessage
{
    public class AudioPartConfiguration
    {
        public AudioPart Part { get; set; }
        public double[] Frequencies { get; set; }
        public int FaultTolerance { get; set; }
    }
}
