using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS.QRA
{
    public interface IAudioMediaUtil
    {
        int GetMinBufferSize(int sampleRateInHz, int channelConfig, int audioFormat);
    }
}
