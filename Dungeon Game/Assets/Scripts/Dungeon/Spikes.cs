using System.Collections.Generic;
using UnityEngine;

namespace Dungeon {
    public partial class DungeonGenerator {
        private void generateSpikes(ICollection<Vector2Int> floor) {
            foreach (var tile in dungeon.hallways) {
                if (!(Random.value <= .02f) || dungeon.collideWithRooms(tile))
                    continue;
                foreach (var dir in new List<Vector2Int> { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right }) {
                    if (!floor.Contains(tile + dir * 2) && !floor.Contains(tile + dir * -2)) {
                        for (int i = -1; i < HallwayWidth - 1; i++) {
                            Instantiate(objects[0], new Vector3(tile.x + dir.x * -1 * i + 0.5f, tile.y + dir.y * -1 * i + 0.5f, 0), Quaternion.identity);
                        }
                    } else {
                        if (floor.Contains(tile + dir) || floor.Contains(tile + dir * (-1 * HallwayWidth)))
                            continue;
                        for (var i = 0; i < HallwayWidth; i++)
                            Instantiate(objects[0], new Vector3(tile.x + dir.x * -1 * i + 0.5f, tile.y + dir.y * -1 * i + 0.5f, 0), Quaternion.identity);
                    }
                }
            }
        }
    }
}
