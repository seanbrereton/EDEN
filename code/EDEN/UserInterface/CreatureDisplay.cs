using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDEN {
    
    class CreatureDisplay : UI {

        public Creature creature;

        public CreatureDisplay(Vector2 _position, int width, int height) : base(_position) {
            texture = Textures.Rect(Color.White, width, height, 4);
        }

        public override void HandleInput() {
            if (Input.Click() && rect.Contains(Input.MousePos))
                creature.Target();
        }

        public override void Draw(SpriteBatch spriteBatch) {
            if (creature == null)
                return;

            color = creature.color;
            fontColor = Math.Sqrt(
               color.R * color.R * .241 +
               color.G * color.G * .691 +
               color.B * color.B * .068
            ) > 127 ? Color.Black : Color.White;

            text = creature.name + ": " + 
                Math.Round((double)creature.age, 1).ToString() + ", " + 
                Math.Round((double)creature.childrenCount, 1).ToString() + ", " + 
                Math.Round((double)creature.generation, 1).ToString();
        }

    }
}
