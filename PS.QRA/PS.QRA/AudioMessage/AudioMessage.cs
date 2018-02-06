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

        public override string ToString()
        {
            if (Frequencies == null || !Frequencies.Any())
                return "Empty message";

            StringBuilder frequencies = new StringBuilder();
            foreach (var freq in Frequencies)
            {
                frequencies.AppendFormat("{0:0.00}, ", freq);
            }

            return frequencies.ToString();
        }
    }
}
