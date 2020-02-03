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

        public Vector2 Forward {
            get {
                float x = (float)Math.Sin(rotation);
                float y = (float)Math.Cos(rotation);
                return new Vector2(x, y);
            }
        }

        public virtual void Update() { }

        //public void Update() {
        //    Update();
        //}
    
        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, position, Color.White);
        }

        public void GoForward(float distance) {
            // TODO: move the entitiy forward in the direction it is facing
        }
    }
}
