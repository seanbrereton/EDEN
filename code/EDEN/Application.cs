using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace EDEN {
    public class Application : Game {
        public static GraphicsDeviceManager graphics;

        SpriteBatch spriteBatch;

        List<Component> components = new List<Component>();

        // Display settings
        bool fullscreen = false;
        Color bgColor = Color.DarkOliveGreen;
        public static Vector2 screenSize = new Vector2(1600, 900);

        public static int[] layers = new int[] { 4, 2 };

        public Application() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize() {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Textures.Init(this);

            ConfigureScreen();

            components.Add(new Simulation());

            foreach (Component component in components)
                component.SuperStart();

            base.Initialize();
        }

        void ConfigureScreen() {
            if (fullscreen) {
                graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            } else {
                graphics.PreferredBackBufferWidth = (int)screenSize.X;
                graphics.PreferredBackBufferHeight = (int)screenSize.Y;
            }

            graphics.IsFullScreen = fullscreen;
            graphics.ApplyChanges();
        }

        protected override void Update(GameTime gameTime) {
            Input.Update();

            foreach (Component component in components)
                component.SuperUpdate();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(bgColor);
            spriteBatch.Begin();

            foreach (Component component in components)
                component.SuperDraw(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
