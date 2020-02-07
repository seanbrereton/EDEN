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

        // Network Inputs
        float age = 0;

        // Network Outputs
        float rotationVelocity;
        float movementVelocity;

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
            int radius = 25;
            texture = Textures.Circle(color, radius);
        }

        public override void Update() {
            // Gets inputs, puts them through neural net, sets outputs
            Think();

            // Uses the network outputs to change position and rotation
            position += Forward * movementVelocity * 10;
            rotation += rotationVelocity;
            
            // Stays on the screen
            KeepOnScreen()
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
    }
}
