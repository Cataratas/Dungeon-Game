using System.Collections.Generic;
using UnityEngine;

namespace Dungeon {
    public static class Direction {
        public readonly static HashSet<Vector2Int> Neighbors = new HashSet<Vector2Int> {
            Vector2Int.up,
            Vector2Int.up + Vector2Int.right,
            Vector2Int.right,
            Vector2Int.right + Vector2Int.down,
            Vector2Int.down,
            Vector2Int.left + Vector2Int.down,
            Vector2Int.left,
            Vector2Int.left + Vector2Int.up,
            
            Vector2Int.up + Vector2Int.up,
            Vector2Int.up + Vector2Int.up + Vector2Int.right,
            Vector2Int.up + Vector2Int.up + Vector2Int.right + Vector2Int.right,
            Vector2Int.right + Vector2Int.right + Vector2Int.up,
            Vector2Int.right + Vector2Int.right,
            Vector2Int.right + Vector2Int.right + Vector2Int.down,
            Vector2Int.right + Vector2Int.right + Vector2Int.down + Vector2Int.down,
            Vector2Int.down + Vector2Int.down + Vector2Int.right,
            Vector2Int.down + Vector2Int.down,
            Vector2Int.down + Vector2Int.down + Vector2Int.left,
            Vector2Int.down + Vector2Int.down + Vector2Int.left + Vector2Int.left,
            Vector2Int.left + Vector2Int.left + Vector2Int.down,
            Vector2Int.left + Vector2Int.left,
            Vector2Int.left + Vector2Int.left + Vector2Int.up,
            Vector2Int.left + Vector2Int.left + Vector2Int.up + Vector2Int.up,
            Vector2Int.left + Vector2Int.up + Vector2Int.up
        };
    }
}
