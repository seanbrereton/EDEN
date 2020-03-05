using Microsoft.Xna.Framework;

namespace EDEN {
    class TextBox : UI {

        public TextBox(Vector2 _position, string _text) : base(_position) {
            text = _text;
            // Gets an empty texture for this textbox
            texture = Textures.Empty(1, 1);
        }

        public TextBox(Vector2 _position) : this(_position, "") {}

    }
}
