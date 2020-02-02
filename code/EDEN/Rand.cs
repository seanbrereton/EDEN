using Microsoft.Xna.Framework;
using System;

namespace EDEN {
	public static class Rand {

		Random rand = new Random();

		public static Color RandColor() {
			Color randColor = new Color(rand.Next(0, 255),
				rand.Next(0, 255), rand.Next(0, 255));
			return randColor;
        }
	}
}
