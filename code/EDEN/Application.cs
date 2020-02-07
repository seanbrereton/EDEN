using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace EDEN {
    public class Application : Game {
        public static MouseState mouse;
        public static GraphicsDeviceManager graphics;

        SpriteBatch spriteBatch;

        List<Creature> creatures;
        List<Entity> entities;

        // Population settings
        int initialPopulation = 128;

        // Display settings
        bool fullscreen = false;
        Color bgColor = Color.DarkSlateBlue;
        Point screenSize = new Point(1600, 900);

        public static int[] layers;

        public Application() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            IsMouseVisible = true;
        }

        protected override void Initialize() {
            layers = new int[] { 5, 8, 8, 3 };

            if (fullscreen) {
                graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            } else {
                graphics.PreferredBackBufferWidth = screenSize.X;
                graphics.PreferredBackBufferHeight = screenSize.Y;
            }
            
            graphics.IsFullScreen = fullscreen;

            graphics.ApplyChanges();

            Textures.Init(this);
            creatures = new List<Creature>();
            entities = new List<Entity>();

            for (int i = 0; i < initialPopulation; i++)
                creatures.Add(new Creature());

            entities.AddRange(creatures);

            base.Initialize();
        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime) {
            base.Update(gameTime);

            mouse = Mouse.GetState();

            if (mouse.RightButton == ButtonState.Pressed)
                foreach (Creature creature in creatures)
                    creature.network = new NeuralNet(layers);

            foreach (Entity entity in entities)
                entity.Update();
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(bgColor);
            spriteBatch.Begin();

            foreach (Entity entity in entities) {
                entity.Draw(spriteBatch);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
