using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace EDEN {
    public class Entity : Component {

        public Texture2D texture;
        public Color color = Color.White;

        public Vector2 position;
        public float rotation;
        public float scale = 1;

        public Rectangle rect;

        public bool dynamic;

        public Entity(Vector2 _position) {
            position = _position;
        }

        // A position directly in front of the entity, based on current rotation and position
        public Vector2 Forward {
            get {
                double rads = MathHelper.DegreeToRadian(rotation);
                float x = (float)Math.Sin(rads);
                float y = (float)Math.Cos(rads);
                return new Vector2(x, y);
            }
        }

        // A position directly to the right of the entity
        public Vector2 Right {
            get {
                Vector2 forward = Forward;
                return new Vector2(forward.Y, -forward.X);
            }
        }

        virtual public Rectangle GetRect() {
            Point pos = position.ToPoint();
            int width = (int)Math.Round(texture.Width * scale);
            int height = (int)Math.Round(texture.Height * scale);
            // Gets a rectangle, the center of which is at the current position
            return new Rectangle(pos - new Point(width / 2, height / 2), new Point(width, height));
        }

        public override void SuperDraw(SpriteBatch spriteBatch, SpriteBatch UIspriteBatch) {
            rect = GetRect();
            (UI ? UIspriteBatch : spriteBatch).Draw(texture, rect, color);

            base.SuperDraw(spriteBatch, UIspriteBatch);
        }

        virtual public void Collides(Entity other) {}
    }
}
