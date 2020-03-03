using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDEN {
    
    class UI : Entity {

        public string text;
        public Color fontColor = Color.Black;

        public UI(Vector2 _position) : base(_position) {}

        public override void SuperDraw(SpriteBatch spriteBatch, SpriteBatch UIspriteBatch) {
            base.SuperDraw(spriteBatch, UIspriteBatch);

            if (text != null) {
                float x = (rect.X + (rect.Width / 2)) - (Application.font.MeasureString(text).X / 2);
                float y = (rect.Y + (rect.Height / 2)) - (Application.font.MeasureString(text).Y / 2);
                UIspriteBatch.DrawString(Application.font, text, new Vector2(x, y), fontColor);
            }
        }

    }
}
