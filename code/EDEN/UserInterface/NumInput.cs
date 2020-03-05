using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EDEN {
    class NumInput : UI {

        // Used for simulation value configuration buttons

        float minValue;
        float maxValue;
        public float value;
        string displayName;

        public NumInput(string _displayName, float startingValue, float min, float max, Vector2 pos, float increment) : base(pos) {
            texture = Textures.Rect(Color.White, 180, 30);

            displayName = _displayName;

            // Value is always kept bound between min and max value
            minValue = min;
            maxValue = max;
            value = startingValue;

            // Adds button to right that increases value by given increment
            AddComponent(new Button(30, 30, Color.White, new Vector2(position.X +160, position.Y), "+", () => {
                value = Math.Min(maxValue, (float)Math.Round((double)value + increment, 2));
            }));

            // Adds button to right that increases value by given increment
            AddComponent(new Button(30, 30 , Color.White, new Vector2(position.X + 120, position.Y), "-", () => {
                value = Math.Max(minValue, (float)Math.Round((double)value - increment, 2));
            }));
        }

        public override void Update(float deltaTime) {
            text = displayName + ": " + value.ToString();
        }
    }
}
