using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Dungeon {
    public static class WallGenerator {
        private static HashSet<Wall> _walls;
        private struct Wall {
            public readonly Vector2Int pos;
            public readonly int type;

            public Wall(Vector2Int position, int wallType) {
                pos = position;
                type = wallType;
            }
        }

        public static void generateWalls(HashSet<Vector2Int> floor, TilemapGenerator tilemapGenerator) {
            var sw = Stopwatch.StartNew();
            try {
                _walls = new HashSet<Wall>();
                findWallsInDirections(floor);
                foreach (var wall in _walls) {
                    try {
                        tilemapGenerator.paintSingleWall(wall.pos, wall.type);
                    }
                    catch (ArgumentOutOfRangeException) {
                    }
                }
            } finally {
                sw.Stop();
                Debug.Log(sw.ElapsedMilliseconds);
            }
        }

        private static void findWallsInDirections(HashSet<Vector2Int> floor) {
            foreach (var tile in floor) {
                var d = 0;
                foreach (var dir in Direction.Neighbors) {
                    if (d > 7) break;
                    checkWallType(floor, tile + dir);
                    d++;
                }
            }
        }
        
        private static void checkWallType(ICollection<Vector2Int> floor, Vector2Int tile) {
            uint neighbors = 0;
            if (floor.Contains(tile)) neighbors |= 1u << 24;
            
            var i = 1;
            foreach (var direction in Direction.Neighbors) {
                if (floor.Contains(tile + direction))
                    neighbors |= 1u << 24 - i;
                i++;
            }
            
            var binaryNeighbors = 0;
            uint mask = 1;
            while (neighbors > 0) {
                if ((neighbors & 1) == 1)
                    binaryNeighbors |= (int) mask;
                neighbors >>= 1;
                mask <<= 1;
            }
            
            var binaryNeighbors9 = binaryNeighbors >> 16;
            switch (binaryNeighbors9) {
                case 0b011000001 or 0b000011100 or 0b000011000 or 0b011000000 or 0b010000001 or 0b000001100:
                    _walls.Add(new Wall(tile, 0)); // 9
                    break;
                case 0b000000111 or 0b000000110 or 0b000000011 or 0b000000100:
                    _walls.Add(new Wall(tile, 15)); // 182
                    break;
                case 0b001110000 or 0b000110000 or 0b001100000 or 0b000010000:
                    _walls.Add(new Wall(tile, 14)); // 181
                    break;
                case 0b001111100 or 0b000111100 or 0b001111000:
                    _walls.Add(new Wall(tile, 6)); // 230
                    break;
                case 0b000001111 or 0b000011111 or 0b000011110:
                    _walls.Add(new Wall(tile, 8)); // 229
                    break;
                case 0b001000000:
                    _walls.Add(new Wall(tile, 16)); // 201
                    break;
                case 0b000000001:
                    _walls.Add(new Wall(tile, 17)); // 202
                    break;
            }
            
            switch (binaryNeighbors) {
                //case 0b0000011000000000111110000 or 0b0000111000000111111110000 or 0b0000110000000111110000000 or 0b0000110000000011111100000 or 0b0000111000000011111111100 or 0b0110000001111000000000011 or 0b0110000011111000000000011 or 0b0110000011111100000000111 or 0b0110000011100000000111111 or 0b0110000011111111000000001 or 0b0000111000011111111111110 or 0b0000111001110011111111111 or 0b0000111001111111111110011 or 0b0000111001110011111110001 or 0b0000111001110011111110000 or 0b0000111000110011111110000 or 0b0000111000010011111110000 or 0b0000111000001111111000000 or 0b0000011000000000011111110 or 0b0000110000011111110000000 or 0b0100000011000000000111111 or 0b0000011000000001111110000 or 0b0000110000001111110000000 or 0b0000011000000000011110000 or 0b0000110000000011110000000 or 0b0110000001111000000000000 or 0b0100000011000000000000111 or 0b0110000011110000000000111 or 0b0110000011111000000001111 or 0b0110000011111111000000111 or 0b0110000011111110000000111 or 0b0110000011111000000011111 or 0b0110000011111000000111111 or 0b0000111000000011111100000 or 0b0110000011111000000000001 or 0b0110000011100000000000111 or 0b0110000011111000000000111 or 0b0000111000000011111110000 or 0b0000111000000000111110000 or 0b0000111000000011111000000 or 0b0000111000000011111111110 or 0b0000111000001111111110000 or 0b0000111000011111111110000 or 0b0000111000000011111111000 or 0b0000111000000001111110000 or 0b0000111000000000111111110 or 0b0000111000011111111000000:
                //    _walls.Add(new Wall(tile, 0)); // 9
                //    break;
                case 0b0000000000000011110000000 or 0b0000000000000011111100000 or 0b0000000000011111110000000 or 0b1111000111111111000000011 or 0b0000000000001111111100000 or 0b0000000000011111111000000 or 0b0000000000000000111111110 or 0b0000000000000111111100000 or 0b1100000111000000000111111 or 0b1111000111111110000001111 or 0b0000000000000111110000000 or 0b1111000111111111000000001 or 0b0000000000000000011111110 or 0b0000000000011001111100000 or 0b0000000000011111111100000 or 0b0000000000000001111111000 or 0b0000000000000001111111100 or 0b1111000001111100000000011 or 0b0000000000000001110000000 or 0b0000000000000001111110000 or 0b0000000000000001111000000 or 0b1111000111111100000000011 or 0b0000000000011111111111110 or 0b0000000000000000011100000 or 0b0000000000000000111100000 or 0b0000000000000001111111110 or 0b1111000111100000000111111 or 0b1111000111111111000001111 or 0b0000000000000001111100000 or 0b1111000111111100000111111 or 0b1111000001111100000000000 or 0b1111000111111100000000001 or 0b1100000111000000000001111 or 0b1111000111100000000001111 or 0b1111000111111100000001111 or 0b0000000000001111111111110 or 0b1111000011111100000000111 or 0b1111000111111100000000111:
                    _walls.Add(new Wall(tile, 1)); // 1
                   break;
                //case 0b0011100001111111100000000 or 0b0011000000111111000000000 or 0b0011100000111111000000000 or 0b0011000001111100000000011 or 0b0000100000011111100000000 or 0b0001100000011111100000000 or 0b0011100000011111100000000 or 0b0000100000000011100000000 or 0b0001100000000111100000000 or 0b0011100000001111100000000 or 0b0011000000111100000000000 or 0b0011100000111111100000000 or 0b0011100001111111100000011 or 0b0011100000111110000000000:
                //    _walls.Add(new Wall(tile, 14)); // 181
                //    break;
                //case 0b0000001111110000000011111 or 0b0000000110000000000111111 or 0b0000001110000000000111111 or 0b0000001000000000001111110 or 0b0000001100000000001111110 or 0b0000001110000000001111110 or 0b0000000111110000000111111 or 0b0000001111000000001111111 or 0b0000001111100000001111111 or 0b0000001111110000001111111 or 0b0000000110000000000001111 or 0b0000001110000000000011111 or 0b0000001110000000001111111 or 0b0000001110000000001111100 or 0b0000001000000000001110000 or 0b0000001100000000001111000:
                //    _walls.Add(new Wall(tile, 15)); // 182
                //    break;
                //case 0b0010000000111111000000000 or 0b0010000000111000000000000 or 0b0010000001111000000000011:
                //    _walls.Add(new Wall(tile, 16)); // 201
                //    break;
                //case 0b0000000010000000000000111 or 0b0000000010000000000111111:
                //    _walls.Add(new Wall(tile, 17)); // 202
                //    break;
                case 0b0000001110000000011111111 or 0b0000001000000001111110000 or 0b0000001110000001111111111 or 0b0000001100000001111111000 or 0b0000001110000001111111100 or 0b0000001110000000111111111:
                    _walls.Add(new Wall(tile, 9)); // 203
                    break;
                case 0b0000000000000001100000000 or 0b0000000000011111100000000:
                    _walls.Add(new Wall(tile, 18)); // 154
                    break;
                case 0b0000000000000000001100000 or 0b0000000000000000001111110:
                    _walls.Add(new Wall(tile, 19)); // 155
                    break;
                case 0b0000100000000011111100000 or 0b0011100000111111111000000 or 0b0000100000000011111000000 or 0b0001100000000111111100000 or 0b0011100000111111111100000 or 0b0001100000000111111000000 or 0b0011100000001111111100000 or 0b0011100000111111110000000:
                    _walls.Add(new Wall(tile, 7)); // 204
                    break;
                //case 0b0000111111110011111111111 or 0b0000111110000011111111111 or 0b0000111110000011111111100 or 0b0000111110000000111111111 or 0b0000111100000011111111000:
                //    _walls.Add(new Wall(tile, 8)); // 229
                //    break;
                //case 0b0011111000111111111000000 or 0b0001111000000111111110000 or 0b0011111001111111111110011 or 0b0011111000111111111110000 or 0b0011111000001111111000000 or 0b0011110000111111110000000:
                //    _walls.Add(new Wall(tile, 6)); // 230
                //    break;
                case 0b1100001111000000001111111 or 0b1111001111111100001111111 or 0b1100001111110000001111111 or 0b1111001111100000001111111:
                    _walls.Add(new Wall(tile, 43)); // 156
                    break;
                case 0b0110001111111000001111111 or 0b0110001111111000000111111 or 0b0100001111000000001111111 or 0b0110001111100000001111111:
                    _walls.Add(new Wall(tile, 12)); // 183
                    break;
                case 0b0111100001111111100000000 or 0b0111100011111111100000111 or 0b0111000011111100000000111 or 0b0111100011111110000000111:
                    _walls.Add(new Wall(tile, 10)); // 184
                    break;
                case 0b1111100001111111100000000 or 0b1111100111111111100001111 or 0b1111100111111110000001111:
                    _walls.Add(new Wall(tile, 42)); // 157
                    break;
                case 0b0111100011111111111100111:
                    _walls.Add(new Wall(tile, 28)); // 518
                    break;
                case 0b0110001111111001111111111:
                    _walls.Add(new Wall(tile, 34)); // 519
                    break;
                case 0b0110000011111001111111111 or 0b0110000011111111111100111 or 0b0110000011111001111100001 or 0b0110000001111001111100000:
                    _walls.Add(new Wall(tile, 26)); // 520
                    break;
                case 0b0010000000111001111100000:
                    _walls.Add(new Wall(tile, 32)); // 527
                    break;
            }
        }
    }
}
