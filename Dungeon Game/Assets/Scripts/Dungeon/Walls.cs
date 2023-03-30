using System.Collections.Generic;
using UnityEngine;

namespace Dungeon {
    public static class Walls {
        private struct Wall {
            public readonly Vector2Int pos;
            public readonly int type;

            public Wall(Vector2Int position, int wallType)  {
                pos = position;
                type = wallType;
            }
        }

        public static void generate(HashSet<Vector2Int> floor, TilemapVisualizer tilemap) {
            var walls = findWallsInDirections(floor);
                foreach (var wall in walls) {
                    tilemap.paintSingleWall(wall.pos, wall.type);
                }
        }

        private static HashSet<Wall> findWallsInDirections(HashSet<Vector2Int> floor) {
            var walls = new HashSet<Wall>();
            foreach(var tile in floor) {
                var up = tile + Vector2Int.up;
                var down = tile + Vector2Int.down;
                var right = tile + Vector2Int.right;
                var left = tile + Vector2Int.left;
                var leftUp = left + Vector2Int.up;
                var leftDown = left + Vector2Int.down;
                var rightUp = right + Vector2Int.up;
                var rightDown = right + Vector2Int.down;
                
                if (!floor.Contains(down)) {
                    if (floor.Contains(leftDown) && floor.Contains(rightDown)) {
                        if (floor.Contains(down + Vector2Int.down))
                            walls.Add(new Wall(down, 20));
                        else if (floor.Contains(down + Vector2Int.down + Vector2Int.down))
                            walls.Add(new Wall(down, 45)); // 536
                        else
                            walls.Add(new Wall(down, 22));
                        walls.Add(new Wall(tile, 23));
                    } else if ((floor.Contains(rightDown) || floor.Contains(rightDown + Vector2Int.down)) && !floor.Contains(down + Vector2Int.down)) {
                        if (floor.Contains(down + Vector2Int.down + Vector2Int.down)) {
                            walls.Add(new Wall(down, 28));
                        } else if (floor.Contains(leftDown)) {
                            walls.Add(new Wall(down, 22));
                            walls.Add(new Wall(tile, 23));
                        }
                        else if (!floor.Contains(leftDown) && floor.Contains(leftDown + Vector2Int.down) && !floor.Contains(down + Vector2Int.down + Vector2Int.down) && floor.Contains(rightDown))
                            walls.Add(new Wall(down, 22)); // 515
                        else
                            walls.Add(new Wall(down, 10));
                        if (!floor.Contains(leftDown) && !floor.Contains(leftDown + Vector2Int.down))
                            walls.Add(new Wall(tile, 11));
                        else {
                            walls.Add(new Wall(tile, 23)); // 514
                        }
                    } else if ((floor.Contains(leftDown) || floor.Contains(leftDown + Vector2Int.down)) && !floor.Contains(down + Vector2Int.down)) {
                        if (floor.Contains(down + Vector2Int.down + Vector2Int.down)) {
                            walls.Add(new Wall(down, 34));
                        } else if (floor.Contains(down + Vector2Int.down + Vector2Int.down + Vector2Int.right) && !floor.Contains(down + Vector2Int.down + Vector2Int.right))
                            walls.Add(new Wall(down, 29)); // 525
                        else
                            walls.Add(new Wall(down, 12));
                        walls.Add(new Wall(tile, 13));
                    } else if (!floor.Contains(down + Vector2Int.down)) {
                        if (floor.Contains(down + Vector2Int.down + Vector2Int.down)) {
                            walls.Add(new Wall(down, 26));
                        } else {
                            if (floor.Contains(down + Vector2Int.down + Vector2Int.down + Vector2Int.right)) 
                                walls.Add(new Wall(down, 27));
                            else if (floor.Contains(down + Vector2Int.down + Vector2Int.down + Vector2Int.left))
                                walls.Add(new Wall(down, 37));
                            else {
                                float random = Random.value;
                                walls.Add(random <= .03f ? new Wall(down, Random.value >= .5f ? 38 : 39) : new Wall(down, 0));
                            }
                        }
                        walls.Add(new Wall(tile, 1));
                    }
                } else if (!floor.Contains(up)) {
                    if (floor.Contains(rightUp) && floor.Contains(up + Vector2Int.up)) {
                        walls.Add(new Wall(up, 2));
                        walls.Add(new Wall(up + Vector2Int.up, 3));
                    } else if (floor.Contains(leftUp) && floor.Contains(up + Vector2Int.up)) {
                        walls.Add(new Wall(up, 4));
                        walls.Add(new Wall(up + Vector2Int.up, 5));
                    } else if (floor.Contains(rightUp) && floor.Contains(leftUp) && (floor.Contains(rightUp + Vector2Int.up) || floor.Contains(leftUp + Vector2Int.up))) {
                        walls.Add(new Wall(up, 20));
                        if (floor.Contains(up + Vector2Int.up + Vector2Int.up))
                            walls.Add(new Wall(up + Vector2Int.up, 45)); // 536
                        else {
                            walls.Add(new Wall(up + Vector2Int.up, 21));
                            if (floor.Contains(up + Vector2Int.up + Vector2Int.up + Vector2Int.right) && !floor.Contains(up + Vector2Int.up + Vector2Int.up + Vector2Int.left) && !floor.Contains(up + Vector2Int.up + Vector2Int.left))
                                walls.Add(new Wall(up + Vector2Int.up + Vector2Int.up, 35));
                        }
                    } else if ((floor.Contains(rightUp + Vector2Int.up) || floor.Contains(rightUp)) && !floor.Contains(up + Vector2Int.up)) {
                        if (floor.Contains(rightUp) && !floor.Contains(leftUp))
                            walls.Add(new Wall(up, 6));
                        else if (floor.Contains(leftUp) && !floor.Contains(rightUp))
                            walls.Add(new Wall(up, 4));
                        else
                            walls.Add(new Wall(up, 8));
                        if (!floor.Contains(up + Vector2Int.up + Vector2Int.up)) {
                            if (!floor.Contains(leftUp + Vector2Int.up))
                                walls.Add(new Wall(up + Vector2Int.up, 7));
                            else if (floor.Contains(leftUp + Vector2Int.up) && floor.Contains(rightUp + Vector2Int.up))
                                walls.Add(new Wall(up + Vector2Int.up, 21));
                        }
                        if (!floor.Contains(up + Vector2Int.up + Vector2Int.right) && !floor.Contains(leftUp + Vector2Int.up + Vector2Int.left))
                            if (!floor.Contains(up + Vector2Int.up + Vector2Int.up + Vector2Int.left) && floor.Contains(up + Vector2Int.up + Vector2Int.up + Vector2Int.up) && floor.Contains(up + Vector2Int.up + Vector2Int.up))
                                walls.Add(new Wall(up + Vector2Int.up + Vector2Int.up, 41));
                            else if (!floor.Contains(up + Vector2Int.up + Vector2Int.up + Vector2Int.left) && floor.Contains(up + Vector2Int.up + Vector2Int.up + Vector2Int.up))
                                walls.Add(new Wall(up + Vector2Int.up + Vector2Int.up, 27));
                            else if (floor.Contains(up + Vector2Int.up + Vector2Int.up))
                                walls.Add(new Wall(up + Vector2Int.up + Vector2Int.up, 42));
                            else 
                                walls.Add(new Wall(up + Vector2Int.up + Vector2Int.up, 18));
                        else if (floor.Contains(up + Vector2Int.up + Vector2Int.up + Vector2Int.left) && !floor.Contains(up + Vector2Int.up + Vector2Int.right) && floor.Contains(leftUp + Vector2Int.up + Vector2Int.left))
                            walls.Add(new Wall(up + Vector2Int.up + Vector2Int.up, 25));
                        else if (!floor.Contains(up + Vector2Int.up + Vector2Int.up + Vector2Int.right) && !floor.Contains(up + Vector2Int.up + Vector2Int.right))
                            walls.Add(new Wall(up + Vector2Int.up + Vector2Int.up, 18));
                    } else if ((floor.Contains(leftUp + Vector2Int.up) || floor.Contains(leftUp)) && !floor.Contains(up + Vector2Int.up)) {
                        walls.Add(new Wall(up, 8));
                        if (floor.Contains(up + Vector2Int.up + Vector2Int.up))
                            walls.Add(new Wall(up + Vector2Int.up, 34));
                        else if (floor.Contains(up + Vector2Int.up + Vector2Int.up + Vector2Int.right))
                            walls.Add(new Wall(up + Vector2Int.up, 40)); // 532
                        else 
                            walls.Add(new Wall(up + Vector2Int.up, 9));
                        if (!floor.Contains(up + Vector2Int.up + Vector2Int.left))
                            if (floor.Contains(up + Vector2Int.up + Vector2Int.up + Vector2Int.up) && !floor.Contains(up + Vector2Int.up + Vector2Int.up))
                                walls.Add(new Wall(up + Vector2Int.up + Vector2Int.up, 47)); // 522
                            else if (floor.Contains(up + Vector2Int.up + Vector2Int.up + Vector2Int.right))
                                walls.Add(new Wall(up + Vector2Int.up + Vector2Int.up, 35)); // 517
                            else
                                walls.Add(new Wall(up + Vector2Int.up + Vector2Int.up, 19));
                    } else {
                        float random = Random.value;
                        walls.Add(random <= .03f ? new Wall(up, Random.value >= .5f ? 38 : 39) : new Wall(up, 0));
                        if (!floor.Contains(up + Vector2Int.up + Vector2Int.up + Vector2Int.right) && !floor.Contains(up + Vector2Int.up + Vector2Int.up) && !floor.Contains(up + Vector2Int.up + Vector2Int.up + Vector2Int.left)) {
                            walls.Add(new Wall(up + Vector2Int.up, 1));
                        } else if (floor.Contains(up + Vector2Int.up + Vector2Int.up + Vector2Int.left) && !floor.Contains(up + Vector2Int.up + Vector2Int.up)) {
                            walls.Add(new Wall(up + Vector2Int.up, 30));
                        } else if (floor.Contains(up + Vector2Int.up + Vector2Int.up + Vector2Int.right) && !floor.Contains(up + Vector2Int.up + Vector2Int.up)) {
                            walls.Add(new Wall(up + Vector2Int.up, 32));
                        } else if (floor.Contains(up + Vector2Int.up))
                            walls.Add(new Wall(up + Vector2Int.up, 1));
                    }
                }
                if (!floor.Contains(rightUp) && !floor.Contains(right) && floor.Contains(right + Vector2Int.right) && !floor.Contains(rightDown) && !floor.Contains(rightDown + Vector2Int.down)) {
                    walls.Add(new Wall(right, 24));
                } else if (!floor.Contains(right + Vector2Int.right + Vector2Int.down + Vector2Int.down) && !floor.Contains(right + Vector2Int.right + Vector2Int.down) && !floor.Contains(right + Vector2Int.right) && !floor.Contains(right) && !floor.Contains(rightDown) && !floor.Contains(rightDown + Vector2Int.down) && !floor.Contains(rightUp)) {
                    if (floor.Contains(rightUp + Vector2Int.right)) {
                        walls.Add(new Wall(right, 31));
                    } else {
                        walls.Add(new Wall(right, 15));
                    }
                } else if (!floor.Contains(left + Vector2Int.left + Vector2Int.down + Vector2Int.down) && !floor.Contains(left + Vector2Int.left + Vector2Int.down) && !floor.Contains(left + Vector2Int.left) && !floor.Contains(left) && !floor.Contains(leftDown) && !floor.Contains(leftDown + Vector2Int.down) && !floor.Contains(leftUp)) {
                    if (floor.Contains(left + Vector2Int.left + Vector2Int.up) && !floor.Contains(left + Vector2Int.left)) {
                        walls.Add(new Wall(left, 33));
                    }else {
                        walls.Add(new Wall(left, 14));
                    }
                }
                if (!floor.Contains(down) && !floor.Contains(right) && !floor.Contains(rightDown + Vector2Int.down)) {
                    if (floor.Contains(rightDown + Vector2Int.right))
                        walls.Add(new Wall(rightDown, 33));
                    else if (floor.Contains(rightDown + Vector2Int.down + Vector2Int.down) && !floor.Contains(down + Vector2Int.down))
                        walls.Add(new Wall(rightDown, 30));
                    else if (floor.Contains(rightDown + Vector2Int.down + Vector2Int.down))
                        walls.Add(new Wall(rightDown, 9));
                    else if (!floor.Contains(rightDown + Vector2Int.down + Vector2Int.down) && floor.Contains(rightDown + Vector2Int.down + Vector2Int.down + Vector2Int.right) && !floor.Contains(right + Vector2Int.right) && !floor.Contains(right + Vector2Int.right + Vector2Int.down))
                        walls.Add(new Wall(rightDown, 41)); // 534
                    else if (floor.Contains(rightDown + Vector2Int.down + Vector2Int.down + Vector2Int.right))
                        walls.Add(new Wall(rightDown, 40));
                    else if (floor.Contains(down + Vector2Int.down) && !floor.Contains(rightDown + Vector2Int.down))
                        walls.Add(new Wall(rightDown, 15)); // 182
                    else if (floor.Contains(down + Vector2Int.down + Vector2Int.down))
                        walls.Add(new Wall(rightDown, 44)); // 538
                    else 
                        walls.Add(new Wall(rightDown, 17));
                } else if (!floor.Contains(down) && !floor.Contains(left) && !floor.Contains(leftDown + Vector2Int.down)) {
                    if (floor.Contains(leftDown + Vector2Int.left))
                        walls.Add(new Wall(leftDown, 31));
                    else if (floor.Contains(leftDown + Vector2Int.down + Vector2Int.down) && floor.Contains(leftDown + Vector2Int.down + Vector2Int.left))
                        walls.Add(new Wall(leftDown, 40)); // 532
                    else if (floor.Contains(leftDown + Vector2Int.down + Vector2Int.down))
                        walls.Add(new Wall(leftDown, 32));
                    else if (floor.Contains(leftDown + Vector2Int.down + Vector2Int.left))
                        walls.Add(new Wall(leftDown, 36)); // 529
                    else
                        walls.Add(new Wall(leftDown, 16));
                }
                if (!floor.Contains(up) && !floor.Contains(leftUp) && !floor.Contains(left) && !floor.Contains(leftDown) && !floor.Contains(leftUp + Vector2Int.up) && floor.Contains(left + Vector2Int.left + Vector2Int.up)) {
                    walls.Add(new Wall(leftUp, 24));
                    if (floor.Contains(leftUp + Vector2Int.up + Vector2Int.up + Vector2Int.up) && floor.Contains(leftUp + Vector2Int.up + Vector2Int.up)) {
                        walls.Add(new Wall(leftUp + Vector2Int.up, 29));
                    } else {
                        walls.Add(new Wall(leftUp + Vector2Int.up, 25));
                    }
                } else if (!floor.Contains(up) && !floor.Contains(rightUp) && !floor.Contains(right) && !floor.Contains(rightDown) && !floor.Contains(rightUp + Vector2Int.up)) {
                    if (floor.Contains(rightUp + Vector2Int.up + Vector2Int.right)) {
                        if (floor.Contains(rightUp + Vector2Int.up + Vector2Int.up))
                            walls.Add(new Wall(rightUp + Vector2Int.up, 48)); // 524
                        else 
                            walls.Add(new Wall(rightUp + Vector2Int.up, 35));
                        if (floor.Contains(rightUp + Vector2Int.right))
                            walls.Add(new Wall(rightUp, 24));
                        else 
                            walls.Add(new Wall(rightUp, 31));
                    } else {
                        walls.Add(new Wall(rightUp, 15));
                        if (floor.Contains(rightUp + Vector2Int.up + Vector2Int.up + Vector2Int.up) && floor.Contains(rightUp + Vector2Int.up + Vector2Int.up)) {
                            walls.Add(new Wall(rightUp + Vector2Int.up, 37));
                        } else if (floor.Contains(rightUp + Vector2Int.up + Vector2Int.up)) {
                            walls.Add(new Wall(rightUp + Vector2Int.up, 36));
                        } else if (floor.Contains(up + Vector2Int.up))
                            walls.Add(new Wall(rightUp + Vector2Int.up, 15));
                        else if (floor.Contains(up + Vector2Int.up + Vector2Int.up) && !floor.Contains(rightUp + Vector2Int.up))
                            walls.Add(new Wall(rightUp + Vector2Int.up, 44)); // 538
                        else {
                            walls.Add(new Wall(rightUp + Vector2Int.up, 19));
                        }
                    }
                } else if (!floor.Contains(up) && !floor.Contains(leftUp) && !floor.Contains(left) && !floor.Contains(leftDown) && !floor.Contains(leftUp + Vector2Int.up)) {
                    if (floor.Contains(leftUp + Vector2Int.left + Vector2Int.up))
                        walls.Add(new Wall(leftUp, 33));
                    else if (floor.Contains(left + Vector2Int.left) && !floor.Contains(leftUp + Vector2Int.up))
                        walls.Add(new Wall(leftUp, 24)); // 521
                    else
                        walls.Add(new Wall(leftUp, 14));
                    if (floor.Contains(leftUp + Vector2Int.up + Vector2Int.up) && !floor.Contains(leftUp + Vector2Int.up))
                        walls.Add(new Wall(leftUp + Vector2Int.up, 27));
                    else if (floor.Contains(leftUp + Vector2Int.left + Vector2Int.up + Vector2Int.up) && !floor.Contains(leftUp + Vector2Int.up + Vector2Int.left))
                        walls.Add(new Wall(leftUp + Vector2Int.up, 40));
                    else if (floor.Contains(leftUp + Vector2Int.up + Vector2Int.left))
                        walls.Add(new Wall(leftUp + Vector2Int.up, 25));
                    else if (floor.Contains(left + Vector2Int.left) && !floor.Contains(leftUp + Vector2Int.up) && !floor.Contains(leftUp) && !floor.Contains(leftUp + Vector2Int.up + Vector2Int.up))
                        walls.Add(new Wall(leftUp + Vector2Int.up, 46)); // 537
                    else
                        walls.Add(new Wall(leftUp + Vector2Int.up, 18));
                }
                if (!floor.Contains(leftDown + Vector2Int.down) && !floor.Contains(leftUp) && !floor.Contains(left) && !floor.Contains(left + Vector2Int.left) && floor.Contains(left + Vector2Int.left + Vector2Int.left) && !floor.Contains(left + Vector2Int.left + Vector2Int.down) && floor.Contains(left + Vector2Int.left + Vector2Int.down + Vector2Int.down))
                    walls.Add(new Wall(left, 35)); // 517]
                if (floor.Contains(leftDown + Vector2Int.down + Vector2Int.left) &&!floor.Contains(left) && !floor.Contains(left + Vector2Int.left) && !floor.Contains(leftDown) && !floor.Contains(leftDown + Vector2Int.down) && !floor.Contains(leftUp) && !floor.Contains(leftUp + Vector2Int.up) && !floor.Contains(left + Vector2Int.left + Vector2Int.down) && !floor.Contains(left + Vector2Int.left + Vector2Int.up) && floor.Contains(up) && floor.Contains(down))
                    walls.Add(new Wall(left, 35)); // 517]
                if (!floor.Contains(rightDown + Vector2Int.down) && !floor.Contains(right) && !floor.Contains(rightDown) && !floor.Contains(rightDown + Vector2Int.right) && floor.Contains(rightDown + Vector2Int.right + Vector2Int.down)) {
                    walls.Add(new Wall(right, 25));
                }
            }
            return walls;
        }
    }
}
