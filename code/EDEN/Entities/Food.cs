using Microsoft.Xna.Framework;

namespace EDEN {
    
    public class Food : Entity {

        public float energy;
        float decayTime = 32;

        public Food(Vector2 _position, Color _color, float _energy) : base(_position) {
            energy = _energy;
            // Colour is set to a lighter version of the colour passed in
            color = Color.Lerp(Color.White, _color, 0.5f);
            texture = Textures.Rect(Color.White, 5, 5, 1);
        }

        public override void Update(float deltaTime) {
            // Removes food if it has not been eaten in time
            decayTime -= deltaTime;
            if (decayTime <= 0) {
                ((Simulation)parent).foods.Remove(this);
                Remove();
            }
        }

    }
}
