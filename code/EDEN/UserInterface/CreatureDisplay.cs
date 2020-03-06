using Microsoft.Xna.Framework;
using System;

namespace EDEN {
    class CreatureDisplay : UI {

        int width;
        int height;

        public TextBox name;
        public TextBox energy;
        public TextBox age;
        public TextBox children;
        public TextBox generation;

        public Creature creature;

        public CreatureDisplay(Vector2 _position, int _width, int _height) : base(_position) {
            width = _width;
            height = _height;
            texture = Textures.Rect(Color.White, width, height);
        }

        public override void Start() {
            Vector2 textPosition = new Vector2(width / 2, position.Y - height / 2 + height / 6);
            name = new TextBox(textPosition);
            AddComponent(name);
            textPosition.Y += height / 6;
            energy = new TextBox(textPosition);
            AddComponent(energy);
            textPosition.Y += height / 6;
            age = new TextBox(textPosition);
            AddComponent(age);
            textPosition.Y += height / 6;
            children = new TextBox(textPosition);
            AddComponent(children);
            textPosition.Y += height / 6;
            generation = new TextBox(textPosition);
            AddComponent(generation);
        }

        public void SetCreature(Creature _creature) {
            creature = _creature;
            color = creature.color;

            fontColor = Math.Sqrt(
               color.R * color.R * 0.21 +
               color.G * color.G * 0.72 +
               color.B * color.B * 0.07
            ) > 127 ? Color.Black : Color.White;

            name.fontColor = fontColor;
            energy.fontColor = fontColor;
            age.fontColor = fontColor;
            children.fontColor = fontColor;
            generation.fontColor = fontColor;
        }

        public override void Update(float deltaTime) {
            if (creature != null) {
                name.text = "Name: " + creature.name;
                energy.text = "Energy: " + Math.Round((double)creature.energy) + " / " + creature.sim.settings.maxEnergy;
                age.text = "Age: " + Math.Round(creature.age);
                children.text = "Children: " + creature.childrenCount;
                generation.text = "Generation: " + creature.generation;
            }
        }

    }
}
