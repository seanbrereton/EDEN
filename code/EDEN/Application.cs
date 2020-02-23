using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace EDEN {
    public class Application : Game {
        public static GraphicsDeviceManager graphics;

        SpriteBatch spriteBatch;

        List<Component> childComponents = new List<Component>();
        List<Component> components = new List<Component>();

        public static Texture2D[] branchTextures = new Texture2D[9];

        // Display settings
        bool fullscreen = false;
        Color bgColor = Color.DarkOliveGreen;
        public static Vector2 screenSize = new Vector2(1600, 900);

        public static int[] layers = new int[] { 5, 2 };

        QuadTree quadTree;
        Component activeScene;

        public Application() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize() {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Textures.Init(this);

            for (int i = 0; i < branchTextures.Length; i++)
                branchTextures[i] = Textures.Rect(Color.Transparent, (int)screenSize.X, (int)screenSize.Y, (int)(Math.Pow(2, i)), Color.Goldenrod);

            ConfigureScreen();

            quadTree = new QuadTree(new Rectangle(Point.Zero, new Point((int)Application.screenSize.X)));
            activeScene = new Simulation();
            components.Add(activeScene);
            components.Add(quadTree);

            foreach (Component component in components)
                component.SuperStart();

            base.Initialize();
        }

        void CreateQuadTree() {
            // Constructs quad tree, and gets all components from the current scene
            quadTree.Clear();
            childComponents = activeScene.Components;
            foreach (Component component in childComponents) {
                if (component is Entity)
                    quadTree.Insert((Entity)component);
            }
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
                component.SuperUpdate(gameTime);

            // Uses the quad tree to find all collisions between entities
            CreateQuadTree();
            foreach (Component component in childComponents) {
                if (component is Entity) {
                    Entity entity = (Entity)component;
                    if (entity.dynamic) {
                        List<Entity> entities = quadTree.Query(entity.rect);
                        foreach (Entity other in entities) {
                            if (entity.rect.Intersects(other.rect) && !entity.Equals(other))
                                entity.Collides(other);
                        }
                    }
                }
            }

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
