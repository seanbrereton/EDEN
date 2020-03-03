using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDEN {
    
    public class Environment : Entity {

        Point size;
        int gridSize;
        float waterRatio;
        public bool[,] tiles;
        public bool[,] baseTiles;
        int smoothLevel;

        public Environment(Vector2 _position, Point _size, int _gridSize, float _waterRatio, int _smoothLevel) : base(_position) {
            size = _size;
            gridSize = _gridSize;
            waterRatio = _waterRatio;
            smoothLevel = _smoothLevel;

            color = Color.DarkOliveGreen;
            texture = Textures.Empty(size.X, size.Y);

            Generate();
        }

        public bool CheckTile(Vector2 position) {
            int gridX = (int)(position.X / gridSize);
            int gridY = (int)(position.Y / gridSize);

            if (InRange(gridX, gridY))
                return tiles[gridX, gridY];
            else
                return false;
        }

        public void Generate() {
            baseTiles = new bool[size.X / gridSize, size.Y / gridSize];
            for (int x = 0; x < baseTiles.GetLength(0); x++)
                for (int y = 0; y < baseTiles.GetLength(1); y++)
                    baseTiles[x, y] = Rand.Range(1f) > waterRatio;

            Smooth(smoothLevel);
        }

        public void GenerateTexture() { 
            Color[] colors = new Color[size.X * size.Y];

            for (int x = 0; x < size.X; x++)
                for (int y = 0; y < size.X; y++) {
                    int i = y * size.X + x;
                    if (tiles[x / gridSize, y / gridSize])
                        colors[i] = Color.White;
                }

            texture.SetData<Color>(colors);
        }

        int GetNeighbourCount(int tileX, int tileY) {
            int count = 0;

            for (int xOffset = -1; xOffset < 2; xOffset++)
                for (int yOffset = -1; yOffset < 2; yOffset++) {
                    int x = tileX + xOffset;
                    int y = tileY + yOffset;
                    if (InRange(x, y))
                        if (tiles[x, y] && (xOffset != 0 || yOffset != 0))
                            count++;
                }

            return count;
        }

        bool InRange(int x, int y) {
            return x >= 0 && x < tiles.GetLength(0) && y >= 0 && y < tiles.GetLength(1);
        }

        void SmoothTiles(int times=1) {
            for (int i = 0; i < times; i++) {
                bool[,] newTiles = new bool[tiles.GetLength(0), tiles.GetLength(1)];

                for (int x = 0; x < tiles.GetLength(0); x++)
                    for (int y = 0; y < tiles.GetLength(1); y++)
                        newTiles[x, y] = GetNeighbourCount(x, y) >= 4;

                tiles = newTiles;
            }
        }

        void Smooth(int times) {
            smoothLevel = times;
            tiles = baseTiles;
            SmoothTiles(times);
            GenerateTexture();
        }

    }
}
