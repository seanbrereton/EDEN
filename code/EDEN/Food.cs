using Microsoft.Xna.Framework;
using System;
namespace EDEN {
    public class Food : Entity {
        public Food(Vector2 _position) : base(_position) {
            //food attributes
            color = Color.Beige;
            int height = 5;
            int width = 5;
            texture = Textures.Rect(Color.White, height, width);
        }

    }
}
