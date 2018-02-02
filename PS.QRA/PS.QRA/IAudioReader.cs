using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS.QRA
{
    public interface IAudioReader
    {
        void Initialize(int sampleRateInHz, int bufferSizeInBytes);

        void StartRecording();
        
        void Read(short[] audioData, int offsetInShorts, int sizeInShorts, int readMode);

        void StopRecording();
    }
}
