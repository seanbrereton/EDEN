using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDEN {
    public class Creature : Entity {

        float rotSpeed;
        float movSpeed;

        public Creature() {
            Color color = Rand.RandColor();
            int radius = 25;

            rotation = Rand.Range(360);

            rotSpeed = Rand.Range(1, 4);
            movSpeed = Rand.Range(1, 4);

            position = new Vector2(
                Application.graphics.PreferredBackBufferWidth * (color.R / 255f),
                Application.graphics.PreferredBackBufferHeight * (color.G / 255f)
            );

            texture = Textures.Circle(color, 1 + (int)(radius * (color.B / 255f)));
        }

        public override void Update() {
            position += Forward * movSpeed;
            rotation += 0.05f * rotSpeed;
            rotSpeed += movSpeed / 1000;
            movSpeed += rotSpeed / 1000;
        }

        public Creature Reproduce(Creature mate) {
            return new Creature();
        }
    }
}
