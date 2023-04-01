using UnityEngine;

namespace Dungeon {
    public class Chest {
        public Vector2Int pos;
        public readonly int tile = 0;

        public Chest(Vector2Int pos) {
            this.pos = pos;
        }
    }
}
