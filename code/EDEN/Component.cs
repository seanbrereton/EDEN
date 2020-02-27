using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace EDEN {
    public class Component {

        public bool UI;

        public Component parent;
        public List<Component> components = new List<Component>();
        public List<Component> toAdd = new List<Component>();
        public List<Component> toRemove = new List<Component>();

        public List<Component> GetChildComponents() {
            List<Component> childComponents = new List<Component>(components);

            foreach (Component component in components)
                childComponents.AddRange(component.GetChildComponents());

            return childComponents;
        }

        public void Remove() {
            parent.toRemove.Add(this);
        }

        public void AddComponent(Component component) {
            component.parent = this;
            toAdd.Add(component);
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

            foreach (Component component in toRemove)
                components.Remove(component);
            foreach (Component component in toAdd)
                components.Add(component);
            toRemove.Clear();
            toAdd.Clear();

            foreach (Component component in components)
                component.SuperUpdate(deltaTime);
        }

        public virtual void Draw(SpriteBatch spriteBatch) { }
        public virtual void SuperDraw(SpriteBatch spriteBatch, SpriteBatch UIspriteBatch) {
            Draw(UI ? UIspriteBatch : spriteBatch);

            foreach (Component component in components)
                component.SuperDraw(spriteBatch, UIspriteBatch);
        }

    }
}
