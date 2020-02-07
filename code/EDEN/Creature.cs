using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDEN {
    public class Creature : Entity {

        public NeuralNet network;

        float age = 0;

        bool grabbed = false;

        // Network Outputs
        float rotSpeed;
        float movSpeed;
        float strafeSpeed;

        public Creature() {
            network = new NeuralNet(Application.layers);

            Color color = Rand.RandColor();
            int radius = 25;

            rotation = Rand.Range(360);

            position = Rand.Range(new Vector2(
                Application.graphics.PreferredBackBufferWidth,
                Application.graphics.PreferredBackBufferHeight)
            );

            texture = Textures.Circle(color, radius);
        }

        public override void Update() {
            Think();

            position += Forward * movSpeed * 10;
            position += Sideways * strafeSpeed * 10;
            rotation += rotSpeed;

            Rectangle screen = new Rectangle(0, 0, Application.graphics.PreferredBackBufferWidth, Application.graphics.PreferredBackBufferHeight);

            if (position.X > screen.Width)
                position.X = 0;
            if (position.X < 0)
                position.X = screen.Width;
            if (position.Y > screen.Height)
                position.Y = 0;
            if (position.Y < 0)
                position.Y = screen.Height;

            if (Application.mouse.LeftButton == ButtonState.Pressed && Rect.Contains(Application.mouse.Position))
                grabbed = true;
            if (Application.mouse.LeftButton == ButtonState.Released)
                grabbed = false;
            if (grabbed)
                position = Application.mouse.Position.ToVector2();

            age += 0.05f;
        }

        void Think() {
            Vector2 toMouse = position - Application.mouse.Position.ToVector2();
            float[] inputs = new float[] { 1, movSpeed, rotSpeed, strafeSpeed, ((rotation % 360) / 180f) - 1};

            float[] outputs = network.FeedForward(inputs);

            movSpeed = outputs[0];
            strafeSpeed = outputs[1];
            rotSpeed = outputs[2];
        }

        public Creature Reproduce(Creature mate) {
            return new Creature();
        }
    }
}
