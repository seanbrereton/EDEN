﻿using Microsoft.Xna.Framework;
using System;

namespace EDEN {
    class PopUp : UI {

        float lifeTime = 6;

        public PopUp(Vector2 _position, Color _color, int width, int height, string _text) : base(_position) {
            texture = Textures.Rect(Color.White, width, height);
            color = _color;
            fontColor = Color.White;
            text = _text;
        }

        public override void Update(float deltaTime) {
            // Removes component after it has been on screen long enough for its lifeTime timer to reach 0
            lifeTime -= deltaTime;
            if (lifeTime <= 0)
                Remove();
        }

    }
}
