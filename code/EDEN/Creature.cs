﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace EDEN {
    public class Creature : Entity {

        public NeuralNet network;

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

        public int childrenCount;
        public int generation;
        public float age;
        public float reproductionTimer;
        public float energy;
        public float maxEnergy = 96;
        int viewSize = 96;
        Rectangle[] visionRects = new Rectangle[2];

        int radius = 8;
        Texture2D eyeTexture;
        Rectangle leftEyeRect;
        Rectangle rightEyeRect;

        float perceiveTimer;
        bool allSeeing;

        Settings settings;
                    
        public Creature(Vector2 _position) : base(_position) {
            generation = 0;
            color = Rand.RandColor();
            network = new NeuralNet(Global.layers);
            Initialize();
        }

        public Creature(Vector2 _position, Color _color, NeuralNet _network, int _generation) : base(_position) {
            generation = _generation;
            color = _color;
            network = _network;
            Initialize();
        }

        public void Initialize() {
            dynamic = true;

            texture = Textures.Circle(Color.White, radius, 4);
            eyeTexture = Textures.Circle(Color.Black, radius, radius / 2, Color.White);

            scale = 0.2f;
            rotation = Rand.Range(360);
            energy = maxEnergy / 2;

            for (int i = 0; i < visionRects.Length; i++)
                visionRects[i] = new Rectangle(Point.Zero, new Point(viewSize));
        }

        public override void Start() {
            settings = ((Simulation)parent).settings;
        }

        public override void Update(float deltaTime) {
            if (perceiveTimer <= 0f || allSeeing) {
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

            if (scale < 1)
                scale += 0.1f * deltaTime;
        }

        void Act(float deltaTime) {
            position += Forward * movement * 200 * deltaTime;
            rotation += turning * 240 * deltaTime;
        }

        void UseEnergy(float deltaTime) {
            energy -= deltaTime * (1 + Math.Abs(movement) * 2 + touchingWater * 4);

            if (energy <= 0)
                Die();
        }

        void Die() {
            parent.AddComponent(new Food(position, color));
            Remove();
            ((Simulation)parent).creatures.Remove(this);
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
                energy / maxEnergy,
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
            if (Input.Press(Microsoft.Xna.Framework.Input.Keys.P)) {
                allSeeing = !allSeeing;
            }
        }

        public override void Collides(Entity entity) {
            if (entity is Food) {
                touchingFood = 1;
                if (toEat > 0 && energy <= maxEnergy - 1) {
                    energy += 8;
                    ((Simulation)parent).foods.Remove(entity);
                    entity.Remove();
                }
            } else if (entity is Creature) {
                touchingCreature = 1;
                Creature creature = (Creature)entity;
                if (toMate > 0 && scale >= 1 && reproductionTimer < 0 && creature.toMate > 0 && creature.scale >= 1 && creature.reproductionTimer < 0)
                    Reproduce(creature);
            }
        }

        public void Reproduce(Creature other) {
            childrenCount += 1;
            other.childrenCount += 1;

            reproductionTimer = 16;
            other.reproductionTimer = 16;

            energy -= maxEnergy / 3;
            other.energy -= maxEnergy / 3;

            NeuralNet newNetwork = new NeuralNet(network, other.network);
            Color newColor = Color.Lerp(color, other.color, 0.5f);
            int newGeneration = Math.Max(generation, other.generation) + 1;

            Creature newCreature = new Creature(position, newColor, newNetwork, newGeneration);
            ((Simulation)parent).creatures.Add(newCreature);
            parent.AddComponent(newCreature);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            Point leftEyePos = (position + (Forward * (rect.Width * 0.35f)) + (Right * (rect.Width * 0.3f))).ToPoint();
            Point rightEyePos = (position + (Forward * (rect.Width * 0.35f)) - (Right * (rect.Width * 0.3f))).ToPoint();
            leftEyeRect = new Rectangle(leftEyePos.X - rect.Width / 6, leftEyePos.Y - rect.Height / 6, rect.Width / 3, rect.Height / 3);
            rightEyeRect = new Rectangle(rightEyePos.X - rect.Width / 6, rightEyePos.Y - rect.Height / 6, rect.Width / 3, rect.Height / 3);

            spriteBatch.Draw(eyeTexture, leftEyeRect, Color.White);
            spriteBatch.Draw(eyeTexture, rightEyeRect, Color.White);
            /*spriteBatch.Draw(eyeTexture, visionRects[0], Color.White);
            spriteBatch.Draw(eyeTexture, visionRects[1], Color.White);
*/
        }
    }
}
