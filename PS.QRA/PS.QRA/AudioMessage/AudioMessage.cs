using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS.QRA.AudioMessage
{
    public class AudioMessage
    {
        public List<double> Frequencies { get; set; }

        public AudioMessage()
        {
            Frequencies = new List<double>();
        }
    }
}
