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
        float foodDensity = 0.25f;

        public override void Start() {
            for (int i = 0; i < initialPopulation; i++)
                creatures.Add(new Creature());

            for (int i = 0; i < (int)(initialPopulation * foodDensity); i++)
                foods.Add(new Food());

            components.AddRange(creatures);
            components.AddRange(foods);
        }

        public override void Update() {
            foreach (Creature creature in creatures)
                foreach (Food food in foods)
                    if (creature.rect.Contains(food.position))
                        food.position = Rand.Range(Application.screenSize);
        }

    }
}
