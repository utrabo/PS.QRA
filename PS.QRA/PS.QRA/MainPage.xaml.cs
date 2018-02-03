using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PS.QRA
{
    public partial class MainPage : ContentPage
    {
        // https://developer.xamarin.com/guides/android/application_fundamentals/working_with_audio/

        // segundo consta, 22Khz é o máximo que o ser humano escuta
        // então tem uma teoria fala que o dobro disso é seguro de se usar
        private const int SampleRateInHz = 44100;

        // precisa colocar em constante pra conversar com o Android
        // não dá pra usar os enums pré-fabricados aqui, fora do projeto do Android
        private const int ChannelIn_Mono = 16;
        private const int Encoding_PCM16Bits = 2;

        // Math.Abs(short.MinValue); 
        // Conforme: https://stackoverflow.com/a/24670698
        // Mas se você tentar executar essa porra dá exception, então vai constante mesmo
        // Conforme: https://stackoverflow.com/a/6265405
        private readonly double Absolute_Short_MinValue = 32768.0;

        private int FrequencyToBeFound = 440;
        private int DefasagemIntelectual = 2;


        private int MinFrequency = 70;
        private int MaxFrequency = 1200;
        public MainPage()
        {
            InitializeComponent();

            IAudioReader reader = DependencyService.Get<IAudioReader>();
            IAudioMediaUtil mediaUtil = DependencyService.Get<IAudioMediaUtil>();

            // bufferSize varia por hardware, no meu celular Asus Zenfone 4 é 3584
            int bufferSize = mediaUtil.GetMinBufferSize(SampleRateInHz, ChannelIn_Mono, Encoding_PCM16Bits);
            short[] audioBuffer = new short[bufferSize];
            reader.Initialize(SampleRateInHz, bufferSize);
            reader.StartRecording();
            
           // int maxIterations = 1000111;
            //int iterationNumber = 0;
            while (true)
            {
                try
                {
                    
                    reader.Read(audioBuffer, 0, audioBuffer.Length, 1);
                    //Debug.WriteLine("teste");
                    double[] x = new double[audioBuffer.Length];
                    for (int i = 0; i < x.Length; i++)
                    {
                        x[i] = audioBuffer[i] / Absolute_Short_MinValue;
                    }

                    //double[] peakValues;
                    //int[] peakIndices = FrequencyUtils.PerformFFTAndReturnPeaks(x, SampleRateInHz, MinFrequency, MaxFrequency, out peakValues);

                    //StringBuilder buffer = new StringBuilder();
                    //bool frequencyFound = false;
                    //int frequencyToBeFoundDividedByTen = FrequencyToBeFound / 10;
                    //foreach(var peak in peakIndices)
                    //{
                    //    if (peak > frequencyToBeFoundDividedByTen - DefasagemIntelectual &&
                    //        peak <= frequencyToBeFoundDividedByTen)
                    //    {
                    //        frequencyFound = true;
                    //        break;
                    //    }
                    //}

                    //if (frequencyFound)
                    //{
                    //    buffer.Append("#############  ");
                    //}

                    //foreach (var peak in peakIndices)
                    //{
                    //    buffer.Append(peak);
                    //    buffer.Append(", ");
                    //}
                    //Debug.WriteLine(buffer);



                    //buffer = new StringBuilder();
                    //foreach (var peak in peakValues)
                    //{
                    //    buffer.Append(peak);
                    //    buffer.Append(", ");
                    //}
                    //Debug.WriteLine(buffer);
                    //Debug.WriteLine("--------------------------------");

                    //StringBuilder buffer = new StringBuilder();
                    //foreach (var bit in audioBuffer)
                    //{
                    //    buffer.Append(NormalizeShortValue(bit));
                    //    buffer.Append(", ");
                    //}
                    //Debug.WriteLine(buffer);

                    double freq = FrequencyUtils.FindFundamentalFrequency(x, SampleRateInHz, MinFrequency, MaxFrequency);
                    Debug.WriteLine(freq);

                    //Debug.WriteLine(freq);
                    //if (freq > 430 && freq < 450)
                    //{
                    //    //DisplayAlert("Nota Lá foi tocada", "Ae caraleo", "Porra!");
                    //}

                    //iterationNumber++;
                    //if (iterationNumber >= maxIterations)
                    //{
                    //    Debug.WriteLine("Fim");
                    //    break;
                    //}

                }
                catch (Exception ex)
                {
                    DisplayAlert("", ex.ToString(), "Fudeu");
                    break;
                }
            }
        }

        private double NormalizeShortValue(short shortValue)
        {
            return (double)shortValue / Absolute_Short_MinValue;
        }

        private void Button_Pressed(object sender, EventArgs e)
        {
            
            
        }
    }
}
