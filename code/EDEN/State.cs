﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EDEN {
    public class State : Component {

        public Application app;
        public Camera camera;
        public Color bgColor;
        public QuadTree quadTree;
        public bool debug;
        public float gameSpeed = 1;

        public State(Application _app) {
            app = _app;
        }

        public override void SuperStart() {
            camera = new Camera(app.screenSize);
            AddComponent(camera);
            Input.Initialize(camera);

            base.SuperStart();
        }

        public override void SuperUpdate(float deltaTime) {
            Input.Update();

            List<Component> childComponents = GetChildComponents();
            List<Entity> childEntities = quadTree?.UpdateEntities(childComponents);
            quadTree?.CheckCollisions(childEntities);

            base.SuperUpdate(deltaTime * gameSpeed);
        }

        public override void SuperDraw(SpriteBatch spriteBatch, SpriteBatch UIspriteBatch) {
            app.GraphicsDevice.Clear(bgColor);
            
            spriteBatch.Begin(transformMatrix: camera.Transform);
            UIspriteBatch.Begin();

            base.SuperDraw(spriteBatch, UIspriteBatch);
            
            if (debug)
                quadTree?.Draw(spriteBatch);

            UIspriteBatch.End();
            spriteBatch.End();
        }

    }
}