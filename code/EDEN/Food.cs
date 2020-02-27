using Microsoft.Xna.Framework;
using System;
namespace EDEN {
    public class Food : Entity {

        float decayTime = 32;

        public Food(Vector2 _position, Color _color) : base(_position) {
            //food attributes
            color = Color.Lerp(Color.White, _color, 0.5f);
            int height = 5;
            int width = 5;
            texture = Textures.Rect(Color.White, height, width, 1);
        }
        public Food(Vector2 _position) : this(_position, Color.Beige) { }

        public override void Update(float deltaTime) {
            decayTime -= deltaTime;
            if (decayTime <= 0) {
                ((Simulation)parent).foods.Remove(this);
                Remove();
            }
        }

    }
}
