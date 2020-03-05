using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace EDEN {
    
    class PopulationDisplay : UI {

        // Possible stats to sort the creatures by
        public enum SortOrder {
            AGE, GENERATION, CHILDREN
        };
        SortOrder sortOrder;

        public CreatureDisplay[] displays;

        int width;
        int height;

        public PopulationDisplay(Vector2 _position, int _width, int _height, int amount) : base(_position) {
            width = _width;
            height = _height;
            texture = Textures.Rect(Color.White, width, height);
            displays = new CreatureDisplay[amount];
        }

        public override void Start() {
            // Adds creature displays to display list, with correct positions
            for (int i = 0; i < displays.Length; i++) {
                displays[i] = new CreatureDisplay(position + new Vector2(0, i * height), width, height);
                AddComponent(displays[i]);
            }
        }

        public void UpdateCreatures (List<Creature> creatures) {
            // Updates list of creature displays to show the creatures sorted in the right order
            creatures = SortCreatures(creatures);
            for (int i = 0; i < displays.Length && i < creatures.Count; i++)
                displays[i].creature = creatures[i];
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

        public override void HandleInput() {
            // Allows the number keys to change the sort order of the display
            if (Input.Press(Keys.D1))
                sortOrder = SortOrder.AGE;
            if (Input.Press(Keys.D2))
                sortOrder = SortOrder.CHILDREN;
            if (Input.Press(Keys.D3))
                sortOrder = SortOrder.GENERATION;
        }

    }
}
