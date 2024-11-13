using System.Collections.Generic;
using System.Linq;
using Data;
using Dungeon.Utils.unity_delaunay_mst.Assets.Scripts.DungeonGen;
using Unity.VisualScripting;
using UnityEngine;

namespace Dungeon {
    public abstract class AbstractDungeonGenerator : MonoBehaviour {
        [SerializeField] protected TilemapVisualizer tilemap;
        [SerializeField] protected List<GameObject> players;
        protected static Vector2Int startPos = new Vector2Int(10, 10);
        protected class Dungeon {
            public readonly List<Room> rooms = new List<Room>();
            public readonly HashSet<Vector2Int> hallways = new HashSet<Vector2Int>();
            public readonly HashSet<Point> centers = new HashSet<Point>();
            public Vector2Int exit = Vector2Int.zero;
            
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
            public RoomData data;
            public bool exit;
        }

        // TODO: Find another way to reference and destroy gameObjects
        public void generateDungeon()
        {
            foreach (var child in GameObject.FindGameObjectsWithTag("Enemy")) DestroyImmediate(child);
            foreach (var child in GameObject.FindGameObjectsWithTag("Spike")) DestroyImmediate(child);
            foreach (var child in GameObject.FindGameObjectsWithTag("Object")) DestroyImmediate(child);
            foreach (var child in GameObject.FindGameObjectsWithTag("Player")) DestroyImmediate(child);
            foreach (var child in GameObject.FindGameObjectsWithTag("Portal")) DestroyImmediate(child);
            
            tilemap.clear();
            runProceduralGeneration();
        }

        protected abstract void runProceduralGeneration();
    }
}