using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace EDEN {
    public class Component {

        public bool delete;

        public List<Component> components = new List<Component>();

        public List<Component> Components {
            get {
                List<Component> comps = new List<Component>(components);

                foreach (Component component in components) {
                    comps.AddRange(component.Components);
                }

                return comps;
            }
        }

        public virtual void Start() { }
        public void SuperStart() {
            Start();

            foreach (Component component in components)
                component.SuperStart();
        }

        public virtual void HandleInput() { }
        public virtual void Update(GameTime gameTime) { }
        public virtual void SuperUpdate(GameTime gameTime) {
            Update(gameTime);
            HandleInput();

            List<Component> toDelete = new List<Component>();

            foreach (Component component in components) {
                if (component.delete)
                    toDelete.Add(component);
                else
                    component.SuperUpdate(gameTime);
            }

            foreach (Component component in toDelete)
                components.Remove(component);
        }

        public virtual void Draw(SpriteBatch spriteBatch) { }
        public virtual void SuperDraw(SpriteBatch spriteBatch) {
            Draw(spriteBatch);

            foreach (Component component in components)
                component.SuperDraw(spriteBatch);
        }

    }
}
