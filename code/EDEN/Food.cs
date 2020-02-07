using Microsoft.Xna.Framework;
using System;
namespace EDEN {
    public class Food : Entity {

        public Food() {
            // Random starting position
            position = Rand.Range(new Vector2(
                Application.graphics.PreferredBackBufferWidth,
                Application.graphics.PreferredBackBufferHeight)
            );

            Color color = Color.Beige;
            int size = 5;

            // Generates square texture
            texture = Textures.Rect(color, size, size);
        }

    }
}
