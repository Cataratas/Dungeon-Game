using System;
using System.Collections.Generic;
using Dungeon.Utils;
using UnityEngine;

namespace Dungeon {
    public partial class DungeonGenerator {
        private void generateHeatmap(Vector2Int start, ICollection<Vector2Int> floors) {
            heatmap = new int[size.x + 20, size.y + 20];
            var queue = new PriorityQueue<Vector2Int>();
            
            for (var x = 0; x < size.x + 20; x++) {
                for (var y = 0; y < size.y + 20; y++) {
                    heatmap[x, y] = -1;
                }
            }
            queue.Enqueue(start, 0);
            heatmap[start.x, start.y] = 0;
            
            while (queue.Count != 0) {
                var current = queue.Dequeue();
                
                foreach (var neighbor in GetNeighbors(current, floors)) {
                    int distance = heatmap[current.x, current.y] + 1;
                    
                    if (distance >= heatmap[neighbor.x, neighbor.y] && heatmap[neighbor.x, neighbor.y] != -1)
                        continue;
                    heatmap[neighbor.x, neighbor.y] = distance;
                    queue.Enqueue(neighbor, distance);
                }
            }
        }
        
        private static List<Vector2Int> GetNeighbors(Vector2Int position, ICollection<Vector2Int> floors) {
            var neighbors = new List<Vector2Int>();
            Vector2Int[] directions = { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };
        
            foreach (var direction in directions) {
                var neighbor = position + direction;
            
                if (floors.Contains(neighbor)) {
                    neighbors.Add(neighbor);
                }
            }
    
            return neighbors;
        }

        private Tuple<int, int> getMinMaxHeatmap() {
            int minCost = int.MaxValue;
            int maxCost = int.MinValue;
            for (int x = 0; x < heatmap.GetLength(0); x++) {
                for (int y = 0; y < heatmap.GetLength(1); y++) {
                    int cost = heatmap[x, y];
                    if (cost < minCost) {
                        minCost = cost; 
                    }
                    if (cost > maxCost) {
                        maxCost = cost;
                    }
                }
            }
            return Tuple.Create(minCost, maxCost);
        }
    }
}
