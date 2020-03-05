using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EDEN {
    public static class Textures {
        static GraphicsDevice graphics;
        
        public static void Init(Application app) {
            // Gets the graphics device from the application,
            // needed to creature textures that can be drawn on screen
            graphics = app.GraphicsDevice;
        }

        // Returns an tempty texture with width and height
        public static Texture2D Empty(int width, int height) {
            return new Texture2D(graphics, width, height);
        }

        // Draws circular texture, with a given outline colour
        public static Texture2D Circle(Color color, int radius, int outlineWidth, Color outlineColor) {
            int diameter = radius * 2;
            Texture2D texture = new Texture2D(graphics, diameter, diameter);
            Color[] colors = new Color[diameter * diameter];

            for (int x = 0; x < diameter; x++) {
                for (int y = 0; y < diameter; y++) {
                    int i = x * diameter + y;
                    Vector2 pos = new Vector2(x - radius, y - radius);
                    if (pos.Length() >= radius)
                        colors[i] = Color.Transparent;
                    else if (pos.Length() >= radius - outlineWidth)
                        colors[i] = outlineColor;
                    else
                        colors[i] = color;
                }
            }

            texture.SetData<Color>(colors);
            return texture;
        }

        // Draws circle texture with default outline, which is the main colour but darkened
        public static Texture2D Circle(Color color, int radius, int outlineWidth = 0) {
            Color outlineColor = outlineWidth > 0 ? Color.Lerp(color, Color.Black, 0.2f) : Color.Black;
            return Circle(color, radius, outlineWidth, outlineColor);
        }

        // Draws rectangle texture, with a given outline colour
        public static Texture2D Rect(Color color, int width, int height, int outlineWidth, Color outlineColor) {
            Texture2D texture = new Texture2D(graphics, width, height);
            Color[] colors = new Color[width * height];

            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    int i = y * width + x;
                    if (x < outlineWidth || width - x <= outlineWidth || y < outlineWidth || height - y <= outlineWidth)
                        colors[i] = outlineColor;
                    else
                        colors[i] = color;
                }
            }

            texture.SetData<Color>(colors);
            return texture;
        }


        // Draws rectangle texture with default outline, which is the main colour but darkened
        public static Texture2D Rect(Color color, int width, int height, int outlineWidth = 0) {
            Color outlineColor = outlineWidth > 0 ? Color.Lerp(color, Color.Black, 0.2f) : Color.Black;
            return Rect(color, width, height, outlineWidth, outlineColor);
        }
    }
}
