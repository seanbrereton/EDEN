using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EDEN {
    public class Entity {
        public Texture2D texture;

        public Vector2 position;
        //public Vector2 scale;
        //public Vector2 velocity;
        //public float rotation;

        public Entity() {}

        public Entity(Texture2D _texture, Vector2 _position) {
            texture = _texture;
            position = _position;
        }

        public Entity(Color color, int radius, Vector2 _position) {
            texture = Textures.Circle(color, radius);
            position = _position;
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, position, Color.White);
        }

        public void GoForward(float distance) {
            // TODO: move the entitiy forward in the direction it is facing
        }
    }
}
