using System.Collections.Generic;

namespace EDEN {
    class Simulation : Component {

        // Settings
        int initialPopulation = 1024;
        float foodDensity = 0.4f;

        public override void Start() {
            // Spawn starting food (TEMP)
            for (int i = 0; i < (int)(initialPopulation * foodDensity); i++)
                components.Add(new Food());
            
            // Spawn initial population
            for (int i = 0; i < initialPopulation; i++)
                components.Add(new Creature());
        }
    }
}