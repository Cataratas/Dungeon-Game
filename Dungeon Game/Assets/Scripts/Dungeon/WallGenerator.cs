using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

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
            _walls = new HashSet<Wall>();
            findWallsInDirections(floor);
            foreach (var wall in _walls) {
                try {
                    tilemapGenerator.paintSingleWall(wall.pos, wall.type);
                }
                catch (ArgumentOutOfRangeException) {
                }
            }
        }

        private static void findWallsInDirections(HashSet<Vector2Int> floor) {
            foreach (var tile in floor) {
                foreach (var direction in Direction.Neighbors) {
                    checkWallType(floor, tile + direction);
                }
            }
        }

        private static void checkWallType(ICollection<Vector2Int> floor, Vector2Int tile) {
            var neighborsString = new StringBuilder("0000000000000000000000000") {
                [0] = floor.Contains(tile)? '1' : '0'
            };
            var i = 1;
            foreach (var direction in Direction.Neighbors) {
                if (floor.Contains(tile + direction))
                    neighborsString[i] = '1';
                i++;
            }
            var neighbors = Convert.ToInt32(neighborsString.ToString(), 2);
            switch (neighbors) {
                case 0b0000111000000011111111100 or 0b0110000001111000000000011 or 0b0110000011111000000000011 or 0b0110000011111100000000111 or 0b0110000011100000000111111 or 0b0110000011111111000000001 or 0b0000111000011111111111110 or 0b0000111001110011111111111 or 0b0000111001111111111110011 or 0b0000111001110011111110001 or 0b0000111001110011111110000 or 0b0000111000110011111110000 or 0b0000111000010011111110000 or 0b0000111000001111111000000 or 0b0000011000000000011111110 or 0b0000110000011111110000000 or 0b0100000011000000000111111 or 0b0000011000000001111110000 or 0b0000110000001111110000000 or 0b0000011000000000011110000 or 0b0000110000000011110000000 or 0b0110000001111000000000000 or 0b0100000011000000000000111 or 0b0110000011110000000000111 or 0b0110000011111000000001111 or 0b0110000011111111000000111 or 0b0110000011111110000000111 or 0b0110000011111000000011111 or 0b0110000011111000000111111 or 0b0000111000000011111100000 or 0b0110000011111000000000001 or 0b0110000011100000000000111 or 0b0110000011111000000000111 or 0b0000111000000011111110000 or 0b0000111000000000111110000 or 0b0000111000000011111000000 or 0b0000111000000011111111110 or 0b0000111000001111111110000 or 0b0000111000011111111110000 or 0b0000111000000011111111000 or 0b0000111000000001111110000 or 0b0000111000000000111111110 or 0b0000111000011111111000000:
                    _walls.Add(new Wall(tile, 0)); // 9
                    break;
                case 0b1111000111111111000001111 or 0b0000000000000001111100000 or 0b1111000111111100000111111 or 0b1111000001111100000000000 or 0b1111000111111100000000001 or 0b1100000111000000000001111 or 0b1111000111100000000001111 or 0b1111000111111100000001111 or 0b0000000000001111111111110 or 0b1111000011111100000000111 or 0b1111000111111100000000111:
                    _walls.Add(new Wall(tile, 1)); // 1
                   break;
            }
        }
    }
}
