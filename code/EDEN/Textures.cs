using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace EDEN {
    public static class Textures {
        static GraphicsDevice graphics;
        
        public static void Init(Game app) {
            graphics = app.GraphicsDevice;
        }

        // Draws circular texture
        public static Texture2D Circle(Color color, int radius, int outlineWidth, Color outlineColor) {
            int diameter = radius * 2;
            Texture2D texture = new Texture2D(graphics, diameter, diameter);
            Color[] colors = new Color[diameter * diameter];

            for (int x = 0; x < diameter; x++) {
                for (int y = 0; y < diameter; y++) {
                    int i = x * diameter + y;
                    Vector2 pos = new Vector2(x - radius, y - radius);
                    if (pos.Length() > radius)
                        colors[i] = Color.Transparent;
                    else if (pos.Length() > radius - outlineWidth)
                        colors[i] = outlineColor;
                    else
                        colors[i] = color;
                }
            }

            texture.SetData<Color>(colors);
            return texture;
        }
        public static Texture2D Circle(Color color, int radius, int outlineWidth = 0) {
            Color outlineColor = outlineWidth > 0 ? Color.Lerp(color, Color.Black, 0.2f) : Color.Black;
            return Circle(color, radius, outlineWidth, outlineColor);
        }

        // Draws rectangular texture
        public static Texture2D Rect(Color color, int height, int width) {
            Texture2D texture = new Texture2D(graphics, height, width);
            Color[] colors = new Color[height * width];

            for (int i = 0; i < colors.Length; ++i) colors[i] = color;
            texture.SetData(colors);
            return texture;
        }

        public static Texture2D Merge(Texture2D layer1, Texture layer2, Point position) {
            // TODO: Method that takes two textures, and a position, and returns a merged texture
            return layer1;
        }
    }
}
