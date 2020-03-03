using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDEN {
    
    class PopulationDisplay : UI {

        public enum SortOrder {
            AGE, GENERATION, CHILDREN
        };
        SortOrder sortOrder;

        public CreatureDisplay[] displays;

        int width;
        int height;

        public PopulationDisplay(Vector2 _position, int _width, int _height) : base(_position) {
            width = _width;
            height = _height;
            texture = Textures.Rect(Color.White, width, height);
        }

        public override void Start() {
            displays = new CreatureDisplay[300];
            for (int i = 0; i < displays.Length; i++) {
                displays[i] = new CreatureDisplay(position + new Vector2(0, i * height), width, height);
                AddComponent(displays[i]);
            }
        }

        public void UpdateCreatures (List<Creature> creatures) {
            creatures = SortCreatures(creatures);
            for (int i = 0; i < displays.Length && i < creatures.Count; i++)
                displays[i].creature = creatures[i];
        }

        List<Creature> SortCreatures(List<Creature> creatures) {
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
            if (Input.Press(Keys.D1))
                sortOrder = SortOrder.AGE;
            if (Input.Press(Keys.D2))
                sortOrder = SortOrder.CHILDREN;
            if (Input.Press(Keys.D3))
                sortOrder = SortOrder.GENERATION;
        }

    }
}
