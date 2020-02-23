using System.Collections.Generic;

namespace EDEN {
    class Simulation : Component {

        // Settings
        int initialPopulation = 1024;
        float foodDensity = 0.8f;

        public override void Start() {
            // Spawn starting food (TEMP)
            for (int i = 0; i < (int)(initialPopulation * foodDensity); i++)
                components.Add(new Food(Rand.Range(Global.worldSize.ToVector2())));
            
            // Spawn initial population
            for (int i = 0; i < initialPopulation; i++)
                components.Add(new Creature(Rand.Range(Global.worldSize.ToVector2())));
        }
    }
}