using Microsoft.Xna.Framework;
using System;

namespace EDEN {
	public static class Rand {

		static Random rand = new Random();

		public static Color RandColor() {
			Color randColor = new Color(rand.Next(0, 255),
				rand.Next(0, 255), rand.Next(0, 255));
			return randColor;
        }

        public static int Range(int min, int max) { return rand.Next(min, max); }
        public static int Range(int max) { return rand.Next(max); }

        public static float Range(float min, float max) {
            return (float)rand.NextDouble() * (max - min) + min;
        }
        public static float Range(float max) { return Range(0, max); }

        public static Vector2 Range(Vector2 min, Vector2 max) {
            return new Vector2(Range(min.X, max.X), Range(min.Y, max.Y));
        }
        public static Vector2 Range(Vector2 max) { return Range(Vector2.Zero, max); }

    }
}