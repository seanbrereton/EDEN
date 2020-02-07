using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace EDEN {
    public static class Textures {
        static GraphicsDevice graphics;
        
        public static void Init(Game app) {
            graphics = app.GraphicsDevice;
        }

        public static Texture2D Circle(Color color, int radius) {
            int diameter = radius * 2;
            Texture2D texture = new Texture2D(graphics, diameter, diameter);
            Color[] colors = new Color[diameter * diameter];


            for (int x = 0; x < diameter; x++) {
                for (int y = 0; y < diameter; y++) {
                    int i = x * diameter + y;
                    Vector2 pos = new Vector2(x - radius, y - radius);
                    if (pos.Length() > radius)
                        colors[i] = Color.Transparent;
                    else if (pos.Length() > radius - 4)
                        colors[i] = Color.Lerp(color, Color.Black, 0.2f);
                    else
                        colors[i] = color;
                }
            }

            texture.SetData<Color>(colors);
            return texture;
        }

        public static Texture2D Rect(Color color, int height, int width) {
            Texture2D texture = new Texture2D(graphics, height, width);
            Color[] colors = new Color[height * width];

            for (int i = 0; i < colors.Length; ++i) colors[i] = color;
            texture.SetData(colors);
            return texture;
        }
    }
}
