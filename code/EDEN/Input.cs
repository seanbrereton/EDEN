using Microsoft.Xna.Framework.Input;

namespace EDEN {
    public static class Input {

        static MouseState prevMouse;
        public static MouseState mouse;

        public static void Update() {
            prevMouse = mouse;
            mouse = Mouse.GetState();
        }

        public static bool Click(int button=0, bool held=false) {
            ButtonState now = button == 0 ? mouse.LeftButton : mouse.RightButton;
            ButtonState then = button == 0 ? prevMouse.LeftButton : prevMouse.RightButton;
            
            return now == ButtonState.Pressed && (held || now != then);
        }

    }
}
