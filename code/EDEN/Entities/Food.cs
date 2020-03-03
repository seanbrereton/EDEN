using Microsoft.Xna.Framework;
using System;
namespace EDEN {
    
    public class Food : Entity {

        public float energy;
        float decayTime = 32;

        public Food(Vector2 _position, Color _color, float _energy) : base(_position) {
            //food attributes
            energy = _energy;
            color = Color.Lerp(Color.White, _color, 0.5f);
            int height = 5;
            int width = 5;
            texture = Textures.Rect(Color.White, height, width, 1);
        }
        public Food(Vector2 _position, float _energy) : this(_position, Color.Beige, _energy) { }

        public override void Update(float deltaTime) {
            decayTime -= deltaTime;
            if (decayTime <= 0) {
                ((Simulation)parent).foods.Remove(this);
                Remove();
            }
        }

    }
}
