using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EDEN {
    public class State : Component {

        public Application app;
        Camera camera = new Camera();
        public Color bgColor;
        public QuadTree quadTree;

        public State(Application _app) {
            app = _app;
        }

        public override void SuperStart() {
            components.Add(camera);

            base.SuperStart();
        }

        public override void SuperUpdate(GameTime gameTime) {
            Input.Update();

            quadTree?.Update(Components);
            quadTree?.CheckCollisions();

            base.SuperUpdate(gameTime);
        }

        public override void SuperDraw(SpriteBatch spriteBatch, SpriteBatch UIspriteBatch) {
            app.GraphicsDevice.Clear(bgColor);
            spriteBatch.Begin(transformMatrix: camera.Transform);
            UIspriteBatch.Begin();

            base.SuperDraw(spriteBatch, UIspriteBatch);

            UIspriteBatch.End();
            spriteBatch.End();
        }

    }
}
