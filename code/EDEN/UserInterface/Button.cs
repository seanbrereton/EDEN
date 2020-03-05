using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace EDEN {
    
    class Button : UI {

        public Color hoverColor;
        public Color defaultColor;
        public Action action;

        // Button constructor
        public Button(int width, int height, Color col, Vector2 pos, String _text, Action _action) : base(pos) {
            texture = Textures.Rect(Color.White, width, height);
            defaultColor = col;
            // Hover colour is the default colour darkened
            hoverColor = Color.Lerp(col, Color.Black, 0.4f);
            text = _text;
            action = _action;
        }
        
        public override void HandleInput() {
            // Check if the mouse position is within the bounds of the button
            bool hovered = rect.Contains(Input.MousePos);

            // Changes button colour based on if it is hovered over
            if (hovered)
                color = hoverColor;
            else
                color = defaultColor;

            // If a button is pressed, its action is performed
            if (Input.Click() && hovered)
                action();
        }

    }
}
