using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace EDEN {
    class Button : Entity {

        public string text;
        public Color fontColor = Color.Black;
        public Color hoverColor = Color.Gray;
        public Color defaultColor;
        public Action action;

        public Button(int width, int height, Color col, Vector2 pos, String _text, Action _action) : base(pos) {
            texture = Textures.Rect(Color.White, width, height);
            defaultColor = col;
            text = _text;
            action = _action;
        }

        public bool IsPressed() {
            return Input.Click() && rect.Contains(Input.MousePos);   
        }

        public bool IsHover() {
            return rect.Contains(Input.MousePos);   
        }
        
        public override void HandleInput() {
            if (IsHover()) {
                color = hoverColor;
            } else {
                color = defaultColor;
            }

            if (IsPressed())
                action();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if(!string.IsNullOrEmpty(text)) {
                float x = (rect.X + (rect.Width / 2)) - (Application.font.MeasureString(text).X / 2);
                float y = (rect.Y + (rect.Height / 2)) - (Application.font.MeasureString(text).Y / 2);

                spriteBatch.DrawString(Application.font, text, new Vector2(x, y), fontColor);

            }
        }

    }
}
