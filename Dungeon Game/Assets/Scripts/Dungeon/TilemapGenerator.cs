using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace Dungeon {
    public class TilemapGenerator : MonoBehaviour {
        [SerializeField] public Tilemap floorTilemap, wallTilemap;
        [SerializeField] public List<TileBase> floorTiles, wallTiles;
        [SerializeField] public List<TileBase> newWallTiles;

        public void paintFloorTiles(IEnumerable<Vector2Int> tiles) {
            foreach (var tile in tiles) {
                var tileBase = 0;
                if (Random.value <= 0.03f)
                    tileBase = Random.Range(1, floorTiles.Count);
                paintSingleTile(floorTilemap, floorTiles[tileBase], tile);
            }
        }
        private static void paintSingleTile(Tilemap tilemap, TileBase tileBase, Vector2Int position) {
            var tilePosition = tilemap.WorldToCell((Vector3Int) position);
            tilemap.SetTile(tilePosition, tileBase);
        }

        public void paintSingleWall(Vector2Int pos, int type) {
            paintSingleTile(wallTilemap, newWallTiles[type], pos);
        }

        public void clear() {
            wallTilemap.ClearAllTiles();
            floorTilemap.ClearAllTiles();
        }
    }
}
