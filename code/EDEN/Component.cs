using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace EDEN {
    public class Component {

        public List<Component> components = new List<Component>();

        public virtual void Start() { }
        public void SuperStart() {
            Start();

            foreach (Component component in components)
                component.SuperStart();
        }

        public virtual void Update() { }
        public virtual void SuperUpdate() {
            Update();

            foreach (Component component in components)
                component.SuperUpdate();
        }

        public virtual void Draw(SpriteBatch spriteBatch) { }
        public virtual void SuperDraw(SpriteBatch spriteBatch) {
            Draw(spriteBatch);

            foreach (Component component in components)
                component.SuperDraw(spriteBatch);
        }

    }
}
