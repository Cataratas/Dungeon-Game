using System.Collections.Generic;
using System.Diagnostics;
using Dungeon;
using UnityEditor.UIElements;
using UnityEngine;
using Debug = UnityEngine.Debug;


public static class DungeonWallGenerator {
    public class Wall {
        public readonly Vector2Int pos;
        public readonly int type;

        public Wall(Vector2Int position, int wallType)  {
            pos = position;
            type = wallType;
        }
    }

    public static void createWalls(HashSet<Vector2Int> floor, TilemapGenerator tilemapGenerator) {
        var sw = Stopwatch.StartNew();
        try {
            var walls = findWallsInDirections(floor);
            foreach (var wall in walls) {
                tilemapGenerator.paintSingleWall(wall.pos, wall.type);
            }
        }
        finally {
            sw.Stop();
            Debug.Log(sw.ElapsedMilliseconds);
        }
        
    }

    private static HashSet<Wall> findWallsInDirections(HashSet<Vector2Int> floor) {
        var walls = new HashSet<Wall>();
        foreach(var tile in floor) {
            var neighborUp = tile + Vector2Int.up;
            var neighborDown = tile + Vector2Int.down;
            var neighborRight = tile + Vector2Int.right;
            var neighborLeft = tile + Vector2Int.left;
            var neighborLeftUp = neighborLeft + Vector2Int.up;
            var neighborLeftDown = neighborLeft + Vector2Int.down;
            var neighborRightUp = neighborRight + Vector2Int.up;
            var neighborRightDown = neighborRight + Vector2Int.down;
            
            if (!floor.Contains(neighborDown)) {
                if (floor.Contains(neighborLeftDown) && floor.Contains(neighborRightDown)) {
                    if (floor.Contains(neighborDown + Vector2Int.down))
                        walls.Add(new Wall(neighborDown, 20));
                    else if (floor.Contains(neighborDown + Vector2Int.down + Vector2Int.down))
                        walls.Add(new Wall(neighborDown, 45)); // 536
                    else 
                        walls.Add(new Wall(neighborDown, 22));
                    walls.Add(new Wall(tile, 23));
                } else if ((floor.Contains(neighborRightDown) || floor.Contains(neighborRightDown + Vector2Int.down)) && !floor.Contains(neighborDown + Vector2Int.down)) {
                    if (floor.Contains(neighborDown + Vector2Int.down + Vector2Int.down)) {
                        walls.Add(new Wall(neighborDown, 28));
                    } else if (floor.Contains(neighborLeftDown)) {
                        walls.Add(new Wall(neighborDown, 22));
                        walls.Add(new Wall(tile, 23));
                    }
                    else if (!floor.Contains(neighborDown) && !floor.Contains(neighborLeftDown) && floor.Contains(neighborLeftDown + Vector2Int.down) && !floor.Contains(neighborDown + Vector2Int.down + Vector2Int.down) && floor.Contains(neighborRightDown))
                        walls.Add(new Wall(neighborDown, 22)); // 515
                    else
                        walls.Add(new Wall(neighborDown, 10));
                    if (!floor.Contains(neighborLeftDown) && !floor.Contains(neighborLeftDown + Vector2Int.down))
                        walls.Add(new Wall(tile, 11));
                    else {
                        walls.Add(new Wall(tile, 23)); // 514
                    }
                } else if ((floor.Contains(neighborLeftDown) || floor.Contains(neighborLeftDown + Vector2Int.down)) && !floor.Contains(neighborDown + Vector2Int.down)) {
                    if (floor.Contains(neighborDown + Vector2Int.down + Vector2Int.down)) {
                        walls.Add(new Wall(neighborDown, 34));
                    } else {
                        walls.Add(new Wall(neighborDown, 12));
                    }
                    walls.Add(new Wall(tile, 13));
                } else if (!floor.Contains(neighborDown + Vector2Int.down)) {
                    if (floor.Contains(neighborDown + Vector2Int.down + Vector2Int.down)) {
                        walls.Add(new Wall(neighborDown, 26));
                    } else if (!floor.Contains(neighborDown + Vector2Int.down + Vector2Int.down) && floor.Contains(neighborDown + Vector2Int.down + Vector2Int.down + Vector2Int.right)) {
                        walls.Add(new Wall(neighborDown, 27));
                    } 
                    else if (!floor.Contains(neighborDown + Vector2Int.down + Vector2Int.down) && floor.Contains(neighborDown + Vector2Int.down + Vector2Int.down + Vector2Int.left)) {
                        walls.Add(new Wall(neighborDown, 37));
                    } else {
                        var random = Random.value;
                        if (random <= .03f) {
                            walls.Add(new Wall(neighborDown, Random.value >= .5f ? 38 : 39));
                        } else
                            walls.Add(new Wall(neighborDown, 0));
                    }
                    walls.Add(new Wall(tile, 1));
                }
            } else if (!floor.Contains(neighborUp)) {
                if (floor.Contains(neighborRightUp) && floor.Contains(neighborUp + Vector2Int.up)) {
                    walls.Add(new Wall(neighborUp, 2));
                    walls.Add(new Wall(neighborUp + Vector2Int.up, 3));
                } else if (floor.Contains(neighborLeftUp) && floor.Contains(neighborUp + Vector2Int.up)) {
                    walls.Add(new Wall(neighborUp, 4));
                    walls.Add(new Wall(neighborUp + Vector2Int.up, 5));
                } else if (floor.Contains(neighborRightUp) && floor.Contains(neighborLeftUp) && (floor.Contains(neighborRightUp + Vector2Int.up) || floor.Contains(neighborLeftUp + Vector2Int.up))) {
                    walls.Add(new Wall(neighborUp, 20));
                    if (floor.Contains(neighborUp + Vector2Int.up + Vector2Int.up))
                        walls.Add(new Wall(neighborUp + Vector2Int.up, 45)); // 536
                    else 
                        walls.Add(new Wall(neighborUp + Vector2Int.up, 21));
                } else if ((floor.Contains(neighborRightUp + Vector2Int.up) || floor.Contains(neighborRightUp)) && !floor.Contains(neighborUp + Vector2Int.up)) {
                    if (floor.Contains(neighborRightUp) && !floor.Contains(neighborLeftUp))
                        walls.Add(new Wall(neighborUp, 6));
                    else if (floor.Contains(neighborLeftUp) && !floor.Contains(neighborRightUp))
                        walls.Add(new Wall(neighborUp, 4));
                    else
                        walls.Add(new Wall(neighborUp, 8));
                    if (!floor.Contains(neighborUp + Vector2Int.up + Vector2Int.up) && !floor.Contains(neighborLeftUp + Vector2Int.up)) {
                        walls.Add(new Wall(neighborUp + Vector2Int.up, 7));
                    } else if (!floor.Contains(neighborUp + Vector2Int.up + Vector2Int.up) && floor.Contains(neighborLeftUp + Vector2Int.up) && floor.Contains(neighborRightUp + Vector2Int.up))
                        walls.Add(new Wall(neighborUp + Vector2Int.up, 21));
                    if (!floor.Contains(neighborUp + Vector2Int.up + Vector2Int.right) && !floor.Contains(neighborLeftUp + Vector2Int.up + Vector2Int.left))
                        if (!floor.Contains(neighborUp + Vector2Int.up + Vector2Int.up + Vector2Int.left) && floor.Contains(neighborUp + Vector2Int.up + Vector2Int.up + Vector2Int.up) && floor.Contains(neighborUp + Vector2Int.up + Vector2Int.up))
                            walls.Add(new Wall(neighborUp + Vector2Int.up + Vector2Int.up, 41));
                        else if (!floor.Contains(neighborUp + Vector2Int.up + Vector2Int.up + Vector2Int.left) && floor.Contains(neighborUp + Vector2Int.up + Vector2Int.up + Vector2Int.up))
                            walls.Add(new Wall(neighborUp + Vector2Int.up + Vector2Int.up, 27));
                        else if (floor.Contains(neighborUp + Vector2Int.up + Vector2Int.up))
                            walls.Add(new Wall(neighborUp + Vector2Int.up + Vector2Int.up, 42));
                        else 
                            walls.Add(new Wall(neighborUp + Vector2Int.up + Vector2Int.up, 18));
                    else if (floor.Contains(neighborUp + Vector2Int.up + Vector2Int.up + Vector2Int.left) && !floor.Contains(neighborUp + Vector2Int.up + Vector2Int.right) && floor.Contains(neighborLeftUp + Vector2Int.up + Vector2Int.left))
                        walls.Add(new Wall(neighborUp + Vector2Int.up + Vector2Int.up, 25));
                    else if (!floor.Contains(neighborUp + Vector2Int.up + Vector2Int.up + Vector2Int.right) && !floor.Contains(neighborUp + Vector2Int.up + Vector2Int.right))
                        walls.Add(new Wall(neighborUp + Vector2Int.up + Vector2Int.up, 18));
                } else if ((floor.Contains(neighborLeftUp + Vector2Int.up) || floor.Contains(neighborLeftUp)) && !floor.Contains(neighborUp + Vector2Int.up)) {
                    walls.Add(new Wall(neighborUp, 8));
                    if(floor.Contains(neighborUp + Vector2Int.up + Vector2Int.up))
                        walls.Add(new Wall(neighborUp + Vector2Int.up, 34));
                    else
                        walls.Add(new Wall(neighborUp + Vector2Int.up, 9));
                    if (!floor.Contains(neighborUp + Vector2Int.up + Vector2Int.left))
                        if (floor.Contains(neighborUp + Vector2Int.up + Vector2Int.up + Vector2Int.up) && !floor.Contains(neighborUp + Vector2Int.up + Vector2Int.up))
                            walls.Add(new Wall(neighborUp + Vector2Int.up + Vector2Int.up, 47)); // 522
                        else if (floor.Contains(neighborUp + Vector2Int.up + Vector2Int.up + Vector2Int.right))
                            walls.Add(new Wall(neighborUp + Vector2Int.up + Vector2Int.up, 35)); // 517
                        else
                            walls.Add(new Wall(neighborUp + Vector2Int.up + Vector2Int.up, 19));
                } else {
                    var random = Random.value;
                    if (random <= .03f) {
                        walls.Add(new Wall(neighborUp, Random.value >= .5f ? 38 : 39));
                    } else
                        walls.Add(new Wall(neighborUp, 0));
                    if (!floor.Contains(neighborUp + Vector2Int.up + Vector2Int.up + Vector2Int.right) && !floor.Contains(neighborUp + Vector2Int.up + Vector2Int.up) && !floor.Contains(neighborUp + Vector2Int.up + Vector2Int.up + Vector2Int.left)) {
                        walls.Add(new Wall(neighborUp + Vector2Int.up, 1));
                    } else if (floor.Contains(neighborUp + Vector2Int.up + Vector2Int.up + Vector2Int.left) && !floor.Contains(neighborUp + Vector2Int.up + Vector2Int.up)) {
                        walls.Add(new Wall(neighborUp + Vector2Int.up, 30));
                    } else if (floor.Contains(neighborUp + Vector2Int.up + Vector2Int.up + Vector2Int.right) && !floor.Contains(neighborUp + Vector2Int.up + Vector2Int.up)) {
                        walls.Add(new Wall(neighborUp + Vector2Int.up, 32));
                    } else if (floor.Contains(neighborUp + Vector2Int.up))
                        walls.Add(new Wall(neighborUp + Vector2Int.up, 1));
                }
            }
            if (!floor.Contains(neighborRightUp) && !floor.Contains(neighborRight) && floor.Contains(neighborRight + Vector2Int.right) && !floor.Contains(neighborRightDown) && !floor.Contains(neighborRightDown + Vector2Int.down)) {
                walls.Add(new Wall(neighborRight, 24));
            } else if (!floor.Contains(neighborRight + Vector2Int.right + Vector2Int.down + Vector2Int.down) && !floor.Contains(neighborRight + Vector2Int.right + Vector2Int.down) && !floor.Contains(neighborRight + Vector2Int.right) && !floor.Contains(neighborRight) && !floor.Contains(neighborRightDown) && !floor.Contains(neighborRightDown + Vector2Int.down) && !floor.Contains(neighborRightUp)) {
                if (floor.Contains(neighborRightUp + Vector2Int.right)) {
                    walls.Add(new Wall(neighborRight, 31));
                } else {
                    walls.Add(new Wall(neighborRight, 15));
                }
            } else if (!floor.Contains(neighborLeft + Vector2Int.left + Vector2Int.down + Vector2Int.down) && !floor.Contains(neighborLeft + Vector2Int.left + Vector2Int.down) && !floor.Contains(neighborLeft + Vector2Int.left) && !floor.Contains(neighborLeft) && !floor.Contains(neighborLeftDown) && !floor.Contains(neighborLeftDown + Vector2Int.down) && !floor.Contains(neighborLeftUp)) {
                if (floor.Contains(neighborLeft + Vector2Int.left + Vector2Int.up) && !floor.Contains(neighborLeft + Vector2Int.left)) {
                    walls.Add(new Wall(neighborLeft, 33));
                }else {
                    walls.Add(new Wall(neighborLeft, 14));
                }
            }
            if (!floor.Contains(neighborDown) && !floor.Contains(neighborRight) && !floor.Contains(neighborRightDown + Vector2Int.down)) {
                if (floor.Contains(neighborRightDown + Vector2Int.right))
                    walls.Add(new Wall(neighborRightDown, 33));
                else if (floor.Contains(neighborRightDown + Vector2Int.down + Vector2Int.down) && !floor.Contains(neighborDown + Vector2Int.down))
                    walls.Add(new Wall(neighborRightDown, 30));
                else if (floor.Contains(neighborRightDown + Vector2Int.down + Vector2Int.down))
                    walls.Add(new Wall(neighborRightDown, 9));
                else if (!floor.Contains(neighborRightDown + Vector2Int.down + Vector2Int.down) && floor.Contains(neighborRightDown + Vector2Int.down + Vector2Int.down + Vector2Int.right) && !floor.Contains(neighborRight + Vector2Int.right) && !floor.Contains(neighborRight + Vector2Int.right + Vector2Int.down))
                    walls.Add(new Wall(neighborRightDown, 41)); // 534
                else if (floor.Contains(neighborRightDown + Vector2Int.down + Vector2Int.down + Vector2Int.right))
                    walls.Add(new Wall(neighborRightDown, 40));
                else if (floor.Contains(neighborDown + Vector2Int.down) && !floor.Contains(neighborRightDown + Vector2Int.down))
                    walls.Add(new Wall(neighborRightDown, 15)); // 182
                else if (floor.Contains(neighborDown + Vector2Int.down + Vector2Int.down))
                    walls.Add(new Wall(neighborRightDown, 44)); // 538
                else 
                    walls.Add(new Wall(neighborRightDown, 17));
            } else if (!floor.Contains(neighborDown) && !floor.Contains(neighborLeft) && !floor.Contains(neighborLeftDown + Vector2Int.down)) {
                if (floor.Contains(neighborLeftDown + Vector2Int.left))
                    walls.Add(new Wall(neighborLeftDown, 31));
                else if (floor.Contains(neighborLeftDown + Vector2Int.down + Vector2Int.down) && floor.Contains(neighborLeftDown + Vector2Int.down + Vector2Int.left))
                    walls.Add(new Wall(neighborLeftDown, 40)); // 532
                else if (floor.Contains(neighborLeftDown + Vector2Int.down + Vector2Int.down))
                    walls.Add(new Wall(neighborLeftDown, 32));
                else if (floor.Contains(neighborLeftDown + Vector2Int.down + Vector2Int.left))
                    walls.Add(new Wall(neighborLeftDown, 36)); // 529
                else
                    walls.Add(new Wall(neighborLeftDown, 16));
            }
            if (!floor.Contains(neighborUp) && !floor.Contains(neighborLeftUp) && !floor.Contains(neighborLeft) && !floor.Contains(neighborLeftDown) && !floor.Contains(neighborLeftUp + Vector2Int.up) && floor.Contains(neighborLeft + Vector2Int.left + Vector2Int.up)) {
                walls.Add(new Wall(neighborLeftUp, 24));
                if (floor.Contains(neighborLeftUp + Vector2Int.up + Vector2Int.up + Vector2Int.up) && floor.Contains(neighborLeftUp + Vector2Int.up + Vector2Int.up)) {
                    walls.Add(new Wall(neighborLeftUp + Vector2Int.up, 29));
                } else {
                    walls.Add(new Wall(neighborLeftUp + Vector2Int.up, 25));
                }
            } else if (!floor.Contains(neighborUp) && !floor.Contains(neighborRightUp) && !floor.Contains(neighborRight) && !floor.Contains(neighborRightDown) && !floor.Contains(neighborRightUp + Vector2Int.up)) {
                if (floor.Contains(neighborRightUp + Vector2Int.up + Vector2Int.right)) {
                    if (floor.Contains(neighborRightUp + Vector2Int.up + Vector2Int.up))
                        walls.Add(new Wall(neighborRightUp + Vector2Int.up, 48)); // 524
                    else 
                        walls.Add(new Wall(neighborRightUp + Vector2Int.up, 35));
                    if (floor.Contains(neighborRightUp + Vector2Int.right))
                        walls.Add(new Wall(neighborRightUp, 24));
                    else 
                        walls.Add(new Wall(neighborRightUp, 31));
                } else {
                    walls.Add(new Wall(neighborRightUp, 15));
                    if (floor.Contains(neighborRightUp + Vector2Int.up + Vector2Int.up + Vector2Int.up) && floor.Contains(neighborRightUp + Vector2Int.up + Vector2Int.up)) {
                        walls.Add(new Wall(neighborRightUp + Vector2Int.up, 37));
                    } else if (floor.Contains(neighborRightUp + Vector2Int.up + Vector2Int.up)) {
                        walls.Add(new Wall(neighborRightUp + Vector2Int.up, 36));
                    } else if (floor.Contains(neighborUp + Vector2Int.up))
                        walls.Add(new Wall(neighborRightUp + Vector2Int.up, 15));
                    else if (floor.Contains(neighborUp + Vector2Int.up + Vector2Int.up) && !floor.Contains(neighborRightUp + Vector2Int.up))
                        walls.Add(new Wall(neighborRightUp + Vector2Int.up, 44)); // 538
                    else {
                        walls.Add(new Wall(neighborRightUp + Vector2Int.up, 19));
                    }
                }
            } else if (!floor.Contains(neighborUp) && !floor.Contains(neighborLeftUp) && !floor.Contains(neighborLeft) && !floor.Contains(neighborLeftDown) && !floor.Contains(neighborLeftUp + Vector2Int.up)) {
                if (floor.Contains(neighborLeftUp + Vector2Int.left + Vector2Int.up))
                    walls.Add(new Wall(neighborLeftUp, 33));
                else if (floor.Contains(neighborLeft + Vector2Int.left) && !floor.Contains(neighborLeftUp + Vector2Int.up))
                    walls.Add(new Wall(neighborLeftUp, 24)); // 521
                else
                    walls.Add(new Wall(neighborLeftUp, 14));
                if (floor.Contains(neighborLeftUp + Vector2Int.up + Vector2Int.up) && !floor.Contains(neighborLeftUp + Vector2Int.up))
                    walls.Add(new Wall(neighborLeftUp + Vector2Int.up, 27));
                else if (floor.Contains(neighborLeftUp + Vector2Int.left + Vector2Int.up + Vector2Int.up) && !floor.Contains(neighborLeftUp + Vector2Int.up + Vector2Int.left))
                    walls.Add(new Wall(neighborLeftUp + Vector2Int.up, 40));
                else if (floor.Contains(neighborLeftUp + Vector2Int.up + Vector2Int.left))
                    walls.Add(new Wall(neighborLeftUp + Vector2Int.up, 25));
                else if (floor.Contains(neighborLeft + Vector2Int.left) && !floor.Contains(neighborLeftUp + Vector2Int.up) && !floor.Contains(neighborLeftUp) && !floor.Contains(neighborLeftUp + Vector2Int.up + Vector2Int.up))
                    walls.Add(new Wall(neighborLeftUp + Vector2Int.up, 46)); // 537
                else
                    walls.Add(new Wall(neighborLeftUp + Vector2Int.up, 18));
            }
            if (!floor.Contains(neighborLeftDown + Vector2Int.down) && !floor.Contains(neighborLeftUp) && !floor.Contains(neighborLeft) && !floor.Contains(neighborLeft + Vector2Int.left) && floor.Contains(neighborLeft + Vector2Int.left + Vector2Int.left) && !floor.Contains(neighborLeft + Vector2Int.left + Vector2Int.down) && floor.Contains(neighborLeft + Vector2Int.left + Vector2Int.down + Vector2Int.down))
                walls.Add(new Wall(neighborLeft, 35)); // 517]
            if (floor.Contains(neighborLeftDown + Vector2Int.down + Vector2Int.left) &&!floor.Contains(neighborLeft) && !floor.Contains(neighborLeft + Vector2Int.left) && !floor.Contains(neighborLeftDown) && !floor.Contains(neighborLeftDown + Vector2Int.down) && !floor.Contains(neighborLeftUp) && !floor.Contains(neighborLeftUp + Vector2Int.up) && !floor.Contains(neighborLeft + Vector2Int.left + Vector2Int.down) && !floor.Contains(neighborLeft + Vector2Int.left + Vector2Int.up) && floor.Contains(neighborUp) && floor.Contains(neighborDown))
                walls.Add(new Wall(neighborLeft, 35)); // 517]
            if (!floor.Contains(neighborRightDown + Vector2Int.down) && !floor.Contains(neighborRight) && !floor.Contains(neighborRightDown) && !floor.Contains(neighborRightDown + Vector2Int.right) && floor.Contains(neighborRightDown + Vector2Int.right + Vector2Int.down)) {
                walls.Add(new Wall(neighborRight, 25));
            }
        }
        return walls;
    }
}

    