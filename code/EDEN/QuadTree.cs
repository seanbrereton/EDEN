using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace EDEN {
    
    public class QuadTree {

        int maxEntities = 4;
        int maxLevels = 10;

        List<Entity> entities = new List<Entity>();
        Rectangle bounds;
        QuadTree[] branches = new QuadTree[4];
        int level;

        Texture2D[] branchTextures;
        Texture2D texture;
        
        public QuadTree(Rectangle _bounds, int _level, Texture2D[] _branchTextures) {
            bounds = _bounds;
            level = _level;
            branchTextures = _branchTextures;
            texture = branchTextures[level];
        }

        public void CheckCollisions(List<Entity> toCheck) {
            // Goes through list of entities to check, Queries the quad tree to find entities that may be near,
            // and calls the Collides method on any entities that have intersecting rects
            foreach (Entity entity in toCheck) {
                if (entity.dynamic) {
                    List<Entity> near = Query(entity.rect);
                    foreach (Entity other in near) {
                        if (entity.rect.Intersects(other.rect) && !entity.Equals(other))
                            entity.Collides(other);
                    }
                }
            }
        }

        public List<Entity> UpdateEntities(List<Component> components) {
            Clear();
            List<Entity> allEntities = new List<Entity>();

            // Inserts all components that are entities into the quad tree

            foreach (Component component in components) {
                if (component is Entity) {
                    Insert((Entity)component);
                    allEntities.Add((Entity)component);
                }
            }

            return allEntities;
        }

        public void Clear() {
            // Recursively clears this quad tree, and all child quad trees
            entities.Clear();

            for (int i = 0; i < branches.Length; i++) {
                if (branches[i] != null) {
                    branches[i].Clear();
                    branches[i] = null;
                }
            }
        }

        void Subdivide() {
            // Create four new quad trees, half the size of this one, one for each corner

            int width = bounds.Width / 2;
            int height = bounds.Height / 2;
            int x = bounds.X;
            int y = bounds.Y;

            branches[0] = new QuadTree(new Rectangle(x, y, width, height), level + 1, branchTextures);
            branches[1] = new QuadTree(new Rectangle(x+width, y, width, height), level + 1, branchTextures);
            branches[2] = new QuadTree(new Rectangle(x+width, y+height, width, height), level + 1, branchTextures);
            branches[3] = new QuadTree(new Rectangle(x, y+height, width, height), level + 1, branchTextures);

            // Clear out the entities of this quad tree, and re-insert them,
            // so that they are put into the appropriate branches

            List<Entity> oldEntities = new List<Entity>(entities);
            entities.Clear();
            foreach (Entity entity in oldEntities)
                Insert(entity);
        }

        public bool Insert(Entity entity) {
            // If the entity is within the bounds of this quad tree, try adding it to it,
            // but if this quad tree already has the max amount of entities,
            // split it up into more quad trees, and try inserting it into those

            if (bounds.Contains(entity.rect)) {
                if (entities.Count < maxEntities || level == maxLevels) {
                    entities.Add(entity);
                    return true;
                } else {
                    if (branches[0] == null)
                        Subdivide();
                    foreach (QuadTree branch in branches) {
                        if (branch.Insert(entity))
                            return true;
                    } 
                    entities.Add(entity);
                    return true;
                }
            }
            return false;
        }

        public List<Entity> Query(Rectangle rect, List<Entity> found) {
            // Return the entities of all quad trees that have bounds that intersect with the rect given

            if (bounds.Intersects(rect)) {
                if (branches[0] != null) {
                    foreach (QuadTree branch in branches)
                        found.AddRange(branch.Query(rect));
                }
                found.AddRange(entities);
            }
            return found;
        }
        public List<Entity> Query(Rectangle rect){
            return Query(rect, new List<Entity>());
        }

        public void Draw(SpriteBatch spriteBatch) {
            // Draw the textures showing the bounds of each quad tree

            spriteBatch.Draw(texture, bounds, Color.White);
            if (branches[0] != null) {
                foreach (QuadTree branch in branches) {
                    branch.Draw(spriteBatch);
                }
            }
        }
    }
}
