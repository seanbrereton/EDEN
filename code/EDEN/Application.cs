﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace EDEN {
    
    public class Application : Game {
        public static GraphicsDeviceManager graphics;

        SpriteBatch spriteBatch;
        SpriteBatch UIspriteBatch;
        public static SpriteFont font;

        bool fullscreen = false;
        public Vector2 screenSize = new Vector2(1600, 900);

        public static State activeState;
        

        public Application() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void LoadContent() {
            font = Content.Load<SpriteFont>("Fonts/Font");
        }

        protected override void Initialize() {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            UIspriteBatch = new SpriteBatch(GraphicsDevice);
            Textures.Init(this);
            ConfigureScreen();


            activeState = new MainMenu(this);
            
            activeState.SuperStart();

            base.Initialize();
        }

        public void SwitchState(State state) {
            activeState.active = false;
            activeState = state;
            activeState.SuperStart();
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
            activeState.SuperUpdate((float)gameTime.ElapsedGameTime.TotalSeconds);

            if (Input.Press(Keys.F)) {
                fullscreen = !fullscreen;
                ConfigureScreen();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            activeState.SuperDraw(spriteBatch, UIspriteBatch);

            base.Draw(gameTime);
        }
    }
}