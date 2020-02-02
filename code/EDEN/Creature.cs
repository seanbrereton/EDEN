using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDEN {
    public class Creature : Entity {
        public Creature() {
            // Constructor to generate a new random creature
            // TODO: Change values to random values, from Random class

            //Color color = Color.Red;
            Color color = Rand.RandColor();
            int radius = 15;
            texture = Textures.Circle(color, radius);

            position = new Vector2(
                Application.graphics.PreferredBackBufferWidth / 2,
                Application.graphics.PreferredBackBufferHeight / 2
            );
        }

        public Creature Reproduce(Creature mate) {
            return new Creature();
        }
    }
}
