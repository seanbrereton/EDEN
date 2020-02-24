using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace EDEN {
    public class Creature : Entity {

        public NeuralNet network;

        public float scale;

        // Network Inputs
        int[] seen = new int[3];

        // Network Outputs
        float rotationVelocity;
        float movementVelocity;

        Texture2D eyeTexture;
        Rectangle leftEyeRect;
        Rectangle rightEyeRect;
                    
        public float energy = 24;
        public float maxEnergy = 48;
        int radius = 8;

        Rectangle visionRect;
        int viewSize = 32;

        public Creature(Vector2 _position) : base(_position) {
            dynamic = true;

            // Creates a random neural network, using the applications layer parameters
            network = new NeuralNet(Global.layers);

            // Random rotation in degrees
            rotation = Rand.Range(360);

            // Creates a circle texture using the colour and radius generated
            Color color = Rand.RandColor();
            texture = Textures.Circle(color, radius, 4);
            eyeTexture = Textures.Circle(Color.Black, 2 * radius, radius, Color.White);

            scale = 0.2f;

            visionRect = new Rectangle(Point.Zero, new Point(viewSize));
        }

        public override void Update(GameTime gameTime) {
            // Gets inputs, puts them through neural net, sets and uses outputs
            Perceive();
            Think();
            Act();

            if (scale < 1)
                scale += 0.002f;

            energy -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (energy <= 0)
                Die();
        }

        void Perceive() {
            visionRect.Location = (position + Forward * viewSize).ToPoint();
            visionRect.Offset(-viewSize / 2, -viewSize / 2);
            List<Entity> seen = Application.quadTree.Query(visionRect);
            foreach (Entity entity in seen)
                if (entity is Food && visionRect.Intersects(entity.rect))
                    entity.delete = false;

            visionRect.Location = (position + Sideways * viewSize).ToPoint();
            visionRect.Offset(-viewSize / 2, -viewSize / 2);
            seen = Application.quadTree.Query(visionRect);
            foreach (Entity entity in seen)
                if (entity is Food && visionRect.Intersects(entity.rect))
                    entity.delete = false;

            visionRect.Location = (position - Sideways * viewSize).ToPoint();
            visionRect.Offset(-viewSize / 2, -viewSize / 2);
            seen = Application.quadTree.Query(visionRect);
            foreach (Entity entity in seen)
                if (entity is Food && visionRect.Intersects(entity.rect))
                    entity.delete = false;
        }

        void Die() {
            delete = true;
        }

        public override void Draw(SpriteBatch spriteBatch) {
            // This is all bad and complex and temporary.
            // TODO: Generate the original texture with the eyes, then just draw the texture at the correct angle.

            Point leftEyePos = (position + (Forward * (rect.Width * 0.35f)) + (Sideways * (rect.Width * 0.3f))).ToPoint();
            Point rightEyePos = (position + (Forward * (rect.Width * 0.35f)) - (Sideways * (rect.Width * 0.3f))).ToPoint();
            leftEyeRect = new Rectangle(leftEyePos.X - rect.Width / 6, leftEyePos.Y - rect.Height / 6, rect.Width / 3, rect.Height / 3);
            rightEyeRect = new Rectangle(rightEyePos.X - rect.Width / 6, rightEyePos.Y - rect.Height / 6, rect.Width / 3, rect.Height / 3);

            spriteBatch.Draw(eyeTexture, leftEyeRect, Color.White);
            spriteBatch.Draw(eyeTexture, rightEyeRect, Color.White);
        }

        // (TEMP)
        void KeepOnScreen() {
            // Checks if outside any bounds of the screen
            // Changes position to keep it in the bounds
            Rectangle screen = new Rectangle(0, 0, Application.graphics.PreferredBackBufferWidth, Application.graphics.PreferredBackBufferHeight);
            if (position.X > screen.Width)
                position.X = 0;
            if (position.X < 0)
                position.X = screen.Width;
            if (position.Y > screen.Height)
                position.Y = 0;
            if (position.Y < 0)
                position.Y = screen.Height;
        }

        void Think() {
            float[] inputs = GetInputs();
            float[] outputs = network.FeedForward(inputs);

            // Sets the variables to the outputs from the network
            movementVelocity = outputs[0];
            rotationVelocity = outputs[1];
        }

        void Act() {
            position += Forward * movementVelocity * 10;
            rotation += rotationVelocity * 8;
        }

        float[] GetInputs() {
            return new float[] { 
                1,
                movementVelocity,
                rotationVelocity,
                // Normalizes the rotation to be in the range [-1, 1]
                ((rotation % 360) / 180f) - 1,
                energy / maxEnergy
            };
        }

        public override Rectangle GetRect() {
            int size = (int)(texture.Width * scale);
            Point newPos = new Point((int)position.X - size / 2, (int)position.Y - size / 2);
            Point sizePoint = new Point(size, size);
            return new Rectangle(newPos, sizePoint);
        }

        public override void Collides(Entity other) {
            if (other is Food) {
                energy += 1;
                other.position = Rand.Range(Global.worldSize.ToVector2());
            }
        }
    }
}
