using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace EDEN {
    public class Creature : Entity {

        public NeuralNet network;

        // Network Inputs
        float[] foodSeen = new float[3];
        float[] creaturesSeen = new float[3];
        float touchingFood;
        float touchingCreature;

        // Network Outputs
        float turning;
        float movement;
        float toEat;
        public float toMate;

        float reproductionTimer;
        public float energy;
        public float maxEnergy = 96;
        int viewSize = 32;
        Rectangle[] visionRects = new Rectangle[3];

        int radius = 8;
        Texture2D eyeTexture;
        Rectangle leftEyeRect;
        Rectangle rightEyeRect;
                    
        public Creature(Vector2 _position) : base(_position) {
            color = Rand.RandColor();
            network = new NeuralNet(Global.layers);
            Initialize();
        }

        public Creature(Vector2 _position, Color _color, NeuralNet _network) : base(_position) {
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

        public override void Update(float deltaTime) {
            Perceive();
            Think();
            Act(deltaTime);
            KeepInWorld();
            UseEnergy(deltaTime);

            reproductionTimer -= deltaTime;

            if (scale < 1)
                scale += 0.1f * deltaTime;
        }

        void KeepInWorld() {
            if (position.X > Global.worldSize.X)
                position.X = 0;
            if (position.X < 0)
                position.X = Global.worldSize.X;
            if (position.Y > Global.worldSize.Y)
                position.Y = 0;
            if (position.Y < 0)
                position.Y = Global.worldSize.Y;
        }

        void Act(float deltaTime) {
            position += Forward * movement * 200 * deltaTime;
            rotation += turning * 160 * deltaTime;
        }

        void UseEnergy(float deltaTime) {
            energy -= deltaTime * (1 + Math.Abs(movement));

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
                1,
                movement,
                turning,
                energy / maxEnergy,
                foodSeen[0],
                foodSeen[1],
                foodSeen[2],
                creaturesSeen[0],
                creaturesSeen[1],
                creaturesSeen[2],
                touchingFood,
                touchingCreature
            };
        }

        void Perceive() {
            foodSeen = new float[3];
            creaturesSeen = new float[3];
            int totalFoodSeen = 0;
            int totalCreaturesSeen = 0;

            visionRects[0].Location = (position + Forward * viewSize).ToPoint();
            visionRects[1].Location = (position + Right * viewSize).ToPoint();
            visionRects[2].Location = (position - Right * viewSize).ToPoint();

            for (int i = 0; i < visionRects.Length; i++) {
                visionRects[i].Offset(-viewSize / 2, -viewSize / 2);
                List<Entity> seen = Application.activeState.quadTree.Query(visionRects[i]);
                foreach (Entity entity in seen) {
                    if (visionRects[i].Intersects(entity.rect)) {
                        if (entity is Food) {
                            foodSeen[i] += 1;
                            totalFoodSeen += 1;
                        } else if (entity is Creature) {
                            creaturesSeen[i] += 1;
                            totalCreaturesSeen += 1;
                        }
                    }
                }
            }

            if (totalFoodSeen > 0)
                for (int i = 0; i < foodSeen.Length; i++)
                    foodSeen[i] = foodSeen[i] / totalFoodSeen;

            if (totalCreaturesSeen > 0)
                for (int i = 0; i < creaturesSeen.Length; i++)
                    creaturesSeen[i] = creaturesSeen[i] / totalCreaturesSeen;
        }

        public override void HandleInput() {
            if (Input.Press(Microsoft.Xna.Framework.Input.Keys.N))
                parent.AddComponent(new Creature(position));
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
            reproductionTimer = 16;
            other.reproductionTimer = 16;

            energy -= 16;
            other.energy -= 16;

            NeuralNet newNetwork = new NeuralNet(network, other.network);
            Color newColor = Color.Lerp(color, other.color, 0.5f);

            Creature newCreature = new Creature(position, newColor, newNetwork);
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
        }
    }
}
