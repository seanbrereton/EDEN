using Microsoft.VisualStudio.TestTools.UnitTesting;
using EDEN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDEN.Tests {
    [TestClass()]
    public class NeuralNetTests {

        public bool AllWithinRange(float[] outputs) {
            foreach (float output in outputs)
                if (output < -1 || output > 1)
                    return false;
            return true;
        }

        public bool FFTestBase(float[] inputs, int hCount, int hSize, int outSize) {
            NeuralNet network = new NeuralNet(inputs.Length, hCount, hSize, outSize);

            return AllWithinRange(network.FeedForward(inputs));
        }

        [TestMethod()]
        public void FeedForwardTest1() {
            float[] inputs = { 0, 0, 0 };
            Assert.IsTrue(FFTestBase(inputs, 3, 3, 3));
        }

        [TestMethod()]
        public void FeedForwardTest2() {
            float[] inputs = { 1, -1, 1 };
            Assert.IsTrue(FFTestBase(inputs, 4, 4, 3));
        }

        [TestMethod()]
        public void FeedForwardTest3() {
            float[] inputs = { 1, 0.25f, 0.3f, -0.23f };
            Assert.IsTrue(FFTestBase(inputs, 0, 10, 100));
        }
    }
}