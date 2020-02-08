using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace EDEN {
    public class Application : Game {
        public static MouseState mouse;
        public static GraphicsDeviceManager graphics;

        SpriteBatch spriteBatch;

        List<Creature> creatures;
        List<Food> foods;
        List<Entity> entities;

        // Population settings
        int initialPopulation = 256;

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
            layers = new int[] { 4, 2 };

            spriteBatch = new SpriteBatch(GraphicsDevice);
            Textures.Init(this);

            ConfigureScreen();

            entities = new List<Entity>();
            InitializeFood();
            InitializePopulation();

            base.Initialize();
        }

        void ConfigureScreen() {
            if (fullscreen) {
                graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            } else {
                graphics.PreferredBackBufferWidth = screenSize.X;
                graphics.PreferredBackBufferHeight = screenSize.Y;
            }

            graphics.IsFullScreen = fullscreen;

            graphics.ApplyChanges();
        }

        void InitializePopulation() {
            creatures = new List<Creature>();

            for (int i = 0; i < initialPopulation; i++)
                creatures.Add(new Creature());

            entities.AddRange(creatures);
        }

        void InitializeFood() {
            foods = new List<Food>();

            for (int i = 0; i < initialPopulation / 4; i++)
                foods.Add(new Food());

            entities.AddRange(foods);
        }

        protected override void Update(GameTime gameTime) {
            base.Update(gameTime);

            mouse = Mouse.GetState();

            if (mouse.RightButton == ButtonState.Pressed)
                foreach (Creature creature in creatures)
                    creature.network = new NeuralNet(layers);
            else if (mouse.LeftButton == ButtonState.Pressed)
                foreach (Creature creature in creatures)
                    creature.growing = true;

            List<Food> toBeEaten = new List<Food>();

            foreach (Creature creature in creatures) {
                creature.Update();
                foreach (Food food in foods) {
                    if (creature.rect.Contains(food.position)) {
                        toBeEaten.Add(food);
                    }
                }
            }

            foreach (Food food in toBeEaten) {
                food.position = Rand.Range(screenSize.ToVector2());
            }
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
