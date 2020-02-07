using Microsoft.Xna.Framework;
using System;
namespace EDEN {
    public class Food : Entity {
        public Food() {

            position = Rand.Range(new Vector2(
                Application.graphics.PreferredBackBufferWidth,
                Application.graphics.PreferredBackBufferHeight)
            );

            Color color = Color.Beige;
            int height = 5;
            int width = 5;
            texture = Textures.Rect(color, height, width);
        }
    }
}
