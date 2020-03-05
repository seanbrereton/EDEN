using Microsoft.VisualStudio.TestTools.UnitTesting;
using EDEN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace EDEN.Tests {
    [TestClass()]
    public class RandTests {

        public bool InRange(int min, int max, int value) {
            return value > min && value < max;
        }

        public bool InRange(float min, float max, float value) {
            return value > min && value < max;
        }

        public bool InRange(Vector2 min, Vector2 max, Vector2 value) {
            return value.X > min.X && value.X < max.X
                && value.Y > min.Y && value.Y < max.Y;
        }

        [TestMethod()]
        public void IntRangeTest1() {
            int min = -2;
            int max = 2;
            int value = Rand.Range(min, max);
            Assert.IsTrue(InRange(min, max, value));
        }

        [TestMethod()]
        public void IntRangeTest2() {
            int max = 2500;
            int value = Rand.Range(max);
            Assert.IsTrue(InRange(0, max, value));
        }

        [TestMethod()]
        public void IntRangeTest3() {
            int min = -38;
            int max = -23;
            int value = Rand.Range(min, max);
            Assert.IsTrue(InRange(min, max, value));
        }

        [TestMethod()]
        public void FloatRangeTest1() {
            float min = -2.3f;
            float max = 3.1f;
            float value = Rand.Range(min, max);
            Assert.IsTrue(InRange(min, max, value));
        }

        [TestMethod()]
        public void FloatRangeTest2() {
            float max = 2.0124f;
            float value = Rand.Range(max);
            Assert.IsTrue(InRange(0, max, value));
        }

        [TestMethod()]
        public void FloatRangeTest3() {
            float min = -30.4f;
            float max = -2f;
            float value = Rand.Range(min, max);
            Assert.IsTrue(InRange(min, max, value));
        }

        [TestMethod()]
        public void Vector2RangeTest1() {
            Vector2 min = new Vector2(-17, -23);
            Vector2 max = new Vector2(49, 21);
            Vector2 value = Rand.Range(min, max);
            Assert.IsTrue(InRange(min, max, value));
        }

        [TestMethod()]
        public void Vector2RangeTest2() {
            Vector2 max = new Vector2(490, 121);
            Vector2 value = Rand.Range(max);
            Assert.IsTrue(InRange(Vector2.Zero, max, value));
        }

        [TestMethod()]
        public void Vector2RangeTest3() {
            Vector2 min = new Vector2(-47, -42);
            Vector2 max = new Vector2(-19, -40);
            Vector2 value = Rand.Range(min, max);
            Assert.IsTrue(InRange(min, max, value));
        }
    }
}