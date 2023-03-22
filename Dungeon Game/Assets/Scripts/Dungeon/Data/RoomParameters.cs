using UnityEngine;

namespace Dungeon.Data {
    [CreateAssetMenu(fileName = "RoomParameters_", menuName = "PCG/RoomParameters")]
    public class RoomParameters : ScriptableObject {
        public int minRoomWidth, minRoomHeight;
        public int maxRoomWidth, maxRoomHeight;
        [Range(0, 10)]
        public int offset;
    }
}
