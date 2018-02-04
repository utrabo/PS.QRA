using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using PS.QRA.AudioMessage;

namespace PS.QRA.Test
{
    [TestClass]
    public class AudioMessageDetectorTest
    {
        // scenario 1: 
        // sample => 1696.15, 1764, 1764, 1764, 1764
        // tone step 100
        // tone variation tolerance of 10
        [TestMethod]
        public void given_scenario_1_above_should_return_tone_1700()
        {
            AudioMessageDetector audioMessageDetector = new AudioMessageDetector(
                new AudioPartConfiguration() { Frequencies = new double[] { 0 } },
                new AudioPartConfiguration() { Frequencies = new double[] { 0 } },
                100, 10, 3, 2, 3);
            List<Tone> tones = audioMessageDetector.DetectTonesInSample(new double[] { 1696.15, 1764, 1764, 1764, 1764 });

            Assert.IsNotNull(tones);
            Assert.AreEqual(1, tones.Count);
            Assert.AreEqual(1700.0, tones[0].Frequency);
        }

        // scenario 2: 
        // sample => 1796.15, 1764, 1764, 1764, 1764
        // tone step 100
        // tone variation tolerance of 10
        [TestMethod]
        public void given_scenario_2_above_should_return_tone_1800()
        {
            AudioMessageDetector audioMessageDetector = new AudioMessageDetector(
                new AudioPartConfiguration() { Frequencies = new double[] { 0 } },
                new AudioPartConfiguration() { Frequencies = new double[] { 0 } },
                100, 10, 3, 2, 3);
            List<Tone> tones = audioMessageDetector.DetectTonesInSample(new double[] { 1796.15, 1764, 1764, 1764, 1764 });

            Assert.IsNotNull(tones);
            Assert.AreEqual(1, tones.Count);
            Assert.AreEqual(1800.0, tones[0].Frequency);
        }

        // scenario 3: 
        // sample => 1709, 1764, 1764, 1764, 1764
        // tone step 100
        // tone variation tolerance of 10
        [TestMethod]
        public void given_scenario_3_above_should_return_tone_1700()
        {
            AudioMessageDetector audioMessageDetector = new AudioMessageDetector(
                new AudioPartConfiguration() { Frequencies = new double[] { 0 } },
                new AudioPartConfiguration() { Frequencies = new double[] { 0 } },
                100, 10, 3, 2, 3);
            List<Tone> tones = audioMessageDetector.DetectTonesInSample(new double[] { 1709, 1764, 1764, 1764, 1764 });

            Assert.IsNotNull(tones);
            Assert.AreEqual(1, tones.Count);
            Assert.AreEqual(1700, tones[0].Frequency);
        }

        // scenario 4: 
        // sample => 1709, 1792, 1764, 1764, 1764
        // tone step 100
        // tone variation tolerance of 10
        [TestMethod]
        public void given_scenario_4_above_should_return_tone_1700_and_1800()
        {
            AudioMessageDetector audioMessageDetector = new AudioMessageDetector(
                new AudioPartConfiguration() { Frequencies = new double[] { 0 } },
                new AudioPartConfiguration() { Frequencies = new double[] { 0 } },
                100, 10, 3, 2, 3);
            List<Tone> tones = audioMessageDetector.DetectTonesInSample(new double[] { 1709, 1792, 1764, 1764, 1764 });

            Assert.IsNotNull(tones);
            Assert.AreEqual(2, tones.Count);
            Assert.AreEqual(1700, tones[0].Frequency);
            Assert.AreEqual(1800, tones[1].Frequency);
        }

        // scenario 5: 
        // sample => 1709, 1750, 1764, 1764, 1764
        //           1709, 1750, 1764, 1764, 1764
        //           1709, 1750, 1764, 1764, 1764
        // tone step 100
        // tone variation tolerance of 10
        // tone minimum repetition of 3
        // prelude configuration 1700
        [TestMethod]
        public void given_scenario_5_above_should_change_state_to_listening_to_message()
        {
            List<double[]> samples = new List<double[]>();
            samples.Add(new double[] { 1709, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1709, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1709, 1750, 1764, 1764, 1764 });

            AudioMessageDetector audioMessageDetector = new AudioMessageDetector(
                new AudioPartConfiguration() { Frequencies = new double[] { 1700 } },
                new AudioPartConfiguration() { Frequencies = new double[] { 0 } },
                100, 10, 3, 2, 3);
            foreach (var sample in samples)
            {
                audioMessageDetector.AnalyzeSample(sample);
            }

            Assert.AreEqual(AudioMessageDetectionState.ListeningMessage, audioMessageDetector.State);
        }

        // scenario 6: 
        // sample => 1709, 1750, 1764, 1764, 1764
        //           1709, 1750, 1764, 1764, 1764
        //           1709, 1750, 1764, 1764, 1764
        //           1791, 1750, 1764, 1764, 1764
        //           1791, 1750, 1764, 1764, 1764
        //           1791, 1750, 1764, 1764, 1764
        //           1891, 1750, 1764, 1764, 1764
        //           1891, 1750, 1764, 1764, 1764
        //           1891, 1750, 1764, 1764, 1764
        // tone step 100
        // tone variation tolerance of 10
        // tone minimum repetition of 3
        // prelude configuration 1700, 1800, 1900
        [TestMethod]
        public void given_scenario_6_above_should_change_state_to_listening_to_message()
        {
            List<double[]> samples = new List<double[]>();
            samples.Add(new double[] { 1709, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1709, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1709, 1750, 1764, 1764, 1764 });

            samples.Add(new double[] { 1791, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1791, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1791, 1750, 1764, 1764, 1764 });

            samples.Add(new double[] { 1891, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1891, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1891, 1750, 1764, 1764, 1764 });

            AudioMessageDetector audioMessageDetector = new AudioMessageDetector(
                new AudioPartConfiguration() { Frequencies = new double[] { 1700, 1800, 1900 } },
                new AudioPartConfiguration() { Frequencies = new double[] { 0 } },
                100, 10, 3, 2, 3);
            foreach(var sample in samples)
            {
                audioMessageDetector.AnalyzeSample(sample);
            }

            Assert.AreEqual(AudioMessageDetectionState.ListeningMessage, audioMessageDetector.State);
        }

        // scenario 7: 
        // sample => 1709, 1750, 1764, 1764, 1764
        //           1709, 1750, 1764, 1764, 1764
        //           1709, 1750, 1764, 1764, 1764
        //           1791, 1750, 1764, 1764, 1764
        //           1791, 1750, 1764, 1764, 1764
        //           1791, 1750, 1764, 1764, 1764
        // tone step 100
        // tone variation tolerance of 10
        // tone minimum repetition of 3
        // prelude configuration 1700, 1800, 1900
        [TestMethod]
        public void given_scenario_7_above_should_continue_searching_for_prelude()
        {
            List<double[]> samples = new List<double[]>();
            samples.Add(new double[] { 1709, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1709, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1709, 1750, 1764, 1764, 1764 });

            samples.Add(new double[] { 1791, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1791, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1791, 1750, 1764, 1764, 1764 });

            AudioMessageDetector audioMessageDetector = new AudioMessageDetector(
                new AudioPartConfiguration() { Frequencies = new double[] { 1700, 1800, 1900 } },
                new AudioPartConfiguration() { Frequencies = new double[] { 0 } },
                100, 10, 3, 2, 3);
            foreach (var sample in samples)
            {
                audioMessageDetector.AnalyzeSample(sample);
            }

            Assert.AreEqual(AudioMessageDetectionState.SearchingForPrelude, audioMessageDetector.State);
        }

        // scenario 8: 
        // sample => 1709, 1750, 1764, 1764, 1764
        //           1709, 1750, 1764, 1764, 1764
        //           1709, 1750, 1764, 1764, 1764
        //           1709, 1750, 1764, 1764, 1764
        //           1709, 1750, 1764, 1764, 1764
        //           1791, 1750, 1764, 1764, 1764
        //           1791, 1750, 1764, 1764, 1764
        //           1791, 1750, 1764, 1764, 1764
        //           1791, 1750, 1764, 1764, 1764
        //           1791, 1750, 1764, 1764, 1764
        //           1891, 1750, 1764, 1764, 1764
        //           1891, 1750, 1764, 1764, 1764
        //           1891, 1750, 1764, 1764, 1764
        //           1891, 1750, 1764, 1764, 1764
        //           1891, 1750, 1764, 1764, 1764
        // tone step 100
        // tone variation tolerance of 10
        // tone minimum repetition of 3
        // prelude configuration 1700, 1800, 1900
        [TestMethod]
        public void given_scenario_8_above_should_change_state_to_listening_to_message()
        {
            List<double[]> samples = new List<double[]>();
            samples.Add(new double[] { 1709, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1709, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1709, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1709, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1709, 1750, 1764, 1764, 1764 });

            samples.Add(new double[] { 1791, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1791, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1791, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1791, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1791, 1750, 1764, 1764, 1764 });

            samples.Add(new double[] { 1891, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1891, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1891, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1891, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1891, 1750, 1764, 1764, 1764 });

            AudioMessageDetector audioMessageDetector = new AudioMessageDetector(
                new AudioPartConfiguration() { Frequencies = new double[] { 1700, 1800, 1900 } },
                new AudioPartConfiguration() { Frequencies = new double[] { 0 } },
                100, 10, 3, 2, 3);
            foreach (var sample in samples)
            {
                audioMessageDetector.AnalyzeSample(sample);
            }

            Assert.AreEqual(AudioMessageDetectionState.ListeningMessage, audioMessageDetector.State);
        }

        // scenario 9: 
        // sample => 1709, 1750, 1764, 1764, 1764
        //           1709, 1750, 1764, 1764, 1764
        //           1709, 1750, 1764, 1764, 1764
        //           1000, 1750, 1764, 1764, 1764
        //           1000, 1750, 1764, 1764, 1764
        //           1791, 1750, 1764, 1764, 1764
        //           1791, 1750, 1764, 1764, 1764
        //           1791, 1750, 1764, 1764, 1764
        //           1791, 1750, 1764, 1764, 1764
        //           1791, 1750, 1764, 1764, 1764
        //           1891, 1750, 1764, 1764, 1764
        //           1891, 1750, 1764, 1764, 1764
        //           1891, 1750, 1764, 1764, 1764
        //           1891, 1750, 1764, 1764, 1764
        //           1891, 1750, 1764, 1764, 1764
        // tone step 100
        // tone variation tolerance of 10
        // tone minimum repetition of 3
        // prelude configuration 1700, 1800, 1900
        // prelude fault tolerance of 2 samples 
        [TestMethod]
        public void given_scenario_9_above_should_not_change_state_to_listening_to_message()
        {
            List<double[]> samples = new List<double[]>();
            samples.Add(new double[] { 1709, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1709, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1709, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1000, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1000, 1750, 1764, 1764, 1764 });

            samples.Add(new double[] { 1791, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1791, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1791, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1791, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1791, 1750, 1764, 1764, 1764 });

            samples.Add(new double[] { 1891, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1891, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1891, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1891, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1891, 1750, 1764, 1764, 1764 });

            AudioMessageDetector audioMessageDetector = new AudioMessageDetector(
                new AudioPartConfiguration() { Frequencies = new double[] { 1700, 1800, 1900 } },
                new AudioPartConfiguration() { Frequencies = new double[] { 0 } },
                100, 10, 3, 2, 3);
            foreach (var sample in samples)
            {
                audioMessageDetector.AnalyzeSample(sample);
            }

            Assert.AreEqual(AudioMessageDetectionState.SearchingForPrelude, audioMessageDetector.State);
        }

        // scenario 10: 
        // sample => 1709, 1750, 1764, 1764, 1764
        //           1709, 1750, 1764, 1764, 1764
        //           1709, 1750, 1764, 1764, 1764
        //           1791, 1750, 1764, 1764, 1764
        //           1791, 1750, 1764, 1764, 1764
        //           1791, 1750, 1764, 1764, 1764
        //           1891, 1750, 1764, 1764, 1764
        //           1891, 1750, 1764, 1764, 1764
        //           1891, 1750, 1764, 1764, 1764
        //           1600, 1750, 1764, 1764, 1764
        //           1600, 1750, 1764, 1764, 1764
        //           1600, 1750, 1764, 1764, 1764
        // tone step 100
        // tone variation tolerance of 10
        // tone minimum repetition of 3
        // prelude configuration 1700, 1800, 1900
        // finale configuration 1600
        [TestMethod]
        public void given_scenario_10_above_should_change_status_to_listening_to_message_and_back_to_search_for_prelude()
        {
            List<double[]> samples = new List<double[]>();
            samples.Add(new double[] { 1709, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1709, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1709, 1750, 1764, 1764, 1764 });

            samples.Add(new double[] { 1791, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1791, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1791, 1750, 1764, 1764, 1764 });

            samples.Add(new double[] { 1891, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1891, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1891, 1750, 1764, 1764, 1764 });

            samples.Add(new double[] { 1600, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1600, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1600, 1750, 1764, 1764, 1764 });

            bool stateWasListeningMessageAtSomePoint = false;
            AudioMessageDetector audioMessageDetector = new AudioMessageDetector(
                new AudioPartConfiguration() { Part = AudioPart.Prelude, Frequencies = new double[] { 1700, 1800, 1900 } },
                new AudioPartConfiguration() { Part = AudioPart.Finale, Frequencies = new double[] { 1600 } },
                100, 10, 3, 2, 3);
            foreach (var sample in samples)
            {
                audioMessageDetector.AnalyzeSample(sample);

                if (!stateWasListeningMessageAtSomePoint)
                    stateWasListeningMessageAtSomePoint = audioMessageDetector.State == AudioMessageDetectionState.ListeningMessage;
            }

            Assert.IsTrue(stateWasListeningMessageAtSomePoint);
            Assert.AreEqual(AudioMessageDetectionState.SearchingForPrelude, audioMessageDetector.State);
        }

        // scenario 11: 
        // sample => 1709, 1750, 1764, 1764, 1764
        //           1709, 1750, 1764, 1764, 1764
        //           1709, 1750, 1764, 1764, 1764
        //           1791, 1750, 1764, 1764, 1764
        //           1791, 1750, 1764, 1764, 1764
        //           1791, 1750, 1764, 1764, 1764
        //           1891, 1750, 1764, 1764, 1764
        //           1891, 1750, 1764, 1764, 1764
        //           1891, 1750, 1764, 1764, 1764
        //           1500, 1750, 1764, 1764, 1764
        //           1500, 1750, 1764, 1764, 1764
        //           1500, 1750, 1764, 1764, 1764
        //           1600, 1750, 1764, 1764, 1764
        //           1600, 1750, 1764, 1764, 1764
        //           1600, 1750, 1764, 1764, 1764
        // tone step 100
        // tone variation tolerance of 10
        // tone minimum repetition of 3
        // prelude configuration 1700, 1800, 1900
        // finale configuration 1600
        [TestMethod]
        public void given_scenario_11_above_should_launch_message_of_1500()
        {
            List<double[]> samples = new List<double[]>();
            samples.Add(new double[] { 1709, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1709, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1709, 1750, 1764, 1764, 1764 });

            samples.Add(new double[] { 1791, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1791, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1791, 1750, 1764, 1764, 1764 });

            samples.Add(new double[] { 1891, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1891, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1891, 1750, 1764, 1764, 1764 });

            samples.Add(new double[] { 1500, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1500, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1500, 1750, 1764, 1764, 1764 });

            samples.Add(new double[] { 1600, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1600, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1600, 1750, 1764, 1764, 1764 });

            AudioMessageDetector audioMessageDetector = new AudioMessageDetector(
                new AudioPartConfiguration() { Part = AudioPart.Prelude, Frequencies = new double[] { 1700, 1800, 1900 } },
                new AudioPartConfiguration() { Part = AudioPart.Finale, Frequencies = new double[] { 1600 } },
                100, 10, 3, 2, 3);

            AudioMessageTestSubscriber.Subscribe(audioMessageDetector);

            foreach (var sample in samples)
            {
                audioMessageDetector.AnalyzeSample(sample);
            }

            Assert.IsNotNull(AudioMessageTestSubscriber.AudioMessages);
            Assert.IsTrue(AudioMessageTestSubscriber.AudioMessages.Count > 0);
            Assert.AreEqual(1500, AudioMessageTestSubscriber.AudioMessages[0].Frequencies[0]);
        }


        // scenario 12: 
        // sample => 1791, 1750, 1750, 1750, 1750
        //           1791, 1750, 1750, 1750, 1750
        //           1791, 1750, 1750, 1750, 1750
        //           1891, 1750, 1750, 1750, 1750
        //           1891, 1750, 1750, 1750, 1750
        //           1891, 1750, 1750, 1750, 1750
        //           1500, 1750, 1750, 1750, 1750
        //           1750, 1750, 1750, 1750, 1750
        //           1750, 1750, 1750, 1750, 1750
        //           1500, 1750, 1750, 1750, 1750
        //           1500, 1750, 1750, 1750, 1750
        //           1600, 1750, 1750, 1750, 1750
        //           1600, 1750, 1750, 1750, 1750
        //           1600, 1750, 1750, 1750, 1750
        // tone step 100
        // tone variation tolerance of 10
        // tone minimum repetition of 3
        // prelude configuration 1700, 1800, 1900
        // finale configuration 1600
        [TestMethod]
        public void given_scenario_12_above_should_launch_a_message_with_empty_frequencies()
        {
            List<double[]> samples = new List<double[]>();
            samples.Add(new double[] { 1791, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1791, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1791, 1750, 1764, 1764, 1764 });

            samples.Add(new double[] { 1891, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1891, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1891, 1750, 1764, 1764, 1764 });

            samples.Add(new double[] { 1500, 1750, 1764, 1764, 1764 });

            samples.Add(new double[] { 1750, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1750, 1750, 1764, 1764, 1764 });

            samples.Add(new double[] { 1500, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1500, 1750, 1764, 1764, 1764 });

            samples.Add(new double[] { 1600, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1600, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1600, 1750, 1764, 1764, 1764 });

            AudioMessageDetector audioMessageDetector = new AudioMessageDetector(
                new AudioPartConfiguration() { Part = AudioPart.Prelude, Frequencies = new double[] { 1800, 1900 } },
                new AudioPartConfiguration() { Part = AudioPart.Finale, Frequencies = new double[] { 1600 } },
                100, 10, 3, 2, 3);

            AudioMessageTestSubscriber.Subscribe(audioMessageDetector);

            foreach (var sample in samples)
            {
                audioMessageDetector.AnalyzeSample(sample);
            }

            Assert.IsNotNull(AudioMessageTestSubscriber.AudioMessages);
            Assert.AreEqual(1, AudioMessageTestSubscriber.AudioMessages.Count);
            Assert.AreEqual(0, AudioMessageTestSubscriber.AudioMessages[0].Frequencies.Count);
        }

        // scenario 13: 
        // sample => 1791, 1750, 1750, 1750, 1750
        //           1791, 1750, 1750, 1750, 1750
        //           1791, 1750, 1750, 1750, 1750
        //           1891, 1750, 1750, 1750, 1750
        //           1891, 1750, 1750, 1750, 1750
        //           1891, 1750, 1750, 1750, 1750
        //           1750, 1750, 1750, 1750, 1750
        //           1750, 1750, 1750, 1750, 1750
        //           1750, 1750, 1750, 1750, 1750
        //           1500, 1750, 1750, 1750, 1750
        //           1500, 1750, 1750, 1750, 1750
        //           1500, 1750, 1750, 1750, 1750
        //           1600, 1750, 1750, 1750, 1750
        //           1600, 1750, 1750, 1750, 1750
        //           1600, 1750, 1750, 1750, 1750
        // tone step 100
        // tone variation tolerance of 10
        // tone minimum repetition of 3
        // prelude configuration 1700, 1800, 1900
        // finale configuration 1600
        [TestMethod]
        public void given_scenario_13_above_should_not_launch_any_messages_and_keep_searching_for_prelude()
        {
            List<double[]> samples = new List<double[]>();
            samples.Add(new double[] { 1791, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1791, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1791, 1750, 1764, 1764, 1764 });

            samples.Add(new double[] { 1891, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1891, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1891, 1750, 1764, 1764, 1764 });

            samples.Add(new double[] { 1750, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1750, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1750, 1750, 1764, 1764, 1764 });

            samples.Add(new double[] { 1500, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1500, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1500, 1750, 1764, 1764, 1764 });

            samples.Add(new double[] { 1600, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1600, 1750, 1764, 1764, 1764 });
            samples.Add(new double[] { 1600, 1750, 1764, 1764, 1764 });

            AudioMessageDetector audioMessageDetector = new AudioMessageDetector(
                new AudioPartConfiguration() { Part = AudioPart.Prelude, Frequencies = new double[] { 1800, 1900 } },
                new AudioPartConfiguration() { Part = AudioPart.Finale, Frequencies = new double[] { 1600 } },
                100, 10, 3, 2, 3);

            AudioMessageTestSubscriber.Subscribe(audioMessageDetector);

            foreach (var sample in samples)
            {
                audioMessageDetector.AnalyzeSample(sample);
            }

            Assert.IsNotNull(AudioMessageTestSubscriber.AudioMessages);
            Assert.AreEqual(0, AudioMessageTestSubscriber.AudioMessages.Count);
            Assert.AreEqual(AudioMessageDetectionState.SearchingForPrelude, audioMessageDetector.State);
        }

        // scenario 14: 
        // samples 1
        // tone step 100
        // tone variation tolerance of 40
        // tone minimum repetition of 5
        // prelude configuration 1000, 1100, 1200
        // finale configuration 1400
        [TestMethod]
        public void given_scenario_14_above_launch_message_of_1300()
        {
            List<double[]> samples = new List<double[]>();
            samples = GetTestSamples_1();

            AudioMessageDetector audioMessageDetector = new AudioMessageDetector(
                new AudioPartConfiguration() { Part = AudioPart.Prelude, Frequencies = new double[] { 1000, 1100, 1200 } },
                new AudioPartConfiguration() { Part = AudioPart.Finale, Frequencies = new double[] { 1400 } },
                100, 40, 5, 2, 3);

            AudioMessageTestSubscriber.Subscribe(audioMessageDetector);

            foreach (var sample in samples)
            {
                audioMessageDetector.AnalyzeSample(sample);
            }

            Assert.IsNotNull(AudioMessageTestSubscriber.AudioMessages);
            Assert.AreEqual(1, AudioMessageTestSubscriber.AudioMessages.Count);
            Assert.AreEqual(1400, AudioMessageTestSubscriber.AudioMessages[0].Frequencies[0]);
        }

        /// <summary>
        /// Based on a true recording.
        /// Returns the five peaks frequencies.
        /// </summary>
        /// <returns></returns>
        private List<double[]> GetTestSamples_1()
        {
            List<double[]> samples = new List<double[]>();

            #region populate samples
            samples.Add(new double[] { 1837.5, 1837.5, 1837.5, 1837.5, 1837.5 });
            samples.Add(new double[] { 1837.5, 1837.5, 1837.5, 1837.5, 1837.5 });
            samples.Add(new double[] { 1837.5, 1837.5, 1837.5, 1837.5, 1837.5 });
            samples.Add(new double[] { 1837.5, 1837.5, 1837.5, 1837.5, 1837.5 });
            samples.Add(new double[] { 1002.27, 1002.27, 1025.58, 1025.58, 1050 });
            samples.Add(new double[] { 1002.27, 1002.27, 1025.58, 1025.58, 1050 });
            samples.Add(new double[] { 1002.27, 1002.27, 1025.58, 1025.58, 1050 });
            samples.Add(new double[] { 980, 1002.27, 1025.58, 1025.58, 1050 });
            samples.Add(new double[] { 1002.27, 1002.27, 1025.58, 1025.58, 1050 });
            samples.Add(new double[] { 1002.27, 1002.27, 1025.58, 1025.58, 1050 });
            samples.Add(new double[] { 1002.27, 1002.27, 1025.58, 1025.58, 1050 });
            samples.Add(new double[] { 1002.27, 1002.27, 1025.58, 1025.58, 1050 });
            samples.Add(new double[] { 1102.5, 1102.5, 1130.77, 1130.77, 1130.77 });
            samples.Add(new double[] { 1102.5, 1102.5, 1130.77, 1130.77, 1130.77 });
            samples.Add(new double[] { 1102.5, 1130.77, 1130.77, 1130.77, 1160.53 });
            samples.Add(new double[] { 1102.5, 1102.5, 1130.77, 1130.77, 1130.77 });
            samples.Add(new double[] { 1102.5, 1102.5, 1130.77, 1130.77, 1130.77 });
            samples.Add(new double[] { 1102.5, 1102.5, 1130.77, 1130.77, 1130.77 });
            samples.Add(new double[] { 1102.5, 1102.5, 1130.77, 1130.77, 1160.53 });
            samples.Add(new double[] { 1102.5, 1102.5, 1130.77, 1130.77, 1130.77 });
            samples.Add(new double[] { 1102.5, 1102.5, 1130.77, 1130.77, 1130.77 });
            samples.Add(new double[] { 1102.5, 1102.5, 1130.77, 1130.77, 1130.77 });
            samples.Add(new double[] { 1102.5, 1102.5, 1130.77, 1130.77, 1191.89 });
            samples.Add(new double[] { 1191.89, 1225, 1225, 1225, 1260 });
            samples.Add(new double[] { 1191.89, 1225, 1225, 1225, 1260 });
            samples.Add(new double[] { 1191.89, 1225, 1225, 1225, 1260 });
            samples.Add(new double[] { 1191.89, 1225, 1225, 1260, 1260 });
            samples.Add(new double[] { 1191.89, 1225, 1225, 1225, 1260 });
            samples.Add(new double[] { 1191.89, 1191.89, 1225, 1225, 1260 });
            samples.Add(new double[] { 1225, 1225, 1225, 1260, 1260 });
            samples.Add(new double[] { 1191.89, 1225, 1225, 1260, 1260 });
            samples.Add(new double[] { 1191.89, 1225, 1225, 1225, 1260 });
            samples.Add(new double[] { 1191.89, 1191.89, 1225, 1225, 1260 });
            samples.Add(new double[] { 1225, 1225, 1225, 1260, 1260 });
            samples.Add(new double[] { 1191.89, 1225, 1225, 1225, 1260 });
            samples.Add(new double[] { 1191.89, 1225, 1336.36, 1336.36, 1336.36 });
            samples.Add(new double[] { 1336.36, 1336.36, 1336.36, 1336.36, 1378.13 });
            samples.Add(new double[] { 1225, 1225, 1336.36, 1336.36, 1336.36 });
            samples.Add(new double[] { 1297.06, 1297.06, 1336.36, 1336.36, 1336.36 });
            samples.Add(new double[] { 1297.06, 1297.06, 1336.36, 1336.36, 1336.36 });
            samples.Add(new double[] { 1297.06, 1297.06, 1336.36, 1336.36, 1336.36 });
            samples.Add(new double[] { 1297.06, 1336.36, 1336.36, 1336.36, 1378.13 });
            samples.Add(new double[] { 1297.06, 1297.06, 1297.06, 1336.36, 1336.36 });
            samples.Add(new double[] { 1378.13, 1422.58, 1422.58, 1422.58, 1470 });
            samples.Add(new double[] { 1422.58, 1422.58, 1422.58, 1470, 1470 });
            samples.Add(new double[] { 1422.58, 1422.58, 1422.58, 1422.58, 1470 });
            samples.Add(new double[] { 1378.13, 1422.58, 1422.58, 1422.58, 1470 });
            samples.Add(new double[] { 1378.13, 1422.58, 1422.58, 1422.58, 1470 });
            samples.Add(new double[] { 1422.58, 1422.58, 1422.58, 1422.58, 1470 });
            samples.Add(new double[] { 1422.58, 1422.58, 1422.58, 1422.58, 1470 });
            samples.Add(new double[] { 1422.58, 1422.58, 1422.58, 1422.58, 1470 });
            samples.Add(new double[] { 1422.58, 1422.58, 1422.58, 1422.58, 1470 });
            samples.Add(new double[] { 1422.58, 1422.58, 1422.58, 1422.58, 1470 });
            samples.Add(new double[] { 1378.13, 1422.58, 1422.58, 1422.58, 1470 });
            samples.Add(new double[] { 1422.58, 1422.58, 1470, 1520.69, 1520.69 });

            #endregion

            return samples;
        }

        #region Real Samples
        //samples.Add(new double[] { 1422.58, 1422.58, 1422.58, 1520.69, 1520.69 });
        //    samples.Add(new double[] {1191.89, 1225, 1225, 1260, 1260});
        //    samples.Add(new double[] {1191.89, 1225, 1225, 1225, 1260});
        //    samples.Add(new double[] {1191.89, 1191.89, 1225, 1225, 1260});
        //    samples.Add(new double[] {1225, 1225, 1225, 1260, 1260});
        //    samples.Add(new double[] {1191.89, 1225, 1225, 1225, 1260});
        //    samples.Add(new double[] {1225, 1225, 1225, 1260, 1260});
        //    samples.Add(new double[] {1225, 1225, 1297.06, 1336.36, 1336.36});
        //    samples.Add(new double[] {1297.06, 1336.36, 1336.36, 1336.36, 1336.36});
        //    samples.Add(new double[] {1297.06, 1297.06, 1336.36, 1336.36, 1336.36});
        //    samples.Add(new double[] {1297.06, 1336.36, 1336.36, 1336.36, 1336.36});
        //    samples.Add(new double[] {1297.06, 1336.36, 1336.36, 1336.36, 1336.36});
        //    samples.Add(new double[] {1297.06, 1297.06, 1336.36, 1336.36, 1336.36});
        //    samples.Add(new double[] {1297.06, 1336.36, 1336.36, 1336.36, 1336.36});
        //    samples.Add(new double[] {1422.58, 1422.58, 1422.58, 1422.58, 1470});
        //    samples.Add(new double[] {1336.36, 1378.13, 1422.58, 1422.58, 1422.58});
        //    samples.Add(new double[] {1422.58, 1422.58, 1422.58, 1422.58, 1470});
        //    samples.Add(new double[] {1378.13, 1422.58, 1422.58, 1422.58, 1470});
        //    samples.Add(new double[] {1378.13, 1422.58, 1422.58, 1422.58, 1470});
        //    samples.Add(new double[] {1422.58, 1422.58, 1422.58, 1422.58, 1470});
        //    samples.Add(new double[] {1422.58, 1422.58, 1422.58, 1422.58, 1470});
        //    samples.Add(new double[] {1422.58, 1422.58, 1422.58, 1422.58, 1470});
        //    samples.Add(new double[] {1422.58, 1422.58, 1422.58, 1422.58, 1470});
        //    samples.Add(new double[] {1378.13, 1422.58, 1422.58, 1422.58, 1470});
        //    samples.Add(new double[] {1378.13, 1422.58, 1422.58, 1422.58, 1470});
        //    samples.Add(new double[] {1422.58, 1422.58, 1422.58, 1470, 1520.69});
        //    samples.Add(new double[] {1520.69, 1520.69, 1520.69, 1520.69, 1575});
        //    samples.Add(new double[] {1520.69, 1520.69, 1520.69, 1575, 1575});
        //    samples.Add(new double[] {1520.69, 1520.69, 1520.69, 1520.69, 1575});
        //    samples.Add(new double[] {1520.69, 1520.69, 1520.69, 1520.69, 1575});
        //    samples.Add(new double[] {1520.69, 1520.69, 1520.69, 1520.69, 1575});
        //    samples.Add(new double[] {1520.69, 1520.69, 1520.69, 1520.69, 1575});
        //    samples.Add(new double[] {1520.69, 1520.69, 1520.69, 1520.69, 1575});
        //    samples.Add(new double[] {1520.69, 1520.69, 1520.69, 1575, 1575});
        //    samples.Add(new double[] {1520.69, 1520.69, 1520.69, 1520.69, 1575});
        //    samples.Add(new double[] {1520.69, 1520.69, 1520.69, 1520.69, 1575});
        //    samples.Add(new double[] {1520.69, 1520.69, 1575, 1575, 1633.33});
        //    samples.Add(new double[] {1520.69, 1633.33, 1633.33, 1633.33, 1696.15});
        //    samples.Add(new double[] {1633.33, 1633.33, 1633.33, 1633.33, 1633.33});
        //    samples.Add(new double[] {1633.33, 1633.33, 1633.33, 1633.33, 1633.33});
        //    samples.Add(new double[] {1633.33, 1633.33, 1633.33, 1633.33, 1633.33});
        //    samples.Add(new double[] {1575, 1633.33, 1633.33, 1633.33, 1696.15});
        //    samples.Add(new double[] {1633.33, 1633.33, 1633.33, 1633.33, 1633.33});
        //    samples.Add(new double[] {1633.33, 1633.33, 1633.33, 1633.33, 1633.33});
        //    samples.Add(new double[] {1633.33, 1633.33, 1633.33, 1633.33, 1696.15});
        //    samples.Add(new double[] {1633.33, 1633.33, 1633.33, 1633.33, 1696.15});
        //    samples.Add(new double[] {1575, 1633.33, 1633.33, 1633.33, 1696.15});
        //    samples.Add(new double[] {1633.33, 1633.33, 1633.33, 1633.33, 1764});
        //    samples.Add(new double[] {1633.33, 1633.33, 1764, 1764, 1764});
        //    samples.Add(new double[] {1633.33, 1633.33, 1764, 1764, 1764});
        //    samples.Add(new double[] {1633.33, 1633.33, 1764, 1764, 1764});
        //    samples.Add(new double[] {1696.15, 1764, 1764, 1764, 1764});
        //    samples.Add(new double[] {1696.15, 1764, 1764, 1764, 1764});
        //    samples.Add(new double[] {1696.15, 1764, 1764, 1764, 1764});
        //    samples.Add(new double[] {1696.15, 1764, 1764, 1764, 1764});
        //    samples.Add(new double[] {1696.15, 1764, 1764, 1764, 1764});
        //    samples.Add(new double[] {1696.15, 1764, 1764, 1764, 1764});
        //    samples.Add(new double[] {1837.5, 1837.5, 1837.5, 1837.5, 1837.5});
        //    samples.Add(new double[] {1837.5, 1837.5, 1837.5, 1837.5, 1837.5});
        //    samples.Add(new double[] {1837.5, 1837.5, 1837.5, 1837.5, 1917.39});
        //    samples.Add(new double[] {1837.5, 1837.5, 1837.5, 1837.5, 1917.39});
        //    samples.Add(new double[] {1837.5, 1837.5, 1837.5, 1837.5, 1837.5});
        //    samples.Add(new double[] {1837.5, 1837.5, 1837.5, 1837.5, 1917.39});
        //    samples.Add(new double[] {1837.5, 1837.5, 1837.5, 1837.5, 1917.39});
        //    samples.Add(new double[] {1837.5, 1837.5, 1837.5, 1837.5, 1837.5});
        //    samples.Add(new double[] {1837.5, 1837.5, 1837.5, 1837.5, 1837.5});
        //    samples.Add(new double[] {1837.5, 1837.5, 1837.5, 1837.5, 1917.39});
        //    samples.Add(new double[] {1837.5, 1837.5, 1837.5, 1837.5, 1837.5});
        //    samples.Add(new double[] {1837.5, 1837.5, 1837.5, 1837.5, 1837.5});
        //    samples.Add(new double[] {1837.5, 1837.5, 1837.5, 1837.5, 1837.5});
        //    samples.Add(new double[] {1837.5, 1837.5, 1837.5, 1837.5, 1837.5});
        //    samples.Add(new double[] {1002.27, 1002.27, 1025.58, 1025.58, 1050});
        //    samples.Add(new double[] {1002.27, 1002.27, 1025.58, 1025.58, 1050});
        //    samples.Add(new double[] {1002.27, 1002.27, 1025.58, 1025.58, 1050});
        //    samples.Add(new double[] {980, 1002.27, 1025.58, 1025.58, 1050});
        //    samples.Add(new double[] {1002.27, 1002.27, 1025.58, 1025.58, 1050});
        //    samples.Add(new double[] {1002.27, 1002.27, 1025.58, 1025.58, 1050});
        //    samples.Add(new double[] {1002.27, 1002.27, 1025.58, 1025.58, 1050});
        //    samples.Add(new double[] {1002.27, 1002.27, 1025.58, 1025.58, 1050});
        //    samples.Add(new double[] {1102.5, 1102.5, 1130.77, 1130.77, 1130.77});
        //    samples.Add(new double[] {1102.5, 1102.5, 1130.77, 1130.77, 1130.77});
        //    samples.Add(new double[] {1102.5, 1130.77, 1130.77, 1130.77, 1160.53});
        //    samples.Add(new double[] {1102.5, 1102.5, 1130.77, 1130.77, 1130.77});
        //    samples.Add(new double[] {1102.5, 1102.5, 1130.77, 1130.77, 1130.77});
        //    samples.Add(new double[] {1102.5, 1102.5, 1130.77, 1130.77, 1130.77});
        //    samples.Add(new double[] {1102.5, 1102.5, 1130.77, 1130.77, 1160.53});
        //    samples.Add(new double[] {1102.5, 1102.5, 1130.77, 1130.77, 1130.77});
        //    samples.Add(new double[] {1102.5, 1102.5, 1130.77, 1130.77, 1130.77});
        //    samples.Add(new double[] {1102.5, 1102.5, 1130.77, 1130.77, 1130.77});
        //    samples.Add(new double[] {1102.5, 1102.5, 1130.77, 1130.77, 1191.89});
        //    samples.Add(new double[] {1191.89, 1225, 1225, 1225, 1260});
        //    samples.Add(new double[] {1191.89, 1225, 1225, 1225, 1260});
        //    samples.Add(new double[] {1191.89, 1225, 1225, 1225, 1260});
        //    samples.Add(new double[] {1191.89, 1225, 1225, 1260, 1260});
        //    samples.Add(new double[] {1191.89, 1225, 1225, 1225, 1260});
        //    samples.Add(new double[] {1191.89, 1191.89, 1225, 1225, 1260});
        //    samples.Add(new double[] {1225, 1225, 1225, 1260, 1260});
        //    samples.Add(new double[] {1191.89, 1225, 1225, 1260, 1260});
        //    samples.Add(new double[] {1191.89, 1225, 1225, 1225, 1260});
        //    samples.Add(new double[] {1191.89, 1191.89, 1225, 1225, 1260});
        //    samples.Add(new double[] {1225, 1225, 1225, 1260, 1260});
        //    samples.Add(new double[] {1191.89, 1225, 1225, 1225, 1260});
        //    samples.Add(new double[] {1191.89, 1225, 1336.36, 1336.36, 1336.36});
        //    samples.Add(new double[] {1336.36, 1336.36, 1336.36, 1336.36, 1378.13});
        //    samples.Add(new double[] {1225, 1225, 1336.36, 1336.36, 1336.36});
        //    samples.Add(new double[] {1297.06, 1297.06, 1336.36, 1336.36, 1336.36});
        //    samples.Add(new double[] {1297.06, 1297.06, 1336.36, 1336.36, 1336.36});
        //    samples.Add(new double[] {1297.06, 1297.06, 1336.36, 1336.36, 1336.36});
        //    samples.Add(new double[] {1297.06, 1336.36, 1336.36, 1336.36, 1378.13});
        //    samples.Add(new double[] {1297.06, 1297.06, 1297.06, 1336.36, 1336.36});
        //    samples.Add(new double[] {1378.13, 1422.58, 1422.58, 1422.58, 1470});
        //    samples.Add(new double[] {1422.58, 1422.58, 1422.58, 1470, 1470});
        //    samples.Add(new double[] {1422.58, 1422.58, 1422.58, 1422.58, 1470});
        //    samples.Add(new double[] {1378.13, 1422.58, 1422.58, 1422.58, 1470});
        //    samples.Add(new double[] {1378.13, 1422.58, 1422.58, 1422.58, 1470});
        //    samples.Add(new double[] {1422.58, 1422.58, 1422.58, 1422.58, 1470});
        //    samples.Add(new double[] {1422.58, 1422.58, 1422.58, 1422.58, 1470});
        //    samples.Add(new double[] {1422.58, 1422.58, 1422.58, 1422.58, 1470});
        //    samples.Add(new double[] {1422.58, 1422.58, 1422.58, 1422.58, 1470});
        //    samples.Add(new double[] {1422.58, 1422.58, 1422.58, 1422.58, 1470});
        //    samples.Add(new double[] {1378.13, 1422.58, 1422.58, 1422.58, 1470});
        //    samples.Add(new double[] {1422.58, 1422.58, 1470, 1520.69, 1520.69});
        //    samples.Add(new double[] {1422.58, 1422.58, 1422.58, 1520.69, 1520.69});
        //    samples.Add(new double[] {1520.69, 1520.69, 1520.69, 1520.69, 1575});
        //    samples.Add(new double[] {1520.69, 1520.69, 1520.69, 1520.69, 1575});
        //    samples.Add(new double[] {1520.69, 1520.69, 1520.69, 1520.69, 1575});
        //    samples.Add(new double[] {1520.69, 1520.69, 1520.69, 1520.69, 1575});
        //    samples.Add(new double[] {1520.69, 1520.69, 1520.69, 1520.69, 1575});
        //    samples.Add(new double[] {1520.69, 1520.69, 1520.69, 1575, 1575});
        //    samples.Add(new double[] {1520.69, 1520.69, 1520.69, 1520.69, 1575});
        //    samples.Add(new double[] {1520.69, 1520.69, 1520.69, 1520.69, 1575});
        //    samples.Add(new double[] {1520.69, 1520.69, 1520.69, 1520.69, 1575});
        //    samples.Add(new double[] {1520.69, 1520.69, 1575, 1633.33, 1633.33});
        //    samples.Add(new double[] {1520.69, 1633.33, 1633.33, 1633.33, 1696.15});
        //    samples.Add(new double[] {1633.33, 1633.33, 1633.33, 1633.33, 1633.33});
        //    samples.Add(new double[] {1633.33, 1633.33, 1633.33, 1633.33, 1633.33});
        //    samples.Add(new double[] {1633.33, 1633.33, 1633.33, 1633.33, 1633.33});
        //    samples.Add(new double[] {1575, 1633.33, 1633.33, 1633.33, 1696.15});
        //    samples.Add(new double[] {1633.33, 1633.33, 1633.33, 1633.33, 1633.33});
        //    samples.Add(new double[] {1633.33, 1633.33, 1633.33, 1633.33, 1633.33});
        //    samples.Add(new double[] {1633.33, 1633.33, 1633.33, 1633.33, 1633.33});
        //    samples.Add(new double[] {1633.33, 1633.33, 1633.33, 1633.33, 1696.15});
        //    samples.Add(new double[] {1633.33, 1633.33, 1633.33, 1633.33, 1696.15});
        //    samples.Add(new double[] {1633.33, 1633.33, 1633.33, 1633.33, 1633.33});
        //    samples.Add(new double[] {1633.33, 1764, 1764, 1764, 1764});
        //    samples.Add(new double[] {1696.15, 1764, 1764, 1764, 1764});
        //    samples.Add(new double[] {1696.15, 1764, 1764, 1764, 1764});
        //    samples.Add(new double[] {1696.15, 1764, 1764, 1764, 1764});
        //    samples.Add(new double[] {1696.15, 1764, 1764, 1764, 1764});
        //    samples.Add(new double[] {1696.15, 1764, 1764, 1764, 1764});
        //    samples.Add(new double[] {1696.15, 1764, 1764, 1764, 1764});
        //    samples.Add(new double[] {1696.15, 1764, 1764, 1764, 1764});
        //    samples.Add(new double[] {1696.15, 1764, 1764, 1764, 1837.5});
        //    samples.Add(new double[] {1764, 1837.5, 1837.5, 1837.5, 1837.5});
        //    samples.Add(new double[] {1837.5, 1837.5, 1837.5, 1837.5, 1917.39});
        //    samples.Add(new double[] {1837.5, 1837.5, 1837.5, 1837.5, 1917.39});
        //    samples.Add(new double[] {1837.5, 1837.5, 1837.5, 1837.5, 1837.5});
        //    samples.Add(new double[] {1837.5, 1837.5, 1837.5, 1837.5, 1837.5});
        //    samples.Add(new double[] {1837.5, 1837.5, 1837.5, 1837.5, 1917.39});
        //    samples.Add(new double[] {1837.5, 1837.5, 1837.5, 1837.5, 1917.39});
        //    samples.Add(new double[] {1837.5, 1837.5, 1837.5, 1837.5, 1837.5});
        //    samples.Add(new double[] {1837.5, 1837.5, 1837.5, 1837.5, 1917.39});
        //    samples.Add(new double[] {1837.5, 1837.5, 1837.5, 1917.39, 1917.39});
        //    samples.Add(new double[] {1002.27, 1025.58, 1025.58, 1075.61, 1130.77});
        //    samples.Add(new double[] {980, 1002.27, 1002.27, 1025.58, 1025.58});
        //    samples.Add(new double[] {1002.27, 1002.27, 1025.58, 1025.58, 1050});
        //    samples.Add(new double[] {980, 1002.27, 1002.27, 1025.58, 1025.58});
        //    samples.Add(new double[] {980, 1002.27, 1025.58, 1025.58, 1050});
        //    samples.Add(new double[] {1002.27, 1002.27, 1025.58, 1025.58, 1050});
        //    samples.Add(new double[] {1002.27, 1002.27, 1025.58, 1025.58, 1050});
        //    samples.Add(new double[] {1002.27, 1002.27, 1025.58, 1025.58, 1050});
        //    samples.Add(new double[] {1002.27, 1002.27, 1025.58, 1025.58, 1050});
        //    samples.Add(new double[] {980, 1002.27, 1025.58, 1025.58, 1050});
        //    samples.Add(new double[] {1002.27, 1002.27, 1025.58, 1025.58, 1050});
        //    samples.Add(new double[] {1102.5, 1102.5, 1130.77, 1130.77, 1130.77});
        //    samples.Add(new double[] {1102.5, 1102.5, 1130.77, 1130.77, 1130.77});
        //    samples.Add(new double[] {1102.5, 1102.5, 1130.77, 1130.77, 1130.77});
        //    samples.Add(new double[] {1102.5, 1102.5, 1130.77, 1130.77, 1130.77});
        //    samples.Add(new double[] {1102.5, 1102.5, 1130.77, 1130.77, 1160.53});
        //    samples.Add(new double[] {1102.5, 1102.5, 1130.77, 1130.77, 1130.77});
        //    samples.Add(new double[] {1102.5, 1102.5, 1130.77, 1130.77, 1130.77});
        //    samples.Add(new double[] {1102.5, 1102.5, 1130.77, 1130.77, 1160.53});
        //    samples.Add(new double[] {1102.5, 1102.5, 1130.77, 1130.77, 1130.77});
        //    samples.Add(new double[] {1102.5, 1102.5, 1130.77, 1130.77, 1130.77});
        //    samples.Add(new double[] {1102.5, 1102.5, 1130.77, 1130.77, 1160.53});
        //    samples.Add(new double[] {1102.5, 1130.77, 1225, 1225, 1225});
        //    samples.Add(new double[] {1130.77, 1225, 1225, 1225, 1260});
        //    samples.Add(new double[] {1191.89, 1225, 1225, 1260, 1260});
        //    samples.Add(new double[] {1191.89, 1225, 1225, 1225, 1260});
        //    samples.Add(new double[] {1191.89, 1191.89, 1225, 1225, 1260});
        //    samples.Add(new double[] {1225, 1225, 1225, 1260, 1260});
        //    samples.Add(new double[] {1191.89, 1225, 1225, 1225, 1260});
        //    samples.Add(new double[] {1191.89, 1225, 1225, 1225, 1260});
        //    samples.Add(new double[] {1191.89, 1225, 1225, 1260, 1260});
        //    samples.Add(new double[] {1191.89, 1225, 1225, 1225, 1260});
        //    samples.Add(new double[] {1191.89, 1191.89, 1225, 1225, 1260});
        //    samples.Add(new double[] {1225, 1225, 1260, 1260, 1297.06});
        //    samples.Add(new double[] {1225, 1225, 1225, 1260, 1336.36});
        //    samples.Add(new double[] {1297.06, 1336.36, 1336.36, 1336.36, 1336.36});
        //    samples.Add(new double[] {1297.06, 1297.06, 1336.36, 1336.36, 1336.36});
        //    samples.Add(new double[] {1297.06, 1336.36, 1336.36, 1336.36, 1378.13});
        //    samples.Add(new double[] {1297.06, 1336.36, 1336.36, 1336.36, 1336.36});
        //    samples.Add(new double[] {1297.06, 1336.36, 1336.36, 1336.36, 1336.36});
        //    samples.Add(new double[] {1297.06, 1336.36, 1336.36, 1336.36, 1336.36});
        //    samples.Add(new double[] {1297.06, 1297.06, 1336.36, 1336.36, 1336.36});
        //    samples.Add(new double[] {1336.36, 1336.36, 1336.36, 1422.58, 1422.58});
        //    samples.Add(new double[] {1422.58, 1422.58, 1422.58, 1422.58, 1470});
        //    samples.Add(new double[] {1378.13, 1422.58, 1422.58, 1422.58, 1470});
        //    samples.Add(new double[] {1422.58, 1422.58, 1422.58, 1422.58, 1470});
        //    samples.Add(new double[] {1422.58, 1422.58, 1422.58, 1422.58, 1470});
        //    samples.Add(new double[] {1378.13, 1422.58, 1422.58, 1422.58, 1470});
        //    samples.Add(new double[] {1378.13, 1422.58, 1422.58, 1422.58, 1470});
        //    samples.Add(new double[] {1422.58, 1422.58, 1422.58, 1422.58, 1470});
        //    samples.Add(new double[] {1378.13, 1422.58, 1422.58, 1422.58, 1470});
        //    samples.Add(new double[] {1378.13, 1422.58, 1422.58, 1422.58, 1470});
        //    samples.Add(new double[] {1422.58, 1422.58, 1422.58, 1422.58, 1470});
        //    samples.Add(new double[] {1422.58, 1422.58, 1422.58, 1422.58, 1470});
        //    samples.Add(new double[] {1422.58, 1422.58, 1520.69, 1520.69, 1520.69});
        //    samples.Add(new double[] {1422.58, 1422.58, 1520.69, 1520.69, 1520.69});
        //    samples.Add(new double[] {1422.58, 1422.58, 1520.69, 1520.69, 1520.69});
        //    samples.Add(new double[] {1520.69, 1520.69, 1520.69, 1520.69, 1575});
        //    samples.Add(new double[] {1520.69, 1520.69, 1520.69, 1575, 1575});
        //    samples.Add(new double[] {1520.69, 1520.69, 1520.69, 1520.69, 1575});
        //    samples.Add(new double[] {1520.69, 1520.69, 1520.69, 1520.69, 1575});
        //    samples.Add(new double[] {1520.69, 1520.69, 1520.69, 1520.69, 1575});
        //    samples.Add(new double[] {1520.69, 1520.69, 1520.69, 1520.69, 1575});
        //    samples.Add(new double[] {1520.69, 1520.69, 1520.69, 1520.69, 1575});
        //    samples.Add(new double[] {1520.69, 1520.69, 1520.69, 1520.69, 1575});
        //    samples.Add(new double[] {1520.69, 1633.33, 1633.33, 1633.33, 1633.33});
        //    samples.Add(new double[] {1520.69, 1575, 1633.33, 1633.33, 1633.33});
        //    samples.Add(new double[] {1520.69, 1633.33, 1633.33, 1633.33, 1633.33});
        //    samples.Add(new double[] {1575, 1633.33, 1633.33, 1633.33, 1696.15});
        //    samples.Add(new double[] {1575, 1633.33, 1633.33, 1633.33, 1696.15});
        //    samples.Add(new double[] {1575, 1633.33, 1633.33, 1633.33, 1696.15});
        //    samples.Add(new double[] {1633.33, 1633.33, 1633.33, 1633.33, 1633.33});
        //    samples.Add(new double[] {1633.33, 1633.33, 1633.33, 1633.33, 1633.33});
        //    samples.Add(new double[] {1633.33, 1633.33, 1633.33, 1633.33, 1696.15});
        //    samples.Add(new double[] {1575, 1633.33, 1633.33, 1633.33, 1696.15});
        //    samples.Add(new double[] {1633.33, 1633.33, 1633.33, 1633.33, 1696.15});
        //    samples.Add(new double[] {1633.33, 1633.33, 1633.33, 1633.33, 1764});
        //    samples.Add(new double[] {1633.33, 1633.33, 1633.33, 1633.33, 1764});
        //    samples.Add(new double[] {1633.33, 1633.33, 1633.33, 1633.33, 1764});
        //    samples.Add(new double[] {1633.33, 1633.33, 1633.33, 1633.33, 1764});
        //    samples.Add(new double[] {1633.33, 1633.33, 1633.33, 1633.33, 1764});
        //    samples.Add(new double[] {1696.15, 1764, 1764, 1764, 1764});
        //    samples.Add(new double[] {1696.15, 1764, 1764, 1764, 1764});
        //    samples.Add(new double[] {1696.15, 1764, 1764, 1764, 1764});
        //    samples.Add(new double[] {1696.15, 1764, 1764, 1837.5, 1837.5});
        //    samples.Add(new double[] {1696.15, 1764, 1837.5, 1837.5, 1837.5});
        //    samples.Add(new double[] {1837.5, 1837.5, 1837.5, 1837.5, 1917.39});
        //    samples.Add(new double[] {1837.5, 1837.5, 1837.5, 1837.5, 1917.39});
        //    samples.Add(new double[] {1837.5, 1837.5, 1837.5, 1837.5, 1837.5});
        //    samples.Add(new double[] {1837.5, 1837.5, 1837.5, 1837.5, 1837.5});
        //    samples.Add(new double[] {1837.5, 1837.5, 1837.5, 1837.5, 1837.5});
        //    samples.Add(new double[] {1837.5, 1837.5, 1837.5, 1837.5, 1837.5});
        //    samples.Add(new double[] {1837.5, 1837.5, 1837.5, 1837.5, 1837.5});
        //    samples.Add(new double[] {1837.5, 1837.5, 1837.5, 1837.5, 1917.39});
        //    samples.Add(new double[] {1837.5, 1837.5, 1837.5, 1837.5, 1917.39});
        //    samples.Add(new double[] {1837.5, 1837.5, 1837.5, 1837.5, 1837.5});
        //    samples.Add(new double[] {1002.27, 1025.58, 1025.58, 1050, 1050});
        //    samples.Add(new double[] {1002.27, 1025.58, 1025.58, 1050, 1050});
        //    samples.Add(new double[] {1002.27, 1025.58, 1050, 1050, 1075.61});
        //    samples.Add(new double[] {1002.27, 1025.58, 1050, 1050, 1075.61});
        //    samples.Add(new double[] {1002.27, 1002.27, 1025.58, 1025.58, 1050});
        //    samples.Add(new double[] {1002.27, 1002.27, 1025.58, 1025.58, 1050});
        //    samples.Add(new double[] {1002.27, 1002.27, 1025.58, 1025.58, 1050});
        //    samples.Add(new double[] {1002.27, 1002.27, 1025.58, 1025.58, 1050});
        //    samples.Add(new double[] {1002.27, 1002.27, 1025.58, 1025.58, 1050});
        //    samples.Add(new double[] {1002.27, 1002.27, 1025.58, 1025.58, 1050});
        //    samples.Add(new double[] {980, 1002.27, 1025.58, 1025.58, 1050});
        //    samples.Add(new double[] {1102.5, 1102.5, 1130.77, 1130.77, 1160.53});
        //    samples.Add(new double[] {1102.5, 1102.5, 1130.77, 1130.77, 1130.77});
        //    samples.Add(new double[] {1102.5, 1102.5, 1130.77, 1130.77, 1160.53});
        //    samples.Add(new double[] {1102.5, 1102.5, 1130.77, 1130.77, 1130.77});
        //    samples.Add(new double[] {1102.5, 1102.5, 1130.77, 1130.77, 1130.77});
        //    samples.Add(new double[] {1102.5, 1102.5, 1130.77, 1130.77, 1160.53});
        //    samples.Add(new double[] {1102.5, 1102.5, 1130.77, 1130.77, 1130.77});
        //    samples.Add(new double[] {1102.5, 1102.5, 1130.77, 1130.77, 1130.77});
        //    samples.Add(new double[] {1102.5, 1102.5, 1130.77, 1130.77, 1130.77});
        //    samples.Add(new double[] {1102.5, 1102.5, 1130.77, 1130.77, 1130.77});
        //    samples.Add(new double[] {1102.5, 1102.5, 1130.77, 1130.77, 1160.53});
        //    samples.Add(new double[] {1102.5, 1130.77, 1225, 1225, 1225});
        //    samples.Add(new double[] {1191.89, 1225, 1225, 1260, 1260});
        //    samples.Add(new double[] {1191.89, 1225, 1225, 1225, 1260});
        //    samples.Add(new double[] {1191.89, 1191.89, 1225, 1225, 1260});
        //    samples.Add(new double[] {1225, 1225, 1225, 1260, 1260});
        //    samples.Add(new double[] {1191.89, 1225, 1225, 1225, 1260});
        //    samples.Add(new double[] {1191.89, 1225, 1225, 1260, 1260});
        //    samples.Add(new double[] {1191.89, 1225, 1225, 1225, 1260});
        //    samples.Add(new double[] {1191.89, 1191.89, 1225, 1225, 1260});
        //    samples.Add(new double[] {1225, 1225, 1225, 1260, 1260});
        //    samples.Add(new double[] {1191.89, 1225, 1225, 1225, 1260});
        //    samples.Add(new double[] {1191.89, 1225, 1260, 1260, 1297.06});
        //    samples.Add(new double[] {1297.06, 1336.36, 1336.36, 1336.36, 1336.36});
        //    samples.Add(new double[] {1297.06, 1336.36, 1336.36, 1336.36, 1336.36});
        //    samples.Add(new double[] {1297.06, 1336.36, 1336.36, 1336.36, 1336.36});
        //    samples.Add(new double[] {1297.06, 1336.36, 1336.36, 1336.36, 1336.36});
        //    samples.Add(new double[] {1297.06, 1336.36, 1336.36, 1336.36, 1336.36});
        //    samples.Add(new double[] {1297.06, 1336.36, 1336.36, 1336.36, 1336.36});
        //    samples.Add(new double[] {1297.06, 1297.06, 1336.36, 1336.36, 1336.36});
        //    samples.Add(new double[] {1297.06, 1336.36, 1336.36, 1336.36, 1378.13});
        //    samples.Add(new double[] {1297.06, 1336.36, 1336.36, 1422.58, 1422.58});
        //    samples.Add(new double[] {1336.36, 1336.36, 1422.58, 1422.58, 1422.58});
        //    samples.Add(new double[] {1336.36, 1336.36, 1422.58, 1422.58, 1422.58});
        //    samples.Add(new double[] {1422.58, 1422.58, 1422.58, 1422.58, 1470});
        //    samples.Add(new double[] {1422.58, 1422.58, 1422.58, 1422.58, 1470});
        //    samples.Add(new double[] {1422.58, 1422.58, 1422.58, 1422.58, 1470});
        //    samples.Add(new double[] {1378.13, 1422.58, 1422.58, 1422.58, 1470});
        //    samples.Add(new double[] {1378.13, 1422.58, 1422.58, 1422.58, 1470});
        //    samples.Add(new double[] {1422.58, 1422.58, 1422.58, 1422.58, 1470});
        //    samples.Add(new double[] {1378.13, 1422.58, 1422.58, 1422.58, 1470});
        //    samples.Add(new double[] {1422.58, 1422.58, 1422.58, 1470, 1520.69});
        //    samples.Add(new double[] {1422.58, 1422.58, 1520.69, 1520.69, 1520.69});
        //    samples.Add(new double[] {1520.69, 1520.69, 1520.69, 1520.69, 1575});
        //    samples.Add(new double[] {1520.69, 1520.69, 1520.69, 1575, 1575});
        //    samples.Add(new double[] {1520.69, 1520.69, 1520.69, 1520.69, 1575});
        //    samples.Add(new double[] {1520.69, 1520.69, 1520.69, 1575, 1575});
        //    samples.Add(new double[] {1520.69, 1520.69, 1520.69, 1520.69, 1575});
        //    samples.Add(new double[] {1520.69, 1520.69, 1520.69, 1520.69, 1575});
        //    samples.Add(new double[] {1520.69, 1520.69, 1520.69, 1520.69, 1575});
        //    samples.Add(new double[] {1520.69, 1520.69, 1520.69, 1520.69, 1575});
        //    samples.Add(new double[] {1520.69, 1520.69, 1520.69, 1520.69, 1575});
        //    samples.Add(new double[] {1520.69, 1520.69, 1520.69, 1633.33, 1633.33});
        //    samples.Add(new double[] {1633.33, 1633.33, 1633.33, 1633.33, 1633.33});
        //    samples.Add(new double[] {1633.33, 1633.33, 1633.33, 1633.33, 1633.33});
        //    samples.Add(new double[] {1633.33, 1633.33, 1633.33, 1633.33, 1633.33});
        //    samples.Add(new double[] {1575, 1633.33, 1633.33, 1633.33, 1633.33});
        //    samples.Add(new double[] {1575, 1633.33, 1633.33, 1633.33, 1633.33});
        //    samples.Add(new double[] {1575, 1633.33, 1633.33, 1633.33, 1696.15});
        //    samples.Add(new double[] {1633.33, 1633.33, 1633.33, 1633.33, 1633.33});
        //    samples.Add(new double[] {1633.33, 1633.33, 1633.33, 1633.33, 1633.33});
        //    samples.Add(new double[] {1633.33, 1633.33, 1633.33, 1633.33, 1633.33});
        //    samples.Add(new double[] {1633.33, 1633.33, 1633.33, 1633.33, 1696.15});
        //    samples.Add(new double[] {1633.33, 1633.33, 1633.33, 1633.33, 1696.15});
        //    samples.Add(new double[] {1633.33, 1633.33, 1633.33, 1764, 1764});
        //    samples.Add(new double[] {1633.33, 1633.33, 1764, 1764, 1764});
        //    samples.Add(new double[] {1633.33, 1633.33, 1633.33, 1764, 1764});
        //    samples.Add(new double[] {1696.15, 1764, 1764, 1764, 1764});
        //    samples.Add(new double[] {1696.15, 1764, 1764, 1764, 1764});
        //    samples.Add(new double[] {1696.15, 1764, 1764, 1764, 1764});
        //    samples.Add(new double[] {1696.15, 1764, 1764, 1764, 1764});
        //    samples.Add(new double[] {1696.15, 1764, 1764, 1764, 1764});
        //    samples.Add(new double[] {1696.15, 1764, 1764, 1764, 1764});
        //    samples.Add(new double[] {1696.15, 1764, 1764, 1837.5, 1837.5});
        //    samples.Add(new double[] {1837.5, 1837.5, 1837.5, 1837.5, 1837.5});
        //    samples.Add(new double[] {1837.5, 1837.5, 1837.5, 1837.5, 1917.39});
        //    samples.Add(new double[] {1837.5, 1837.5, 1837.5, 1837.5, 1917.39});
        //    samples.Add(new double[] {1837.5, 1837.5, 1837.5, 1837.5, 1917.39});
        //    samples.Add(new double[] {1837.5, 1837.5, 1837.5, 1837.5, 1837.5});
        //    samples.Add(new double[] {1837.5, 1837.5, 1837.5, 1837.5, 1917.39});
        //    samples.Add(new double[] {1837.5, 1837.5, 1837.5, 1837.5, 1917.39});
        //    samples.Add(new double[] {1837.5, 1837.5, 1837.5, 1837.5, 1837.5});
        //    samples.Add(new double[] {1837.5, 1837.5, 1837.5, 1837.5, 1837.5});
        //    samples.Add(new double[] {1837.5, 1837.5, 1837.5, 1837.5, 1837.5});
        //    samples.Add(new double[] {1025.58, 1025.58, 1050, 1050, 1075.61});
        //    samples.Add(new double[] {980, 1002.27, 1025.58, 1025.58, 1050});
        //    samples.Add(new double[] {1002.27, 1002.27, 1025.58, 1025.58, 1050});
        //    samples.Add(new double[] {1002.27, 1002.27, 1025.58, 1025.58, 1050});
        //    samples.Add(new double[] {1002.27, 1002.27, 1025.58, 1025.58, 1050});
        //    samples.Add(new double[] {1002.27, 1002.27, 1025.58, 1025.58, 1050});
        //    samples.Add(new double[] {980, 1002.27, 1002.27, 1025.58, 1025.58});
        //    samples.Add(new double[] {1002.27, 1002.27, 1025.58, 1025.58, 1050});
        //    samples.Add(new double[] {1002.27, 1002.27, 1025.58, 1025.58, 1050});

        #endregion
    }
}
