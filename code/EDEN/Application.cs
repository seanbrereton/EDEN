using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
            // Initializes the manager needed to draw to the window
            graphics = new GraphicsDeviceManager(this);
            // Sets the directory used for loading content
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void LoadContent() {
            // Loads the font used
            font = Content.Load<SpriteFont>("Fonts/Font");
        }

        protected override void Initialize() {
            // Initializes the spritebatches (used to draw to screen)
            spriteBatch = new SpriteBatch(GraphicsDevice);
            UIspriteBatch = new SpriteBatch(GraphicsDevice);
            
            Textures.Init(this);
            ConfigureScreen();

            // Sets the initial active state to the main menu
            activeState = new MainMenu(this);
            // Starts the active state
            activeState.SuperStart();

            base.Initialize();
        }

        public void SwitchState(State state) {
            // Sets current state's active to false, to prevent any further updates from its components
            activeState.active = false;
            // Switches current active state to the new one, and starts it
            activeState = state;
            activeState.SuperStart();
        }

        void ConfigureScreen() {
            // Sets the window size to appropriate values, based on settings

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
            // GameTime is a type used to show information about how much time has passed since the last Update
            // Call the active state's SuperUpdate method, passing in the time in seconds since the last call
            activeState.SuperUpdate((float)gameTime.ElapsedGameTime.TotalSeconds);

            if (Input.Press(Keys.F)) {
                // Toggles fullscreen, and updates window to apply it
                fullscreen = !fullscreen;
                ConfigureScreen();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            // Call the active state's SuperDraw method, with the two spriteBatches
            activeState.SuperDraw(spriteBatch, UIspriteBatch);

            base.Draw(gameTime);
        }
    }
}