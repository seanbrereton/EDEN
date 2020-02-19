using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace EDEN {
    class Simulation : Component {

        QuadTree quadTree;
        bool debug;
        bool useTree = true;

        // Entities
        List<Creature> creatures = new List<Creature>();
        List<Food> foods = new List<Food>();

        // Settings
        int initialPopulation = 128;
        float foodDensity = 0.9f;

        public override void Start() {
            // Spawn starting food (TEMP)
            for (int i = 0; i < (int)(initialPopulation * foodDensity); i++)
                foods.Add(new Food());
            
            // Spawn initial population
            for (int i = 0; i < initialPopulation; i++)
                creatures.Add(new Creature());

            // Add creatures and foods to components list
            components.AddRange(foods);
            components.AddRange(creatures);

            quadTree = new QuadTree(new Rectangle(Point.Zero, new Point((int)Application.screenSize.X)));
        }

        void CreateQuadTree() {
            quadTree.Clear();
            foreach (Creature creature in creatures)
                quadTree.Insert(creature);
            foreach (Food food in foods)
                quadTree.Insert(food);
        }

        public override void Update(GameTime gameTime) {
            if (useTree) {
                CreateQuadTree();
                foreach (Creature creature in creatures) {
                    List<Entity> entities = quadTree.Query(creature.rect);
                    foreach (Entity entity in entities) {
                        if (entity is Food && creature.rect.Contains(entity.position) && creature.energy < creature.maxEnergy) {
                            creature.energy += 1;
                            entity.position = Rand.Range(Application.screenSize);
                        }
                    }
                }
            } else {
                foreach (Creature creature in creatures) {
                    foreach (Food food in foods) {
                        // If the creature rect overlaps with the food rect, move the food to a new position (TEMP)
                        if (creature.rect.Contains(food.position) && creature.energy < creature.maxEnergy) {
                            creature.energy += 1;
                            food.position = Rand.Range(Application.screenSize);
                        }
                    }
                }
            }
        }

        public override void HandleInput() {
            if (Input.Click())
                debug = !debug;
            if (Input.Click(1))
                useTree = !useTree;
        }

        public override void Draw(SpriteBatch spriteBatch) {
            if (debug)
                quadTree.Draw(spriteBatch);
        }
    }
}