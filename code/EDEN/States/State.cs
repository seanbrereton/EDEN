using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EDEN {
    
    public class State : Component {
        
        public Application app;
        public QuadTree quadTree;
        public Camera camera;
        public Color bgColor;
        public bool debug;
        public float runSpeed = 1;

        public State(Application _app) {
            app = _app;
        }

        public override void SuperStart() {
            // Initialises camera, adds it to the components list
            camera = new Camera(app.screenSize);
            AddComponent(camera);

            // Initializes Input with the new camera
            Input.Initialize(camera);

            base.SuperStart();
        }

        public override void SuperUpdate(float deltaTime) {
            // Updates the static Input class
            Input.Update();

            // Recursively get all components contained in this state
            List<Component> childComponents = GetChildComponents();
            // If the state has a quad tree, update it with the components, and check for collisions
            List<Entity> childEntities = quadTree?.UpdateEntities(childComponents);
            quadTree?.CheckCollisions(childEntities);

            // Call Component's SuperUpdate method, using deltaTime (seconds since last Update) multiplied by the runSpeed
            // This allows us to change the speed that the application runs
            base.SuperUpdate(deltaTime * runSpeed);
        }

        public override void SuperDraw(SpriteBatch spriteBatch, SpriteBatch UIspriteBatch) {
            // Fills the screen with the background color
            app.GraphicsDevice.Clear(bgColor);

            // Begins both spritebatches, using PointClamp to get clearer graphics, without antialiasing            
            UIspriteBatch.Begin(samplerState: SamplerState.PointClamp);
            // The regular spritebatch Begins with the camera transform, allowing us to pan and zoom around a state,
            // while the UI stays in a fixed position on the screen.
            spriteBatch.Begin(transformMatrix: camera.transform, samplerState: SamplerState.PointClamp);

            base.SuperDraw(spriteBatch, UIspriteBatch);

            // Visualises the quad tree if debug mode is on
            if (debug)
                quadTree?.Draw(spriteBatch);

            // Finishes altering the spriteBatches, drawing everything to the screen
            spriteBatch.End();
            UIspriteBatch.End();
        }

    }
}
