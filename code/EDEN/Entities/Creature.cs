using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace EDEN {

    public class Creature : Entity {

        public NeuralNet network;
        float mutationRate = 0.05f;

        // Network Inputs
        float[] foodSeen = new float[3];
        float[] creaturesSeen = new float[3];
        float[] waterSeen = new float[3];
        float touchingFood;
        float touchingCreature;
        float touchingWater;

        // Network Outputs
        float turning;
        float movement;
        float toEat;
        public float toMate;

        // Attributes
        public string name;
        public int childrenCount;
        public int generation;
        public float age;
        public float energy;
        int viewSize = 96;

        Rectangle[] visionRects = new Rectangle[2];
        int radius = 16;
        Texture2D eyeTexture;

        // Timers
        float perceiveTimer;
        public float reproductionTimer;

        public Simulation sim;

        public Creature() { }

        public Creature(Vector2 _position, string _name, Simulation _sim) : base(_position) {
            // Constructs a creature using a random network, colour, and starting at generation 0
            name = _name;
            generation = 0;
            color = Rand.RandColor();
            sim = _sim;
            network = new NeuralNet(13, sim.settings.hiddenLayerCount, sim.settings.hiddenLayerSize, 4);
            energy = sim.settings.maxEnergy / 2;
            scale = 0.1f;
            rotation = Rand.Range(360);
        }

        public Creature(Vector2 _position, string _name, Simulation _sim, Color _color, NeuralNet _network, int _generation) : base(_position) {
            // Constructs a creature using given network, colour, and generation
            name = _name;
            generation = _generation;
            color = _color;
            sim = _sim;
            network = new NeuralNet(13, sim.settings.hiddenLayerCount, sim.settings.hiddenLayerSize, 4);
            energy = sim.settings.maxEnergy / 2;
            scale = 0.1f;
            rotation = Rand.Range(360);
        }

        public override void Start() {
            // Set dynamic to true so that it will be checked by the quad tree
            dynamic = true;

            // Construct textures for body and for eyes
            texture = Textures.Circle(Color.White, radius, 4);
            eyeTexture = Textures.Circle(Color.Black, radius / 3, radius / 6, Color.White);

            // Initialize the rects that it can use to perceive its surroundings
            for (int i = 0; i < visionRects.Length; i++)
                visionRects[i] = new Rectangle(Point.Zero, new Point(viewSize));
        }

        public override void Update(float deltaTime) {
            // Perceive only every 0.05 seconds, to increase performance
            if (perceiveTimer <= 0f) {
                Perceive();
                perceiveTimer = 0.05f;
            }

            Think();
            Act(deltaTime);
            UseEnergy(deltaTime);

            // Adjust timers
            perceiveTimer -= deltaTime;
            reproductionTimer -= deltaTime;
            age += deltaTime;

            // Until it reaches the final scale, increase size
            if (scale < 0.5f)
                scale += 0.05f * deltaTime;
        }

        void Act(float deltaTime) {
            // Adjust position and rotation based on outputs
            position += Forward * movement * 200 * deltaTime;
            rotation += turning * 240 * deltaTime;
        }

        void UseEnergy(float deltaTime) {
            // Energy drops over time, more energy is used based on movement speed, and based on if the creature is in water
            energy -= deltaTime * (1 + Math.Abs(movement) * 2) * (Math.Max(1, touchingWater * 4));

            // If energy drops to 0, the creature dies
            if (energy <= 0)
                Die();
        }

        void Die() {
            // Remove this from the simulation, and spawn a piece of food in its place
            parent.AddComponent(new Food(position, color, sim.settings.maxEnergy / 16));
            sim.creatures.Remove(this);
            Remove();
        }

        void Think() {
            float[] inputs = GetInputs();

            // Resets inputs that need to be reset
            touchingFood = 0;
            touchingCreature = 0;

            float[] outputs = network.FeedForward(inputs);

            // Sets the variables to the outputs from the network
            movement = outputs[0];
            turning = outputs[1];
            toEat = outputs[2];
            toMate = outputs[3];
        }

        float[] GetInputs() {
            // The inputs that are used in the neural network
            return new float[] {
                energy / sim.settings.maxEnergy,
                foodSeen[0],
                foodSeen[1],
                foodSeen[2],
                touchingFood,
                creaturesSeen[0],
                creaturesSeen[1],
                creaturesSeen[2],
                touchingCreature,
                waterSeen[0],
                waterSeen[1],
                waterSeen[2],
                touchingWater
            };
        }

        void Perceive() {
            foodSeen = new float[3];
            creaturesSeen = new float[3];

            // Set the vision rects positions to the left and right, and both forward
            visionRects[0].Location = (position - (Right * (radius + viewSize / 6)) + (Forward * viewSize / 2)).ToPoint();
            visionRects[1].Location = (position + (Right * (radius + viewSize / 6)) + (Forward * viewSize / 2)).ToPoint();

            List<Entity> leftFoods = new List<Entity>();
            List<Entity> leftCreatures = new List<Entity>();

            for (int i = 0; i < visionRects.Length; i++) {
                // Offset the vision rects so that they are centered on the correct position
                visionRects[i].Offset(-viewSize / 2, -viewSize / 2);

                // Check the simulation's environment, and see if this rect is in water
                waterSeen[i] = sim.environment.CheckTile(visionRects[i].Center.ToVector2()) ? 0 : 1;

                // Query quad tree for entities near this rect
                List<Entity> seen = sim.quadTree.Query(visionRects[i]);
                foreach (Entity entity in seen) {
                    // If the rect collides with this entity, add it to the appropriate seen list
                    if (visionRects[i].Intersects(entity.rect)) {
                        if (entity is Food) {
                            if (i == 0)
                                leftFoods.Add(entity);
                            else if (leftFoods.Contains(entity))
                                foodSeen[2] += 1;
                            foodSeen[i] += 1;
                        } else if (entity is Creature) {
                            if (i == 0)
                                leftCreatures.Add(entity);
                            else if (leftCreatures.Contains(entity))
                                creaturesSeen[2] += 1;
                            creaturesSeen[i] += 1;
                        }
                    }
                }
            }

            // Check the simulation environment for water directly in front
            waterSeen[2] = sim.environment.CheckTile(position + (Forward * (radius + viewSize / 2))) ? 0 : 1;

            // Check the simulation environment for water directly beneath
            touchingWater = sim.environment.CheckTile(position) ? 0 : 1;

            // Adjust the values in the seen list, so that they are proportional to the total amount seen
            // For example if there is one food to the left and three foods to the right,
            // the left would be 0.25, and the right would be 0.75

            float totalFoodSeen = foodSeen[0] + foodSeen[1] - foodSeen[2];
            if (totalFoodSeen > 0)
                for (int i = 0; i < foodSeen.Length; i++)
                    foodSeen[i] = foodSeen[i] / totalFoodSeen;

            float totalCreaturesSeen = creaturesSeen[0] + creaturesSeen[1] - creaturesSeen[2];
            if (totalCreaturesSeen > 0)
                for (int i = 0; i < creaturesSeen.Length; i++)
                    creaturesSeen[i] = creaturesSeen[i] / totalCreaturesSeen;
        }

        public override void HandleInput() {
            // Target this creature in the simulation if clicked
            if (Input.Click() && rect.Contains(Input.MouseWorldPos))
                Target();
        }

        public void Target() {
            sim.targeted = this;
        }

        public override void Collides(Entity entity) {
            if (entity is Food) {
                // If the entity is touching food, use outputs and energy to decide if it eats
                touchingFood = 1;
                if (toEat > 0 && energy < sim.settings.maxEnergy) {
                    // Add energy to the creature and remove the food
                    energy += ((Food)entity).energy;
                    sim.foods.Remove(entity);
                    entity.Remove();
                }
            } else if (entity is Creature) {
                // If the entity is touching a creature, use outputs and energy to decide if it reproduces
                touchingCreature = 1;
                Creature creature = (Creature)entity;
                // Only reproduces if both creatures' toMate outputs are of a suitable level,
                // and this creature's reproduction timer is ready, and both have enough energy
                if (toMate > 0 && scale >= 0.5f && reproductionTimer < 0 && creature.toMate > -0.5 && creature.scale >= 0.5f
                    && energy > 9 * sim.settings.maxEnergy / 32 && creature.energy > 9 * sim.settings.maxEnergy / 32)
                    Reproduce(creature);
            }
        }

        public void Reproduce(Creature other) {
            // Adjust creatures stats
            childrenCount += 1;
            other.childrenCount += 1;

            // Resets the reproduction timer
            reproductionTimer = 8;

            // Takes appropriate amount of energy from both creature
            energy -= sim.settings.maxEnergy / 4;
            other.energy -= sim.settings.maxEnergy / 4;

            // Generates a new network, crossing over the two creatures' networks
            NeuralNet newNetwork = new NeuralNet(network, other.network, mutationRate);
            // Gets a new colour, halfway between the two colours
            Color newColor = Color.Lerp(color, other.color, 0.5f);
            int newGeneration = Math.Max(generation, other.generation) + 1;

            // Combines the two's names
            int firstEnd = Math.Max(0, Math.Min(name.Length, name.Length / 2 + Rand.Range(-1, 1)));
            string firstHalf = name.Substring(0, firstEnd);
            int secondStart = Math.Max(0, Math.Min(other.name.Length, other.name.Length / 2 + Rand.Range(-1, 1)));
            string secondHalf = other.name.Substring(secondStart, other.name.Length - secondStart);
            string newName = firstHalf + secondHalf;

            // Creates the child and adds it to simulation
            Creature newCreature = new Creature(position, newName, sim, newColor, newNetwork, newGeneration);
            sim.creatures.Add(newCreature);
            parent.AddComponent(newCreature);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            // Gets the position of each eye and draws

            Point leftEyePos = (position + (Forward * (rect.Width * 0.35f)) + (Right * (rect.Width * 0.3f))).ToPoint();
            Point rightEyePos = (position + (Forward * (rect.Width * 0.35f)) - (Right * (rect.Width * 0.3f))).ToPoint();
            Rectangle leftEyeRect = new Rectangle(leftEyePos.X - rect.Width / 6, leftEyePos.Y - rect.Height / 6, rect.Width / 3, rect.Height / 3);
            Rectangle rightEyeRect = new Rectangle(rightEyePos.X - rect.Width / 6, rightEyePos.Y - rect.Height / 6, rect.Width / 3, rect.Height / 3);

            spriteBatch.Draw(eyeTexture, leftEyeRect, Color.White);
            spriteBatch.Draw(eyeTexture, rightEyeRect, Color.White);
        }
    }
}
