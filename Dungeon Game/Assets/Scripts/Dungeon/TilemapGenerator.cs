using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace Dungeon {
    public class TilemapGenerator : MonoBehaviour {
        [SerializeField] public Tilemap floorTilemap, wallTilemap;
        [SerializeField] public List<TileBase> floorTiles, wallTiles;

        public void paintFloorTiles(IEnumerable<Vector2Int> tiles) {
            foreach (var tile in tiles) {
                int tileBase = Random.value <= 0.03f ? Random.Range(1, floorTiles.Count) : 0;
                paintSingleTile(floorTilemap, floorTiles[tileBase], tile);
            }
        }
        private static void paintSingleTile(Tilemap tilemap, TileBase tileBase, Vector2Int position) {
            var tilePosition = tilemap.WorldToCell((Vector3Int) position);
            tilemap.SetTile(tilePosition, tileBase);
        }

        public void paintSingleWall(Vector2Int pos, int type) {
            paintSingleTile(wallTilemap, wallTiles[type], pos);
        }

        public void clear() {
            wallTilemap.ClearAllTiles();
            floorTilemap.ClearAllTiles();
        }
    }
}
