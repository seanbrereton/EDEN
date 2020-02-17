using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace EDEN {
    class Simulation : Component {

        // Entities
        List<Creature> creatures = new List<Creature>();
        List<Food> foods = new List<Food>();

        // Settings
        int initialPopulation = 256;
        float foodDensity = 0.75f;

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
        }

        public override void Update(GameTime gameTime) {
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
}