using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace EDEN {
    public static class Input {

        static MouseState prevMouse;
        static MouseState mouse;

        static KeyboardState prevKeyboard;
        static KeyboardState keyboard;

        static Camera camera;

        public static void Update() {
            prevMouse = mouse;
            mouse = Mouse.GetState();
            prevKeyboard = keyboard;
            keyboard = Keyboard.GetState();
        }

        public static Point MousePos {
            get {
                return mouse.Position;
            }
        }

        public static Point MouseWorldPos {
            get {
                Matrix invertedMatrix = Matrix.Invert(camera.transform);
                return Vector2.Transform(MousePos.ToVector2(), invertedMatrix).ToPoint();
            }
        }

        public static void Initialize(Camera _camera) {
            camera = _camera;
        }

        public static bool Click(int button=0, bool held=false) {
            ButtonState now = button == 0 ? mouse.LeftButton : mouse.RightButton;
            ButtonState then = button == 0 ? prevMouse.LeftButton : prevMouse.RightButton;
            
            return now == ButtonState.Pressed && (held || now != then);
        }

        public static bool Press(Keys key, bool held = false) {
            return keyboard.IsKeyDown(key) && (held || prevKeyboard.IsKeyUp(key));
        }

        public static bool ScrollUp() {
            return mouse.ScrollWheelValue > prevMouse.ScrollWheelValue;
        }

        public static bool ScrollDown() {
            return mouse.ScrollWheelValue < prevMouse.ScrollWheelValue;
        }

    }
}
