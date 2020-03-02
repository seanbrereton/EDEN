using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace EDEN {
    public class Entity : Component {

        public Texture2D texture;
        public Color color = Color.White;
        Color highlightColor;
        Texture2D highlightTexture;

        public float rotation;
        public float scale = 1;

        public Rectangle rect;

        public bool dynamic;

        public Entity(Vector2 _position) {
            position = _position;
        }

        public void WrapInBounds(Point bounds) {
            if (position.X > bounds.X)
                position.X = 0;
            if (position.X < 0)
                position.X = bounds.X;
            if (position.Y > bounds.Y)
                position.Y = 0;
            if (position.Y < 0)
                position.Y = bounds.Y;
        }

        // A position directly in front of the entity, based on current rotation and position
        public Vector2 Forward {
            get {
                double rads = Math.PI * rotation / 180f;
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

        public void Highlight(Color color) {
            if (highlightTexture is null)
                highlightTexture = Textures.Circle(
                    Color.Transparent,
                    Math.Max(texture.Width, texture.Height),
                    4, Color.White
                );
            highlightColor = color;
        }

        public override void SuperDraw(SpriteBatch spriteBatch, SpriteBatch UIspriteBatch) {
            rect = GetRect();
            (UI ? UIspriteBatch : spriteBatch).Draw(texture, rect, color);
            if (highlightTexture != null && highlightColor != Color.Transparent) {
                Rectangle highlightRect = new Rectangle(rect.Location, rect.Size);
                highlightRect.Inflate(rect.Width / 2, rect.Height / 2);
                (UI ? UIspriteBatch : spriteBatch).Draw(highlightTexture, highlightRect, highlightColor);
                highlightColor = Color.Transparent;
            }

            base.SuperDraw(spriteBatch, UIspriteBatch);
        }

        virtual public void Collides(Entity other) {}
    }
}
