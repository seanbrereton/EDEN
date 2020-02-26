using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDEN {
    public class QuadTree {

        int maxEntities = 2;
        int maxLevels = 8;

        List<Entity> entities = new List<Entity>();
        Rectangle bounds;
        QuadTree[] branches = new QuadTree[4];
        int level;
        
        Texture2D texture;


        public QuadTree(Rectangle _bounds, int _level = 0) {
            bounds = _bounds;
            level = _level;
            texture = Application.branchTextures[level];

        }

        public void CheckCollisions() {
            foreach (Entity entity in entities) {
                if (entity.dynamic) {
                    List<Entity> near = quadTree.Query(entity.rect);
                    foreach (Entity other in near) {
                        if (entity.rect.Intersects(other.rect) && !entity.Equals(other))
                            entity.Collides(other);
                    }
                }
            }
        }

        public void Update(List<Component> components) {
            Clear();

            foreach (Component component in components) {
                if (component is Entity)
                    Insert(component);
            }
        }

        public void Clear() {
            entities.Clear();

            for (int i = 0; i < branches.Length; i++) {
                if (branches[i] != null) {
                    branches[i].Clear();
                    branches[i] = null;
                }
            }
        }

        void Subdivide() {
            int width = bounds.Width / 2;
            int height = bounds.Height / 2;
            int x = bounds.X;
            int y = bounds.Y;

            branches[0] = new QuadTree(new Rectangle(x, y, width, height), level + 1);
            branches[1] = new QuadTree(new Rectangle(x+width, y, width, height), level + 1);
            branches[2] = new QuadTree(new Rectangle(x+width, y+height, width, height), level + 1);
            branches[3] = new QuadTree(new Rectangle(x, y+height, width, height), level + 1);

            List<Entity> oldEntities = new List<Entity>(entities);
            entities.Clear();
            foreach (Entity entity in oldEntities)
                Insert(entity);
        }

        public bool Insert(Entity entity) {
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
            spriteBatch.Draw(texture, bounds, Color.White);
            if (branches[0] != null) {
                foreach (QuadTree branch in branches) {
                    branch.Draw(spriteBatch);
                }
            }

        }
    }
}
