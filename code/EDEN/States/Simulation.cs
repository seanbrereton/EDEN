using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EDEN {
    
    public class Simulation : State {

        public Settings settings;
        public Environment environment;

        PopulationDisplay populationDisplay;

        public Creature targeted;
        public List<Creature> creatures = new List<Creature>();
        public List<Component> foods = new List<Component>();
        
        public static Texture2D[] branchTextures = new Texture2D[17];

        string[] nouns;

        public Simulation(Application _app, Settings _settings) : base(_app) {
            settings = _settings;

            for (int i = 0; i < branchTextures.Length; i++)
                branchTextures[i] = Textures.Rect(Color.Transparent, settings.envSize, settings.envSize, 2 * (int)(Math.Pow(2, i)), Color.Goldenrod);
            
            quadTree = new QuadTree(new Rectangle(Point.Zero, new Point(settings.envSize)), 0, branchTextures);
            environment = new Environment(new Vector2(settings.envSize) / 2, new Point(settings.envSize), 16, 0.62f, 9);
        }

        public override void Start() {
            bgColor = Color.DodgerBlue;
            camera.locked = false;

            populationDisplay = new PopulationDisplay(new Vector2(120, 22.5f), 240, 45);
            AddComponent(populationDisplay);

            AddComponent(environment);

            foreach (Creature creature in creatures)
                AddComponent(creature);

            nouns = File.ReadAllLines(app.Content.RootDirectory + "/nounlist.txt");
        }

        public override void Update(float deltaTime) {
            float highestAge = 0;
            Creature highestAgeCreature = null;
            int highestChildren = 0;
            Creature highestChildrenCreature = null;
            float highestGeneration = 0;
            Creature highestGenerationCreature = null;

            populationDisplay.UpdateCreatures(creatures);

            foreach (Creature creature in creatures) {
                if (creature.age > highestAge) {
                    highestAge = creature.age;
                    highestAgeCreature = creature;
                }
                if (creature.childrenCount > highestChildren) {
                    highestChildren = creature.childrenCount;
                    highestChildrenCreature = creature;
                }
                if (creature.generation > highestGeneration) {
                    highestGeneration = creature.generation;
                    highestGenerationCreature = creature;
                }
            }

            highestAgeCreature?.Highlight(Color.Red);
            highestChildrenCreature?.Highlight(Color.Blue);
            highestGenerationCreature?.Highlight(Color.Yellow);

            if (targeted != null)
                camera.position = targeted.position;

            while (creatures.Count < settings.population)
                SpawnNewCreature();
            while (foods.Count < (int)(settings.population * settings.foodDensity))
                SpawnNewFood();
        }

        public Creature SpawnNewCreature(Vector2 position) {
            Creature newCreature = new Creature(position, Rand.Choice(nouns));
            creatures.Add(newCreature);
            AddComponent(newCreature);
            return newCreature;
        }
        public Creature SpawnNewCreature() {
            return SpawnNewCreature(Rand.Range(new Vector2(settings.envSize)));
        }

        public void SpawnNewFood(Vector2 position) {
            if (environment.CheckTile(position)) {
                Food newFood = new Food(position, 8);
                foods.Add(newFood);
                AddComponent(newFood);
            }
        }
        public void SpawnNewFood() {
            SpawnNewFood(Rand.Range(new Vector2(settings.envSize)));
        }

        public override void HandleInput() {
            if (Input.Press(Keys.W) || Input.Press(Keys.A) || Input.Press(Keys.S) || Input.Press(Keys.D)
                || Input.Press(Keys.Up) || Input.Press(Keys.Left) || Input.Press(Keys.Down) || Input.Press(Keys.Right))
                targeted = null;

            if (Input.Press(Keys.Q))
                debug = !debug;

            if (Input.Press(Keys.OemCloseBrackets))
                runSpeed += 0.5f;
            if (Input.Press(Keys.OemOpenBrackets))
                runSpeed -= 0.5f;

            if (Input.Press(Keys.OemQuestion))
                Serialization.SaveState(this);
        }
    }
}