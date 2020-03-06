using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EDEN {
    
    class SimulationSidebar : UI {

        // Possible stats to sort the creatures by
        public enum SortOrder {
            AGE, GENERATION, CHILDREN
        };
        SortOrder sortOrder;

        TextBox populationCount;
        TextBox foodCount;
        public Button[] creatureButtons;
        CreatureDisplay creatureDisplay;

        int width;
        int height;
        int buttonCount;

        public SimulationSidebar(Vector2 _position, int _width, int _height, int _buttonCount) : base(_position) {
            width = _width;
            height = _height;
            buttonCount = _buttonCount;
            texture = Textures.Rect(Color.White, width, height / 10);
        }

        public override void Start() {
            Vector2 textPosition = new Vector2(position.X, height / (buttonCount * 2));
            populationCount = new TextBox(textPosition);
            AddComponent(populationCount);
            textPosition.Y += height / (buttonCount * 2);
            foodCount = new TextBox(textPosition);
            AddComponent(foodCount);

            creatureDisplay = new CreatureDisplay(new Vector2(width / 2, width / 2), width, width / 2);
            AddComponent(creatureDisplay);

            // Adds creature buttons to list, with correct positions
            creatureButtons = new Button[buttonCount];
            for (int i = 0; i < creatureButtons.Length; i++) {
                creatureButtons[i] = new Button(
                    width, (int)(height - 0.75f * width) / buttonCount, Color.White, 
                    new Vector2(position.X, 0.75f * width + (0.5f + i) * (height - 0.75f * width) / buttonCount),
                    "", null
                );
                AddComponent(creatureButtons[i]);
            }
        }

        public void UpdateDetails (Simulation sim) {
            if (sim.targeted != null)
                creatureDisplay.SetCreature(sim.targeted);

            populationCount.text = "Population: " + sim.creatures.Count.ToString();
            foodCount.text = "Food count: " + sim.foods.Count.ToString();

            // Updates list of buttons to show the creatures sorted in the right order
            List<Creature> creatures = SortCreatures(sim.creatures);
            for (int i = 0; i < creatureButtons.Length && i < creatures.Count; i++) {
                Button button = creatureButtons[i];
                Creature creature = creatures[i];
                button.text = "age: " +
                    Math.Round(creature.age, 1).ToString() + "  |  child: " +
                    creature.childrenCount.ToString() + "  |  gen: " + creature.generation.ToString();

                button.defaultColor = creature.color;
                button.hoverColor = Color.Lerp(creature.color, Color.Black, 0.3f);

                button.fontColor = Math.Sqrt(
                   creature.color.R * creature.color.R * 0.21 +
                   creature.color.G * creature.color.G * 0.72 +
                   creature.color.B * creature.color.B * 0.07
                ) > 127 ? Color.Black : Color.White;

                button.action = () => {
                    creature.Target();
                };
            }
        }

        List<Creature> SortCreatures(List<Creature> creatures) {
            // Sorts list by different attributes and returns sorted list
            switch (sortOrder) {
                case SortOrder.AGE:
                    return creatures.OrderBy(c => -c.age).ToList();
                case SortOrder.CHILDREN:
                    return creatures.OrderBy(c => -c.childrenCount).ToList();
                case SortOrder.GENERATION:
                    return creatures.OrderBy(c => -c.generation).ToList();
                default:
                    return creatures;
            }
        }

        void ToggleDetails() {
            creatureDisplay.active = !creatureDisplay.active;
            foreach (Button button in creatureButtons)
                button.active = !button.active;
        }

        public override void HandleInput() {
            // Allows the number keys to change the sort order of the display
            if (Input.Press(Keys.D1))
                sortOrder = SortOrder.AGE;
            if (Input.Press(Keys.D2))
                sortOrder = SortOrder.CHILDREN;
            if (Input.Press(Keys.D3))
                sortOrder = SortOrder.GENERATION;

            if (Input.Click()) {
                Point mouse = Input.MousePos;
                if (mouse.X <= width && mouse.Y <= height / 10)
                    ToggleDetails();
            }
        }

    }
}
