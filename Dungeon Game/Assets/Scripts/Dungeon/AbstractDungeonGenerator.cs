using System.Collections.Generic;
using UnityEngine;
using DungeonGen;

namespace Dungeon {
    public abstract class AbstractDungeonGenerator : MonoBehaviour {
        [SerializeField] protected TilemapGenerator tilemapGenerator;
        [SerializeField] protected Vector2Int startPos = Vector2Int.zero;

        protected class Dungeon {
            private readonly List<Room> rooms = new();
            private readonly HashSet<Vector2Int> hallways = new();
            public readonly HashSet<Point> centers = new();

            public void AddRoom(Room room) {
                rooms.Add(room);
                centers.Add(room.center);
            }

            public void AddHallway(IEnumerable<Vector2Int> hallway) {
                hallways.UnionWith(hallway);
            }

            public bool collidesWithRooms(HashSet<Vector2Int> obj) {
                foreach (var room in rooms) {
                    if (room.floors.Overlaps(obj))
                        return true;
                }
                return false;
            }

            public HashSet<Vector2Int> getFloors() {
                var floors = new HashSet<Vector2Int>();
                foreach (var room in rooms) {
                    foreach (var tile in room.floors)
                        floors.Add(tile);
                }
                floors.UnionWith(hallways);
                return floors;
            }
        }

        protected class Room {
            public HashSet<Vector2Int> floors = new HashSet<Vector2Int>();
            public Point center;
        }

        public void generateDungeon() {
            tilemapGenerator.clear();
            runProceduralGeneration();
        }

        protected abstract void runProceduralGeneration();
    }
}