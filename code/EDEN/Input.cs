using Microsoft.Xna.Framework.Input;

namespace EDEN {
    public static class Input {

        static MouseState prevMouse;
        static MouseState mouse;

        static KeyboardState prevKeyboard;
        static KeyboardState keyboard;

        public static void Update() {
            prevMouse = mouse;
            mouse = Mouse.GetState();
            prevKeyboard = keyboard;
            keyboard = Keyboard.GetState();
        }

        public static bool Click(int button=0, bool held=false) {
            ButtonState now = button == 0 ? mouse.LeftButton : mouse.RightButton;
            ButtonState then = button == 0 ? prevMouse.LeftButton : prevMouse.RightButton;
            
            return now == ButtonState.Pressed && (held || now != then);
        }

        public static bool Press(Keys key, bool held = false) {
            return keyboard.IsKeyDown(key) && (held || prevKeyboard.IsKeyUp(key));
        }

    }
}
