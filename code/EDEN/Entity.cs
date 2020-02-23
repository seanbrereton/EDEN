using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace EDEN {
    public class Entity : Component {
        public Texture2D texture;

        public Vector2 position;
        public float rotation;

        public Rectangle rect;

        public bool dynamic;

        public Entity() { }

        // A position directly in front of the entity, based on current rotation and position
        public Vector2 Forward {
            get {
                double rads = MathHelper.DegreeToRadian(rotation);
                float x = (float)Math.Sin(rads);
                float y = (float)Math.Cos(rads);
                return new Vector2(x, y);
            }
        }

        // A position directly to the side of the entity
        public Vector2 Sideways {
            get {
                Vector2 forward = Forward;
                return new Vector2(forward.Y, -forward.X);
            }
        }

        virtual public Rectangle GetRect() {
            Point pos = position.ToPoint();
            int width = texture.Width;
            int height = texture.Height;
            // Gets a rectangle, the center of which is at the current position
            return new Rectangle(pos - new Point(width / 2, height / 2), new Point(width, height));
        }

        public override void SuperDraw(SpriteBatch spriteBatch) {
            rect = GetRect();
            spriteBatch.Draw(texture, rect, Color.White);

            base.SuperDraw(spriteBatch);
        }

        virtual public void Collides(Entity other) {}
    }
}
