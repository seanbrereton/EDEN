using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace EDEN {
    public static class Textures {
        static GraphicsDevice graphics;
        
        public static void Init(Game app) {
            graphics = app.GraphicsDevice;
        }

        public static Texture2D Empty(int width, int height) {
            return new Texture2D(graphics, width, height);
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

        public static Texture2D Rect(Color color, int width, int height, int outlineWidth, Color outlineColor) {
            Texture2D texture = new Texture2D(graphics, width, height);
            Color[] colors = new Color[width * height];

            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    int i = y * width + x;
                    if (x < outlineWidth || width - x < outlineWidth || y < outlineWidth || height - y < outlineWidth)
                        colors[i] = outlineColor;
                    else
                        colors[i] = color;
                }
            }

            texture.SetData<Color>(colors);
            return texture;
        }
        public static Texture2D Rect(Color color, int width, int height, int outlineWidth = 0) {
            Color outlineColor = outlineWidth > 0 ? Color.Lerp(color, Color.Black, 0.2f) : Color.Black;
            return Rect(color, width, height, outlineWidth, outlineColor);
        }

        public static Texture2D Merge(Texture2D layer1, Texture2D layer2, Point position) {
            int height = layer1.Height;
            int width = layer1.Width;
            Rectangle addedRect = new Rectangle(position, new Point(layer2.Width, layer2.Height));
            Texture2D texture = new Texture2D(graphics, height, width);
            Color[] colors = new Color[height * width];
            Color[] layer1Colors = new Color[height * width];
            layer1.GetData(layer1Colors);
            Color[] layer2Colors = new Color[addedRect.Width * addedRect.Height];
            layer2.GetData(layer2Colors);

            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    int i = x * width + y;
                    if (addedRect.Contains(x, y))
                        colors[i] = layer2Colors[(x + addedRect.X) * addedRect.Width + (y + addedRect.Y)];
                    else
                        colors[i] = layer1Colors[i];
                }
            }

            texture.SetData(colors);
            return texture;
        }
    }
}
