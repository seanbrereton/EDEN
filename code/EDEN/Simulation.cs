using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace EDEN {
    class Simulation : State {

        // Settings
        int minPopulation = 256;
        int initialPopulation = 1024;
        float foodDensity = 1.6f;

        public bool running = false;

        public List<Creature> creatures = new List<Creature>();
        public List<Component> foods = new List<Component>();

        public Simulation(Application _app) : base(_app) {
            quadTree = new QuadTree(new Rectangle(Point.Zero, Global.worldSize));
        }

        public override void Start() {
            bgColor = Color.DarkSlateBlue;

            Entity background = new Entity(new Point(Global.worldSize.X/2, Global.worldSize.Y/2).ToVector2());
            background.texture = Textures.Rect(Color.DarkOliveGreen, Global.worldSize.X, Global.worldSize.Y);
            AddComponent(background);

            // Spawn starting food (TEMP)
            for (int i = 0; i < (int)(initialPopulation * foodDensity); i++)
                SpawnNewFood();
            
            // Spawn initial population
            for (int i = 0; i < initialPopulation; i++) {
                SpawnNewCreature();
            }
        }

        public override void Update(float deltaTime) {
            int highestGeneration = 0;
            float highestAge = 0;

            foreach (Creature creature in creatures) {
                if (creature.generation > highestGeneration)
                    highestGeneration = creature.generation;
                if (creature.age > highestAge)
                    highestAge = creature.age;
            }

            Console.WriteLine("===\nPop: " + creatures.Count + "\nGen: " + highestGeneration + "\nAge: " + highestAge);

            while (creatures.Count < minPopulation)
                SpawnNewCreature();
            while (foods.Count < (int)(initialPopulation * foodDensity))
                SpawnNewFood();
        }

        public void SpawnNewCreature(Vector2 position) {
            Creature newCreature = new Creature(position);
            creatures.Add(newCreature);
            AddComponent(newCreature);
        }
        public void SpawnNewCreature() {
            SpawnNewCreature(Rand.Range(Global.worldSize.ToVector2()));
        }

        public void SpawnNewFood(Vector2 position) {
            Food newFood = new Food(position);
            foods.Add(newFood);
            AddComponent(newFood);
        }
        public void SpawnNewFood() {
            SpawnNewFood(Rand.Range(Global.worldSize.ToVector2()));
        }

        public override void HandleInput() {
            if (Input.Click(0, true))
                if (Input.Press(Keys.LeftShift, true))
                    for (int x = 0; x < 16; x++)
                        SpawnNewFood(Input.MouseWorldPos.ToVector2() + Rand.Range(new Vector2(-32), new Vector2(32)));
                else
                    SpawnNewFood(Input.MouseWorldPos.ToVector2());

            if (Input.Click(1))
                for (int i = 0; i < 1; i++)
                    SpawnNewCreature(Input.MouseWorldPos.ToVector2());

            if (Input.Press(Keys.Q))
                debug = !debug;
        }
    }
}