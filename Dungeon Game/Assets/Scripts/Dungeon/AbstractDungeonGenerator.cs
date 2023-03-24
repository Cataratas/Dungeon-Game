using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DungeonGen;

namespace Dungeon {
    public abstract class AbstractDungeonGenerator : MonoBehaviour {
        [SerializeField] protected TilemapGenerator tilemapGenerator;
        [SerializeField] protected Vector2Int startPos = Vector2Int.zero;

        protected class Dungeon {
            private readonly List<Room> rooms = new List<Room>();
            private readonly HashSet<Vector2Int> hallways = new HashSet<Vector2Int>();
            public readonly HashSet<Point> centers = new HashSet<Point>();

            public void AddRoom(Room room) {
                rooms.Add(room);
                centers.Add(room.center);
            }

            public void AddHallway(IEnumerable<Vector2Int> hallway) {
                hallways.UnionWith(hallway);
            }

            public bool collidesWithRooms(HashSet<Vector2Int> obj) {
                return rooms.Any(room => room.floors.Overlaps(obj));
            }

            public HashSet<Vector2Int> getFloors() {
                var floors = new HashSet<Vector2Int>();
                foreach (var room in rooms)
                    floors.UnionWith(room.floors);
                
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