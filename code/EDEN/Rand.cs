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

        public static int Range(int max) { return rand.Next(max); }
        public static int Range(int min, int max) { return rand.Next(min, max); }
    }
}