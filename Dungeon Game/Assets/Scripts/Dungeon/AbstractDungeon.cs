using System.Collections.Generic;
using System.Linq;
using Dungeon.Data;
using Dungeon.Utils.unity_delaunay_mst.Assets.Scripts.DungeonGen;
using UnityEngine;

namespace Dungeon {
    public abstract class AbstractDungeonGenerator : MonoBehaviour {
        [SerializeField] protected TilemapVisualizer tilemap;
        protected static Vector2Int startPos = new Vector2Int(10, 10);
        protected class Dungeon {
            public readonly List<Room> rooms = new List<Room>();
            public readonly HashSet<Vector2Int> hallways = new HashSet<Vector2Int>();
            public readonly HashSet<Point> centers = new HashSet<Point>();

            public void AddRoom(Room room) {
                rooms.Add(room);
                centers.Add(room.center);
            }

            public void AddHallway(IEnumerable<Vector2Int> hallway) {
                hallways.UnionWith(hallway);
            }

            public bool collidesWithRooms(HashSet<Vector2Int> tiles) {
                return rooms.Any(room => room.floors.Overlaps(tiles));
            }

            public bool collideWithRooms(Vector2Int tile) {
                return rooms.Any(room => room.floors.Contains(tile));
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
            public RoomParameters parameters;
        }

        public void generateDungeon() {
            tilemap.clear();
            runProceduralGeneration();
        }

        protected abstract void runProceduralGeneration();
    }
}