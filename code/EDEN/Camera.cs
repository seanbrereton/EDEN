using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace EDEN {

    public class Camera : Component {

        public Vector2 screenSize;
        public bool locked = true;
        public Matrix transform;
        float zoomLevel = 1;

        public Camera(Vector2 _screenSize) {
            // Gets screen size and set position to centre
            screenSize = _screenSize;
            position = screenSize / 2;
        }

        public override void Update(float deltaTime) {
            // Updates the camera's transform matrix, with correct position and zoom

            Matrix positionMatrix = Matrix.CreateTranslation(-position.X, -position.Y, 0);
            Matrix zoomMatrix = Matrix.CreateScale(new Vector3(zoomLevel, zoomLevel, 1));
            Matrix offsetMatrix = Matrix.CreateTranslation(new Vector3(screenSize.X / 2, screenSize.Y / 2, 0));

            transform = positionMatrix * zoomMatrix * offsetMatrix;
        }

        public override void HandleInput() {
            if (!locked) {
                // Sets up keyboard input for camera control

                // Allows the holding of shift to increase the speed of moving and zooming
                int speedMultiplier = 1;
                if (Input.Press(Keys.LeftShift, true) || Input.Press(Keys.RightShift, true))
                    speedMultiplier = 2;

                if (Input.Press(Keys.W, true) || Input.Press(Keys.Up, true))
                    position.Y -= 8 * speedMultiplier;
                if (Input.Press(Keys.S, true) || Input.Press(Keys.Down, true))
                    position.Y += 8 * speedMultiplier;
                if (Input.Press(Keys.A, true) || Input.Press(Keys.Left, true))
                    position.X -= 8 * speedMultiplier;
                if (Input.Press(Keys.D, true) || Input.Press(Keys.Right, true))
                    position.X += 8 * speedMultiplier;

                if (Input.ScrollUp())
                    zoomLevel *= 1 + (0.08f * speedMultiplier);
                if (Input.ScrollDown())
                    zoomLevel *= 1 - (0.08f * speedMultiplier);

                if (Input.Press(Keys.OemPlus, true))
                    zoomLevel *= 1 + (0.02f * speedMultiplier);
                if (Input.Press(Keys.OemMinus, true))
                    zoomLevel *= 1 - (0.02f * speedMultiplier);
            }
        }
    }
}
