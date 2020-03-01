using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace EDEN {
    class Simulation : State {

        public Settings settings;
        public Environment environment;

        public List<Creature> creatures = new List<Creature>();
        public List<Component> foods = new List<Component>();
        
        public static Texture2D[] branchTextures = new Texture2D[17];

        public Simulation(Application _app, Settings _settings) : base(_app) {
            settings = _settings;

            for (int i = 0; i < branchTextures.Length; i++)
                branchTextures[i] = Textures.Rect(Color.Transparent, settings.envSize.X, settings.envSize.Y, 2 * (int)(Math.Pow(2, i)), Color.Goldenrod);
            
            quadTree = new QuadTree(new Rectangle(Point.Zero, settings.envSize), 0, branchTextures);
        }

        public override void Start() {
            bgColor = Color.DodgerBlue;

            camera.locked = false;
            environment = new Environment(settings.envSize.ToVector2() / 2, settings.envSize, 16, 0.6f, 9);
            AddComponent(environment);

            // Spawn starting food (TEMP)
            for (int i = 0; i < (int)(settings.population * settings.foodDensity); i++)
                SpawnNewFood();
            
            // Spawn initial population
            for (int i = 0; i < settings.population; i++) {
                SpawnNewCreature();
            }
        }

        public override void Update(float deltaTime) {
            int highestChildren = 0;
            Creature highestChildrenCreature = null;
            float highestAge = 0;
            Creature highestAgeCreature = null;

            foreach (Creature creature in creatures) {
                if (creature.childrenCount > highestChildren) {
                    highestChildren = creature.childrenCount;
                    highestChildrenCreature = creature;
                }
                if (creature.age > highestAge) {
                    highestAge = creature.age;
                    highestAgeCreature = creature;
                }
            }

            highestChildrenCreature?.Highlight(Color.Blue);
            highestAgeCreature?.Highlight(Color.Red);

            Console.WriteLine("===\nPop: " + creatures.Count + "\nKid: " + highestChildren + "\nAge: " + highestAge);

            while (creatures.Count < settings.population)
                SpawnNewCreature();
            while (foods.Count < (int)(settings.population * settings.foodDensity))
                SpawnNewFood();
        }

        public Creature SpawnNewCreature(Vector2 position) {
            Creature newCreature = new Creature(position);
            creatures.Add(newCreature);
            AddComponent(newCreature);
            return newCreature;
        }
        public Creature SpawnNewCreature() {
            return SpawnNewCreature(Rand.Range(settings.envSize.ToVector2()));
        }

        public void SpawnNewFood(Vector2 position) {
            if (environment.CheckTile(position)) {
                Food newFood = new Food(position);
                foods.Add(newFood);
                AddComponent(newFood);
            }
        }
        public void SpawnNewFood() {
            SpawnNewFood(Rand.Range(settings.envSize.ToVector2()));
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

            if (Input.Press(Keys.OemCloseBrackets))
                gameSpeed += 0.5f;
            if (Input.Press(Keys.OemOpenBrackets))
                gameSpeed -= 0.5f;
        }
    }
}