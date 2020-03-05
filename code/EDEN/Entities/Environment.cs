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
            // Converts a vector2 to indexes for the tiles array

            int gridX = (int)(position.X / gridSize);
            int gridY = (int)(position.Y / gridSize);

            if (InRange(gridX, gridY))
                return tiles[gridX, gridY];
            else
                return false;
        }

        public void Generate() {
            // Generates a random 2D array of bools, based on the waterRatio
            // A true tile represents land

            tiles = new bool[size.X / gridSize, size.Y / gridSize];
            for (int x = 0; x < tiles.GetLength(0); x++)
                for (int y = 0; y < tiles.GetLength(1); y++)
                    tiles[x, y] = Rand.Range(1f) > waterRatio;

            SmoothTiles(smoothLevel);
            GenerateTexture();
        }

        public void GenerateTexture() {
            // Uses the grid size and tiles array to generate a texture

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
            // Counts how many of the 8 tiles surrounding the given tile coordinates are true

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
            // Checks that the given x y values are within the range of the 2D tiles array
            return x >= 0 && x < tiles.GetLength(0) && y >= 0 && y < tiles.GetLength(1);
        }

        void SmoothTiles(int times=1) {
            // Smooths out the tiles by making it so that if a tile has four or more true neighbours,
            // it is true, otherwise it is false
            // When this is repeated a number of times, it makes smoother shapes, resembling islands

            for (int i = 0; i < times; i++) {
                bool[,] newTiles = new bool[tiles.GetLength(0), tiles.GetLength(1)];

                for (int x = 0; x < tiles.GetLength(0); x++)
                    for (int y = 0; y < tiles.GetLength(1); y++)
                        newTiles[x, y] = GetNeighbourCount(x, y) >= 4;

                tiles = newTiles;
            }
        }

    }
}
