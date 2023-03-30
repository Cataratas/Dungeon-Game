using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace Dungeon {
    public class TilemapVisualizer : MonoBehaviour {
        [SerializeField] public Tilemap floorTilemap, wallTilemap, heatmapTilemap;
        [SerializeField] public List<TileBase> floorTiles, wallTiles;

        public void paintFloorTiles(IEnumerable<Vector2Int> tiles) {
            foreach (var tile in tiles) {
                int tileBase = Random.value <= 0.03f ? Random.Range(1, 3) : 0;
                paintSingleTile(floorTilemap, floorTiles[tileBase], tile);
            }
        }
        public void paintSingleTile(Tilemap tilemap, TileBase tileBase, Vector2Int position) {
            var tilePosition = tilemap.WorldToCell((Vector3Int) position);
            tilemap.SetTile(tilePosition, tileBase);
        }

        public void paintSingleWall(Vector2Int pos, int type) {
            paintSingleTile(wallTilemap, wallTiles[type], pos);
        }
        
        public void paintHeatmap(int[,] heatmap) {
            Color lowCostColor = Color.green;
            Color highCostColor = Color.red;

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
            for (int x = 0; x < heatmap.GetLength(0); x++) {
                for (int y = 0; y < heatmap.GetLength(1); y++) {
                    int cost = heatmap[x, y];
                    if (cost == -1) continue;

                    float t = Mathf.Clamp01((float)cost / (float)maxCost);
                    Color color = Color.Lerp(lowCostColor, highCostColor, t);
                    
                    heatmapTilemap.SetTile(new Vector3Int(x, y, 0), floorTiles[0]);
                    heatmapTilemap.SetTileFlags(new Vector3Int(x, y, 0), TileFlags.None);
                    heatmapTilemap.SetColor(new Vector3Int(x, y, 0), color);
                }
            }
        }

        public void clear() {
            wallTilemap.ClearAllTiles();
            floorTilemap.ClearAllTiles();
            heatmapTilemap.ClearAllTiles();
        }
    }
}
