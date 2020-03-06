using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace EDEN {
    
    public class Component {

        public bool active;

        public Component parent;
        public List<Component> components = new List<Component>();
        public List<Component> toAdd = new List<Component>();
        public List<Component> toRemove = new List<Component>();
        public Vector2 position;

        public List<Component> GetChildComponents() {
            // Recursively gets all components contained in this component

            List<Component> childComponents = new List<Component>(components);

            foreach (Component component in components)
                childComponents.AddRange(component.GetChildComponents());

            return childComponents;
        }

        public void Remove() {
            // Sets active to false, to prevent any further updates from its components
            active = false;
            // Adds to toRemove list, to be removed later in the parent component's update
            // This is needed as removing the component directly would change the length of a list while it is being iterated through
            parent.toRemove.Add(this);
        }

        public void AddComponent(Component component) {
            // Sets new component's parent to this component, and starts it
            component.parent = this;
            component.SuperStart();
            // Adds to toAdd list, to be added later in this component's update
            // This is needed as adding the new component directly would change the length of a list while it is being iterated through
            toAdd.Add(component);
        }

        // Uses a template method design pattern for Start, Update, and Draw
        // The application's active state calls its own Super methods
        // Each Super method calls the corresponding regular method,
        // calls the Super method of each child component,
        // as well as anything else it needs to handle.

        // This allows us to give child component classes these methods
        // without having to implement the same behaviours needed in each

        public virtual void Start() { }
        public virtual void SuperStart() {
            active = true;
            Start();

            if (active)
                foreach (Component component in components)
                    component.SuperStart();
        }

        public virtual void HandleInput() { }
        public virtual void Update(float deltaTime) { }
        public virtual void SuperUpdate(float deltaTime) {
            Update(deltaTime);
            HandleInput();

            // Removes and adds components that need to be removed or added
            foreach (Component component in toRemove)
                components.Remove(component);
            foreach (Component component in toAdd)
                components.Add(component);
            toRemove.Clear();
            toAdd.Clear();

            if (active)
                foreach (Component component in components)
                    component.SuperUpdate(deltaTime);
        }

        public virtual void Draw(SpriteBatch spriteBatch) { }
        public virtual void SuperDraw(SpriteBatch spriteBatch, SpriteBatch UIspriteBatch) {
            // If this component is of the type UI, draw using the UI spritebatch
            Draw(this is UI ? UIspriteBatch : spriteBatch);

            if (active)
                foreach (Component component in components)
                    component.SuperDraw(spriteBatch, UIspriteBatch);
        }

    }
}
