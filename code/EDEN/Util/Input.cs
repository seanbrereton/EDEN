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
            // Updates mouse and keyboard states
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
            // Returns mouse position in relation to sceen point
            get {
                Matrix invertedMatrix = Matrix.Invert(camera.transform);
                return Vector2.Transform(MousePos.ToVector2(), invertedMatrix).ToPoint();
            }
        }

        public static void Initialize(Camera _camera) {
            // Initialized with camera so that the mouse's world position can be calculated 
            camera = _camera;
        }

        public static bool Click(int button=0, bool held=false) {
            // Gets current and previous state of given mouse button (0 = left, 1 = right)
            ButtonState now = button == 0 ? mouse.LeftButton : mouse.RightButton;
            ButtonState then = button == 0 ? prevMouse.LeftButton : prevMouse.RightButton;
            
            // If held is true, return true if the button is pressed
            // Otherwise, only return true if the button is pressed now, but not in the previous state
            return now == ButtonState.Pressed && (held || now != then);
        }

        public static bool Press(Keys key, bool held = false) {
            // If held is true, return true if the key is pressed
            // Otherwise, only return true if the key is pressed now, but not in the previous state
            return keyboard.IsKeyDown(key) && (held || prevKeyboard.IsKeyUp(key));
        }

        public static bool ScrollUp() {
            // Check if mouse scrollwheel is being scrolled up
            return mouse.ScrollWheelValue > prevMouse.ScrollWheelValue;
        }

        public static bool ScrollDown() {
            // Check if mouse scrollwheel is being scrolled down
            return mouse.ScrollWheelValue < prevMouse.ScrollWheelValue;
        }

    }
}
