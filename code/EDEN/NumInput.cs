using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EDEN {
    class NumInput : Entity {

        int minValue;
        int maxValue;
        public int value;
        string text;


        public NumInput(string _text, int startingValue, int min, int max, Vector2 pos) : base(pos) {
            text = _text;
            texture = Textures.Rect(Color.Gray, 200, 30);

            minValue = min;
            maxValue = max;
            value = startingValue;

            AddComponent(new Button(30, 30, Color.White, new Vector2(position.X +160, position.Y), "+", () => {
                value = Math.Min(maxValue, value + 1);
            }));

            AddComponent(new Button(30, 30 , Color.White, new Vector2(position.X + 120, position.Y), "-", () => {
                value = Math.Max(minValue, value - 1);
            }));
        }

        public override void Draw(SpriteBatch spriteBatch) {
            string display = text + ": " + value.ToString();
            float x = (rect.X + (rect.Width / 2)) - (Application.font.MeasureString(display).X / 2);
            float y = (rect.Y + (rect.Height / 2)) - (Application.font.MeasureString(display).Y / 2);
            spriteBatch.DrawString(Application.font, display, new Vector2(x, y), Color.Black);
        }
    }
}
