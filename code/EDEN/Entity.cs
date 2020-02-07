using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace EDEN {
    public class Entity {
        public Texture2D texture;

        public Vector2 position;
        public float rotation;
        //public Vector2 scale;
        //public Vector2 velocity;

        public Entity() {}

        public Entity(Texture2D _texture, Vector2 _position) {
            texture = _texture;
            position = _position;
        }

        public Entity(Color color, int radius, Vector2 _position) {
            texture = Textures.Circle(color, radius);
            position = _position;
        }

        double DegreeToRadian(float angle) {
            return Math.PI * angle / 180f;
        }

        public Vector2 Forward {
            get {
                double rads = DegreeToRadian(rotation);
                float x = (float)Math.Sin(rads);
                float y = (float)Math.Cos(rads);
                return new Vector2(x, y);
            }
        }

        public Vector2 Sideways {
            get {
                Vector2 forward = Forward;
                return new Vector2(forward.Y, -forward.X);
            }
        }

        public Rectangle Rect {
            get {
                Point pos = position.ToPoint();
                int width = texture.Width;
                int height = texture.Height;
                return new Rectangle(pos - new Point(width / 2, height / 2), new Point(texture.Width, texture.Height));
            }
        }

        public virtual void Update() { }

        //public void Update() {
        //    Update();
        //}
    
        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, Rect, Color.White);
        }

        public void GoForward(float distance) {
            // TODO: move the entitiy forward in the direction it is facing
        }
    }
}
