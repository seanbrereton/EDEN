using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace EDEN {
    class Simulation : State {

        // Settings
        int initialPopulation = 50;
        float foodDensity = 0.8f;

        public bool running = false;

        public override void Start() {
            running = true;

            // Spawn starting food (TEMP)
            for (int i = 0; i < (int)(initialPopulation * foodDensity); i++)
                components.Add(new Food(Rand.Range(Global.worldSize.ToVector2())));
            
            // Spawn initial population
            for (int i = 0; i < initialPopulation; i++)
                components.Add(new Creature(Rand.Range(Global.worldSize.ToVector2())));
        }

        public override void HandleInput() {
            if (Input.Click(0, true))
                if (Input.Press(Keys.LeftShift, true))
                    for (int x = 0; x < 16; x++)
                        components.Add(new Food(Input.mousePos.ToVector2() + Rand.Range(new Vector2(-32), new Vector2(32))));
                else
                    components.Add(new Food(Input.mousePos.ToVector2()));

            if (Input.Click(1))
                components.Add(new Creature(Input.mousePos.ToVector2()));
        }
    }
}