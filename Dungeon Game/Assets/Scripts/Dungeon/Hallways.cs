using System;
using System.Collections.Generic;
using System.Linq;
using Dungeon.Utils.unity_delaunay_mst.Assets.Scripts.DungeonGen;
using UnityEngine;

namespace Dungeon {
    public partial class DungeonGenerator {
        private void generateHallways() {
            var triangles = BowyerWatson.Triangulate(dungeon.centers);
            var delaunay = new HashSet<Edge>();
            foreach (var triangle in triangles) {
                delaunay.UnionWith(triangle.edges);
            }
            graph = Kruskal.MinimumSpanningTree(delaunay);

            var graphList = delaunay.Except(graph).ToList();
            var numElements = (int) Math.Round(graphList.Count * PercentageOfEdges);
            var selectedList = new List<Edge>();

            while (selectedList.Count < numElements)
                selectedList.Add(graphList[UnityEngine.Random.Range(0, graphList.Count)]);
            graph.AddRange(selectedList);

            foreach (var edge in graph) {
                var p1 = new Vector3(edge.a.x, edge.a.y);
                var p2 = new Vector3(edge.b.x, edge.b.y);
                dungeon.AddHallway(generateHallway(new Vector2Int((int) p1.x, (int) p1.y), new Vector2Int((int) p2.x, (int) p2.y)));
            }
        }

        private static IEnumerable<Vector2Int> generateHallway(Vector2Int start, Vector2Int end) {
            var hallway = new HashSet<Vector2Int>();
            int dx = end.x - start.x;
            int dy = end.y - start.y;
            int xStep = dx > 0 ? 1 : -1;
            int yStep = dy > 0 ? 1 : -1;
            int x = start.x;
            int y = start.y;
            if (Mathf.Abs(dx) > Mathf.Abs(dy)) {
                while (x != end.x) {
                    for (var i = 0; i < HallwayWidth; i++) {
                        hallway.Add(new Vector2Int(x, y + i * yStep));
                    }
                    x += xStep;
                }
                while (y != end.y) {
                    for (var i = 0; i < HallwayWidth; i++) {
                        hallway.Add(new Vector2Int(x + i * xStep, y));
                    }
                    y += yStep;
                }
            } else {
                while (y != end.y) {
                    for (var i = 0; i < HallwayWidth; i++) {
                        hallway.Add(new Vector2Int(x + i * xStep, y));
                    }
                    y += yStep;
                }
                while (x != end.x) {
                    for (var i = 0; i < HallwayWidth; i++) {
                        hallway.Add(new Vector2Int(x, y + i * yStep));
                    }
                    x += xStep;
                }
            }
            return hallway;
        }
    }
}
