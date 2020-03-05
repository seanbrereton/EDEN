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
            dynamic = true;

            texture = Textures.Circle(Color.White, radius, 4);
            eyeTexture = Textures.Circle(Color.Black, radius / 3, radius / 6, Color.White);

            for (int i = 0; i < visionRects.Length; i++)
                visionRects[i] = new Rectangle(Point.Zero, new Point(viewSize));
        }

        public override void Update(float deltaTime) {
            if (perceiveTimer <= 0f) {
                Perceive();
                perceiveTimer = 0.05f;
            }

            Think();
            Act(deltaTime);
            // WrapInBounds(Global.worldSize);
            UseEnergy(deltaTime);

            perceiveTimer -= deltaTime;
            reproductionTimer -= deltaTime;
            age += deltaTime;

            if (scale < 0.5f) {
                scale += 0.05f * deltaTime;
                energy -= deltaTime;
            }
        }

        void Act(float deltaTime) {
            position += Forward * movement * 200 * deltaTime;
            rotation += turning * 240 * deltaTime;
        }

        void UseEnergy(float deltaTime) {
            energy -= deltaTime * (1 + Math.Abs(movement) * 2) * (Math.Max(1, touchingWater * 4));

            if (energy <= 0)
                Die();
        }

        void Die() {
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

            visionRects[0].Location = (position - (Right * (radius + viewSize / 6)) + (Forward * viewSize / 2)).ToPoint();
            visionRects[1].Location = (position + (Right * (radius + viewSize / 6)) + (Forward * viewSize / 2)).ToPoint();

            List<Entity> leftFoods = new List<Entity>();
            List<Entity> leftCreatures = new List<Entity>();

            for (int i = 0; i < visionRects.Length; i++) {
                visionRects[i].Offset(-viewSize / 2, -viewSize / 2);

                waterSeen[i] = ((Simulation)parent).environment.CheckTile(visionRects[i].Center.ToVector2()) ? 0 : 1;

                List<Entity> seen = Application.activeState.quadTree.Query(visionRects[i]);
                foreach (Entity entity in seen) {
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

            waterSeen[2] = ((Simulation)parent).environment.CheckTile(position + (Forward * (radius + viewSize))) ? 0 : 1;

            touchingWater = ((Simulation)parent).environment.CheckTile(position) ? 0 : 1;

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
            if (Input.Click() && rect.Contains(Input.MouseWorldPos))
                Target();
        }

        public void Target() {
            sim.targeted = this;
        }

        public override void Collides(Entity entity) {
            if (entity is Food) {
                touchingFood = 1;
                if (toEat > 0 && energy < sim.settings.maxEnergy) {
                    energy += ((Food)entity).energy;
                    sim.foods.Remove(entity);
                    entity.Remove();
                }
            } else if (entity is Creature) {
                touchingCreature = 1;
                Creature creature = (Creature)entity;
                if (toMate > 0 && scale >= 0.5f && reproductionTimer < 0 && creature.toMate > -0.5 && creature.scale >= 0.5f
                    && energy > 9 * sim.settings.maxEnergy / 32 && creature.energy > 9 * sim.settings.maxEnergy / 32)
                    Reproduce(creature);
            }
        }

        public void Reproduce(Creature other) {
            childrenCount += 1;
            other.childrenCount += 1;

            reproductionTimer = 8;
            other.reproductionTimer = 8;

            energy -= sim.settings.maxEnergy / 4;
            other.energy -= sim.settings.maxEnergy / 4;

            NeuralNet newNetwork = new NeuralNet(network, other.network, mutationRate);
            Color newColor = Color.Lerp(color, other.color, 0.5f);
            int newGeneration = Math.Max(generation, other.generation) + 1;

            int firstEnd = Math.Max(0, Math.Min(name.Length, name.Length / 2 + Rand.Range(-1, 1)));
            string firstHalf = name.Substring(0, firstEnd);
            int secondStart = Math.Max(0, Math.Min(other.name.Length, other.name.Length / 2 + Rand.Range(-1, 1)));
            string secondHalf = other.name.Substring(secondStart, other.name.Length - secondStart);
            string newName = firstHalf + secondHalf;

            Creature newCreature = new Creature(position, newName, sim, newColor, newNetwork, newGeneration);
            sim.creatures.Add(newCreature);
            parent.AddComponent(newCreature);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            Point leftEyePos = (position + (Forward * (rect.Width * 0.35f)) + (Right * (rect.Width * 0.3f))).ToPoint();
            Point rightEyePos = (position + (Forward * (rect.Width * 0.35f)) - (Right * (rect.Width * 0.3f))).ToPoint();
            Rectangle leftEyeRect = new Rectangle(leftEyePos.X - rect.Width / 6, leftEyePos.Y - rect.Height / 6, rect.Width / 3, rect.Height / 3);
            Rectangle rightEyeRect = new Rectangle(rightEyePos.X - rect.Width / 6, rightEyePos.Y - rect.Height / 6, rect.Width / 3, rect.Height / 3);

            spriteBatch.Draw(eyeTexture, leftEyeRect, Color.White);
            spriteBatch.Draw(eyeTexture, rightEyeRect, Color.White);
        }
    }
}
