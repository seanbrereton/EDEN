using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace EDEN {

    public class Camera : Component {

        public Vector2 screenSize;
        public bool locked = true;

        float zoomLevel = 1;
        public Matrix transform;

        public Camera(Vector2 _screenSize) {
            screenSize = _screenSize;
            position = screenSize / 2;
        }


        public override void Update(float deltaTime) {
            transform = Matrix.CreateTranslation(-position.X, -position.Y, 0)
                * Matrix.CreateScale(new Vector3(zoomLevel, zoomLevel, 1))
                * Matrix.CreateTranslation(new Vector3(screenSize.X / 2, screenSize.Y / 2, 0));
        }

        public override void HandleInput() {
            if (locked)
                return;

            //sets up keyboard input for camera control
            
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
