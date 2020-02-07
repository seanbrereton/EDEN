using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDEN {
    public class Creature : Entity {

        public NeuralNet network;

        public float scale;

        // Network Outputs
        float rotationVelocity;
        float movementVelocity;

        Texture2D eyeTexture;
        Rectangle leftEyeRect;
        Rectangle rightEyeRect;

        public bool growing;

        public Creature() {
            // Creates a random neural network, using the applications layer parameters
            network = new NeuralNet(Application.layers);

            // Random rotation in degrees
            rotation = Rand.Range(360);

            // Starting position is a random space on the screen
            position = Rand.Range(new Vector2(
                Application.graphics.PreferredBackBufferWidth,
                Application.graphics.PreferredBackBufferHeight)
            );

            // Creates a circle texture using the colour and radius generated
            Color color = Rand.RandColor();
            int radius = 15;
            texture = Textures.Circle(color, radius, 4);
            eyeTexture = Textures.Circle(Color.Black, radius / 3, radius / 5, Color.White);

            scale = 0.2f;
        }

        public override void Update() {
            // Gets inputs, puts them through neural net, sets outputs
            Think();

            // Uses the network outputs to change position and rotation
            position += Forward * movementVelocity * 10;
            rotation += rotationVelocity;

            // Adds to scale until fully grown
            if (scale < 1 && growing)
                scale += 0.005f;

            // Stays on the screen
            KeepOnScreen();
        }

        public override void Draw(SpriteBatch spriteBatch) {
            base.Draw(spriteBatch);

            Point leftEyePos = (position + (Forward * (rect.Width * 0.35f)) + (Sideways * (rect.Width * 0.3f))).ToPoint();
            Point rightEyePos = (position + (Forward * (rect.Width * 0.35f)) - (Sideways * (rect.Width * 0.3f))).ToPoint();
            leftEyeRect = new Rectangle(leftEyePos.X - rect.Width / 6, leftEyePos.Y - rect.Height / 6, rect.Width / 3, rect.Height / 3);
            rightEyeRect = new Rectangle(rightEyePos.X - rect.Width / 6, rightEyePos.Y - rect.Height / 6, rect.Width / 3, rect.Height / 3);

            spriteBatch.Draw(eyeTexture, leftEyeRect, Color.White);
            spriteBatch.Draw(eyeTexture, rightEyeRect, Color.White);
        }

        void KeepOnScreen() {
            // Checks if outside any bounds of the screen
            // Changes position to keep it in the bounds
            Rectangle screen = new Rectangle(0, 0, Application.graphics.PreferredBackBufferWidth, Application.graphics.PreferredBackBufferHeight);
            if (position.X > screen.Width)
                position.X = 0;
            if (position.X < 0)
                position.X = screen.Width;
            if (position.Y > screen.Height)
                position.Y = 0;
            if (position.Y < 0)
                position.Y = screen.Height;
        }

        void Think() {
            // Gets inputs from self and surroundings
            float[] inputs = GetInputs();

            // Gets the outputs from the network
            float[] outputs = network.FeedForward(inputs);

            // Sets the variables to the outputs from the network
            movementVelocity = outputs[0];
            rotationVelocity = outputs[1];
        }

        float[] GetInputs() {
            return new float[] { 
                1,
                movementVelocity,
                rotationVelocity,
                // Normalizes the rotation to be in the range [-1, 1]
                ((rotation % 360) / 180f) - 1
            };
        }

        public override Rectangle GetRect() {
            int size = (int)(texture.Width * scale);
            Point newPos = new Point((int)position.X - size / 2, (int)position.Y - size / 2);
            Point sizePoint = new Point(size, size);
            return new Rectangle(newPos, sizePoint);
        }
    }
}
