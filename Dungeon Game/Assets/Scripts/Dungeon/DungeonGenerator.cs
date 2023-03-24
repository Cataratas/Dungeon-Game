using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Dungeon.Data;
using DungeonGen;
using Debug = UnityEngine.Debug;

namespace Dungeon
{
    public class DungeonGenerator : AbstractDungeonGenerator
    {
        [SerializeField] protected Vector2Int size = new Vector2Int(100, 100);
        [SerializeField] protected int roomQuantity = 25;
        [SerializeField] protected List<RoomParameters> roomParameters;
        [SerializeField] protected int hallwayWidth = 1;
        [SerializeField] [Range(0, 1)] protected double percentageOfEdges = 0.1;
        [SerializeField] protected int seed;
        //[SerializeField] protected string seed;

        protected override void runProceduralGeneration() {
            var dungeon = new Dungeon();

            var randomSeed = UnityEngine.Random.Range(Int32.MinValue, Int32.MaxValue);
            //UnityEngine.Random.InitState(seed.Length > 0 ? seed.GetHashCode() : UnityEngine.Random.Range(Int32.MinValue, Int32.MaxValue));
            UnityEngine.Random.InitState(seed != 0 ? seed : randomSeed);
            if (seed == 0)
                Debug.Log(randomSeed);

            generateRooms(dungeon);
            generateHallways(dungeon);
            
            tilemapGenerator.paintFloorTiles(dungeon.getFloors());
            WallGenerator.generateWalls(dungeon.getFloors(), tilemapGenerator);
        }

        protected void generateHallways(Dungeon dungeon) {
            var triangles = BowyerWatson.Triangulate(dungeon.centers);
            var graph = new HashSet<Edge>();
            foreach (var triangle in triangles) {
                graph.UnionWith(triangle.edges);
            }
            var tree = Kruskal.MinimumSpanningTree(graph);

            var graphList = graph.Except(tree).ToList();
            var numElements = (int) Math.Round(graphList.Count * percentageOfEdges);
            var selectedList = new List<Edge>();

            while (selectedList.Count < numElements)
                selectedList.Add(graphList[UnityEngine.Random.Range(0, graphList.Count)]);

            foreach (var edge in selectedList) {
                tree.Add(edge);
            }

            foreach (var edge in tree) {
                var p1 = new Vector3(edge.a.x, edge.a.y);
                var p2 = new Vector3(edge.b.x, edge.b.y);
                dungeon.AddHallway(generateHallway(new Vector2Int((int) p1.x, (int) p1.y), new Vector2Int((int) p2.x, (int) p2.y)));
            }
        }

        private IEnumerable<Vector2Int> generateHallway(Vector2Int start, Vector2Int end) {
            var hallway = new HashSet<Vector2Int>();
            var dx = end.x - start.x;
            var dy = end.y - start.y;
            var xStep = dx > 0 ? 1 : -1;
            var yStep = dy > 0 ? 1 : -1;
            var x = start.x;
            var y = start.y;
            if (Mathf.Abs(dx) > Mathf.Abs(dy)) {
                while (x != end.x) {
                    for (var i = 0; i < hallwayWidth; i++) {
                        hallway.Add(new Vector2Int(x, y + i * yStep));
                    }
                    x += xStep;
                }
                while (y != end.y) {
                    for (var i = 0; i < hallwayWidth; i++) {
                        hallway.Add(new Vector2Int(x + i * xStep, y));
                    }
                    y += yStep;
                }
            } else {
                while (y != end.y) {
                    for (var i = 0; i < hallwayWidth; i++) {
                        hallway.Add(new Vector2Int(x + i * xStep, y));
                    }
                    y += yStep;
                }
                while (x != end.x) {
                    for (var i = 0; i < hallwayWidth; i++) {
                        hallway.Add(new Vector2Int(x, y + i * yStep));
                    }
                    x += xStep;
                }
            }
            return hallway;
        }

        private void generateRooms(Dungeon dungeon) {
            int roomCount = 0, tries = 0;
            while (roomCount != roomQuantity) {
                if (tries > 99999)
                    break;

                var p = roomParameters[UnityEngine.Random.Range(0, roomParameters.Count)];
                var roomSize = new Vector2Int(UnityEngine.Random.Range(p.minRoomWidth, p.maxRoomWidth), UnityEngine.Random.Range(p.minRoomHeight, p.maxRoomHeight));
                var pos = new Vector2Int(UnityEngine.Random.Range(startPos.x, startPos.x + size.x), UnityEngine.Random.Range(startPos.y, startPos.y + size.y));
                var room = generateRoom(dungeon, p, pos, roomSize);

                if (room.floors.Count > 0) {
                    dungeon.AddRoom(room);
                    roomCount++;
                } else
                    tries++;
            }
        }

        private static Room generateRoom(Dungeon dungeon, RoomParameters p, Vector2Int roomPos, Vector2Int roomSize) {
            var room = new Room();
            var floor = new HashSet<Vector2Int>();

            var pos = roomPos + new Vector2Int((-roomSize.x / 2) - p.offset, (roomSize.y / 2) + p.offset);

            for (var i = 0; i < roomSize.x + p.offset * 2; i++) {
                pos += Vector2Int.right;
                floor.Add(pos);
                var vPos = pos;
                for (var j = 0; j < roomSize.y + p.offset * 2; j++) {
                    vPos += Vector2Int.down;
                    floor.Add(vPos);
                }
            }
            if (dungeon.collidesWithRooms(floor))
                return new Room();

            pos = roomPos + new Vector2Int((-roomSize.x / 2), (roomSize.y / 2));
            floor = new HashSet<Vector2Int>();
            for (var i = 0; i < roomSize.x; i++) {
                pos += Vector2Int.right;
                floor.Add(pos);
                var vPos = pos;
                for (var j = 0; j < roomSize.y; j++) {
                    vPos += Vector2Int.down;
                    floor.Add(vPos);
                }
            }
            room.center = new Point(roomPos.x + 1, roomPos.y);
            room.floors = floor;
            return room;
        }
    }
}