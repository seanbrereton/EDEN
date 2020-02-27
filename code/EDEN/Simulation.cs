﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace EDEN {
    class Simulation : State {

        // Settings
        int initialPopulation = 512;
        float foodDensity = 1.6f;

        public bool running = false;

        public Simulation(Application _app) : base(_app) {
            quadTree = new QuadTree(new Rectangle(Point.Zero, Global.worldSize));
        }

        public override void Start() {
            bgColor = Color.DarkOliveGreen;

            // Spawn starting food (TEMP)
            for (int i = 0; i < (int)(initialPopulation * foodDensity); i++)
                AddComponent(new Food(Rand.Range(Global.worldSize.ToVector2())));
            
            // Spawn initial population
            for (int i = 0; i < initialPopulation; i++)
                AddComponent(new Creature(Rand.Range(Global.worldSize.ToVector2())));
        }

        public override void HandleInput() {
            if (Input.Click(0, true))
                if (Input.Press(Keys.LeftShift, true))
                    for (int x = 0; x < 16; x++)
                        AddComponent(new Food(Input.MouseWorldPos.ToVector2() + Rand.Range(new Vector2(-32), new Vector2(32))));
                else
                    AddComponent(new Food(Input.MouseWorldPos.ToVector2()));

            if (Input.Click(1))
                for (int i = 0; i < 1; i++)
                    AddComponent(new Creature(Input.MouseWorldPos.ToVector2()));

            if (Input.Press(Keys.Q))
                debug = !debug;
        }
    }
}