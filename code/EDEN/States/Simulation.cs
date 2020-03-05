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

        SimulationSidebar populationDisplay;

        public Creature targeted;
        public List<Creature> creatures = new List<Creature>();
        public List<Component> foods = new List<Component>();
        
        public static Texture2D[] branchTextures = new Texture2D[17];

        string[] nouns;

        public Simulation(Application _app, Settings _settings) : base(_app) {
            settings = _settings;

            // Set up quad tree textures for displaying it
            for (int i = 0; i < branchTextures.Length; i++)
                branchTextures[i] = Textures.Rect(Color.Transparent, settings.envSize, settings.envSize, 2 * (int)(Math.Pow(2, i)), Color.Goldenrod);
            
            quadTree = new QuadTree(new Rectangle(Point.Zero, new Point(settings.envSize)), 0, branchTextures);
            environment = new Environment(new Vector2(settings.envSize) / 2, new Point(settings.envSize), 16, 0.5f + settings.waterLevel, 9);
        }

        public override void Start() {
            bgColor = Color.DodgerBlue;

            // Unlocks the camera so that it can be moved and zoomed
            camera.locked = false;

            // Initializes side panel display and adds it to components
            populationDisplay = new SimulationSidebar(new Vector2(120, app.screenSize.Y / 40), 240, (int)app.screenSize.Y, 20);
            AddComponent(populationDisplay);

            AddComponent(new Button(60, 30, Color.PaleVioletRed, new Vector2(app.screenSize.X - 30, 15), "Exit", () => {
                app.SwitchState(new MainMenu(app));
            }));
            AddComponent(new Button(60, 30, Color.White, new Vector2(app.screenSize.X - 30, 45), "Save", () => {
                Serialization.SaveState(this);
            }));


            AddComponent(environment);

            // Adds creatures to components list. This is used when a saved simulation is loaded in,
            // the creatures list is updated from the file.
            foreach (Creature creature in creatures)
                AddComponent(creature);

            // Reads the nouns in from a text file in the content directory, for creature names
            nouns = File.ReadAllLines(app.Content.RootDirectory + "/nounlist.txt");
        }

        public override void Update(float deltaTime) {
            float highestAge = 0;
            Creature highestAgeCreature = null;
            int highestChildren = 0;
            Creature highestChildrenCreature = null;
            float highestGeneration = 0;
            Creature highestGenerationCreature = null;

            populationDisplay.UpdateDetails(this);
            // Update most successful creatures
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
            
            // Highlights most successful creatures
            highestAgeCreature?.Highlight(Color.Red);
            highestChildrenCreature?.Highlight(Color.Blue);
            highestGenerationCreature?.Highlight(Color.Yellow);

            // If a creature is targeted, follow it with the camera
            if (targeted != null)
                camera.position = targeted.position;

            // Spawns new food and creatures if they fall below min count
            while (creatures.Count < settings.population)
                SpawnNewCreature();
            while (foods.Count < (int)(settings.population * settings.foodDensity))
                SpawnNewFood();
        }

        public void SpawnNewCreature(Vector2 position) {
            Creature newCreature = new Creature(position, Rand.Choice(nouns), this);
            creatures.Add(newCreature);
            AddComponent(newCreature);
        }
        public void SpawnNewCreature() {
            SpawnNewCreature(Rand.Range(new Vector2(settings.envSize)));
        }

        public void SpawnNewFood(Vector2 position) {
            // Spawns food at position a creature died at
            if (environment.CheckTile(position)) {
                Food newFood = new Food(position, Color.Beige, 8);
                foods.Add(newFood);
                AddComponent(newFood);
            }
        }
        public void SpawnNewFood() {
            SpawnNewFood(Rand.Range(new Vector2(settings.envSize)));
        }

        public override void HandleInput() {
            if (Input.Click())
                SpawnNewFood(Input.MouseWorldPos.ToVector2());

            // Stops focusing on a creature if any camera movement buttons are pressed
            if (Input.Press(Keys.W) || Input.Press(Keys.A) || Input.Press(Keys.S) || Input.Press(Keys.D)
                || Input.Press(Keys.Up) || Input.Press(Keys.Left) || Input.Press(Keys.Down) || Input.Press(Keys.Right))
                targeted = null;

            // Displays quad tree if Q is pressed
            if (Input.Press(Keys.Q))
                debug = !debug;

            // Left square bracket speeds up sim and left slows down
            if (Input.Press(Keys.OemCloseBrackets))
                runSpeed += 0.5f;
            if (Input.Press(Keys.OemOpenBrackets))
                runSpeed -= 0.5f;
        }
    }
}