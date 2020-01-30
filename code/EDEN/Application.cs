using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace EDEN {
    public class Application : Game {
        public static GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        List<Entity> entities;

        public Application() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize() {
            graphics.PreferredBackBufferHeight = (int)Math.Round(1080 * 0.9f);
            graphics.PreferredBackBufferWidth = (int)Math.Round(1920 * 0.9f);
            graphics.ApplyChanges();

            Textures.Init(this);
            entities = new List<Entity>();

            entities.Add(new Creature());

            base.Initialize();
        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime) {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Goldenrod);

            spriteBatch.Begin();

            foreach (Entity entity in entities) {
                entity.Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
