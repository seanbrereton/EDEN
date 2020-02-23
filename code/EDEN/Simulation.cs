using System.Collections.Generic;

namespace EDEN {
    class Simulation : Component {

        // Entities
        List<Creature> creatures = new List<Creature>();
        List<Food> foods = new List<Food>();

        // Settings
        int initialPopulation = 1024;
        float foodDensity = 0.4f;

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
    }
}