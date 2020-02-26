using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace EDEN {
    class State : Component {

        Application app;
        Camera camera = new Camera();
        Color bgColor;
        QuadTree quadTree;

        public State(Application _app) {
            app = _app;
        }

        public override void SuperUpdate(GameTime gameTime) {
            Input.Update();

            quadTree.Create(Components);
            quadTree.CheckCollisions();

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
