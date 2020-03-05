using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace EDEN {
    
    class CreatureDisplay : UI {

        public Creature creature;

        public CreatureDisplay(Vector2 _position, int width, int height) : base(_position) {
            texture = Textures.Rect(Color.White, width, height);
        }

        public override void HandleInput() {
            // Tartgets creature in simulation if they are clicked on list
            if (Input.Click() && rect.Contains(Input.MousePos))
                creature.Target();
        }

        public override void Draw(SpriteBatch spriteBatch) {
            if (creature != null) {
                color = creature.color;

                // Chooses between black and white font based on the relative luminance of the colour
                fontColor = Math.Sqrt(
                   color.R * color.R * 0.21 +
                   color.G * color.G * 0.72 +
                   color.B * color.B * 0.07
                ) > 127 ? Color.Black : Color.White;

                // Displays creatures name, age, child count and generation
                text = creature.name + ": " + 
                    Math.Round((double)creature.age, 1).ToString() + ", " + 
                    Math.Round((double)creature.childrenCount, 1).ToString() + ", " + 
                    Math.Round((double)creature.generation, 1).ToString();
            }
        }

    }
}
