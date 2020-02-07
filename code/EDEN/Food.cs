using Microsoft.Xna.Framework;
using System;
namespace EDEN {
    public class Food : Entity {
        public Food() {
            //Initialise food values
            Color color = Color.Beige;
            int height = 5;
            int width = 5;
            //Draws food
            texture = Textures.Rect(color, height, width);

            //Chooses random position to spawn
            position = Rand.Range(new Vector2(
                Application.graphics.PreferredBackBufferWidth,
                Application.graphics.PreferredBackBufferHeight)
            );
        }
    }
}
