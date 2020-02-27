using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace EDEN {
    public class Component {

        public bool UI;
        public bool delete;

        public List<Component> components = new List<Component>();

        public List<Component> GetChildComponents() {
            List<Component> childComponents = new List<Component>(components);

            foreach (Component component in components)
                childComponents.AddRange(component.GetChildComponents());

            return childComponents;
        }

        public virtual void Start() { }
        public virtual void SuperStart() {
            Start();

            foreach (Component component in components)
                component.SuperStart();
        }

        public virtual void HandleInput() { }
        public virtual void Update(float deltaTime) { }
        public virtual void SuperUpdate(float deltaTime) {
            Update(deltaTime);
            HandleInput();

            List<Component> toDelete = new List<Component>();

            foreach (Component component in components) {
                if (component.delete)
                    toDelete.Add(component);
                else
                    component.SuperUpdate(deltaTime);
            }

            foreach (Component component in toDelete)
                components.Remove(component);
        }

        public virtual void Draw(SpriteBatch spriteBatch) { }
        public virtual void SuperDraw(SpriteBatch spriteBatch, SpriteBatch UIspriteBatch) {
            Draw(UI ? UIspriteBatch : spriteBatch);

            foreach (Component component in components)
                component.SuperDraw(spriteBatch, UIspriteBatch);
        }

    }
}
