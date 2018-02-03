using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PS.QRA.Test
{
    [TestClass]
    public class SignalDetectorTest
    {
        [TestMethod]
        public void given_five_frequency_occurrences_should_register_a_signal_with_the_average_frenquency()
        {
            int[] frequencyOccurrences = new int[] { 440, 440, 440, 440, 440 };
        }
    }
}
