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

        private const int SampleRateInHz = 11025; // não sei porque esse valor, tava no link
        private const int MinFrequency = 70;
        private const int MaxFrequency = 1200;
        public MainPage()
        {
            InitializeComponent();

            IAudioReader reader = DependencyService.Get<IAudioReader>();


            short[] audioBuffer = new short[100000];
            reader.Initialize(SampleRateInHz, audioBuffer.Length);
            reader.StartRecording();

            while (true)
            {
                try
                {
                    reader.Read(audioBuffer, 0, audioBuffer.Length, 1);

                    double[] x = new double[audioBuffer.Length];
                    for (int i = 0; i < x.Length; i++)
                    {
                        x[i] = audioBuffer[i] / 32768.0;
                    }

                    double freq = FrequencyUtils.FindFundamentalFrequency(x, SampleRateInHz, MinFrequency, MaxFrequency) ;
                    Debug.WriteLine(freq);
                    if (freq > 430 && freq < 450)
                    {
                        //DisplayAlert("Nota Lá foi tocada", "Ae caraleo", "Porra!");
                    }

                }
                catch (Exception ex)
                {
                    DisplayAlert("", ex.ToString(), "Fudeu");
                    break;
                }
            }
        }

        private void Button_Pressed(object sender, EventArgs e)
        {
            
            
        }
    }
}
