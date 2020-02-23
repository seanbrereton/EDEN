using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace EDEN {
    
    public class Camera : Component {
        //center camera
        Vector2 target = new Vector2(800, 450);

        public Matrix Transform { get; private set; }

        public override void Update(GameTime gameTime) {
            var position = Matrix.CreateTranslation(
                -target.X,
                -target.Y,
                0);

            var offset = Matrix.CreateTranslation(
                Application.screenSize.X / 2,
                Application.screenSize.Y / 2,
                0);

            Transform = position * offset;
        }

        public override void HandleInput() {
            //sets up keyboard input for camera control
            
            int speedMultiplier = 1;
            if (Input.Press(Keys.LeftShift, true) || Input.Press(Keys.RightShift, true))
                speedMultiplier = 2;

            if (Input.Press(Keys.W, true) || Input.Press(Keys.Up, true))
                target.Y -= 8 * speedMultiplier;    
            if (Input.Press(Keys.S, true) || Input.Press(Keys.Down, true))
                target.Y += 8 * speedMultiplier;    
            if (Input.Press(Keys.A, true) || Input.Press(Keys.Left, true))
                target.X -= 8 * speedMultiplier;    
            if (Input.Press(Keys.D, true) || Input.Press(Keys.Right, true))
                target.X += 8 * speedMultiplier;    
        }
    }
}
