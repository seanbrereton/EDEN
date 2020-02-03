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

        Color bgColor = Color.DarkSlateBlue;
        Point screenSize = new Point(1600, 900);

        public Application() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize() {
            graphics.PreferredBackBufferWidth = screenSize.X;
            graphics.PreferredBackBufferHeight = screenSize.Y;

            //graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            //graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            //graphics.IsFullScreen = true;

            graphics.ApplyChanges();

            Textures.Init(this);
            entities = new List<Entity>();

            for (int i = 0; i < 1024; i++)
                entities.Add(new Creature());

            base.Initialize();
        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime) {
            base.Update(gameTime);

            foreach (Entity entity in entities)
                entity.Update();
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(bgColor);
            spriteBatch.Begin();

            foreach (Entity entity in entities)
                entity.Draw(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
