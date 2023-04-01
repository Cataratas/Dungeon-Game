using UnityEngine;

namespace Dungeon.Data {
    [CreateAssetMenu(fileName = "RoomParameters_", menuName = "PCG/RoomParameters")]
    public class RoomParameters : ScriptableObject {
        [Header("Size")]
        public int minRoomWidth;
        public int minRoomHeight;
        public int maxRoomWidth;
        public int maxRoomHeight;
        [Range(0, 10)]
        public int offset;

        [Header("Parameters")]
        public bool playerCanSpawn;
        public bool playerCanExit;
        [Range(0, 1)]
        public double treasureRoomChance;

        [Header("Chest")]
        public int minChestAmount;
        public int maxChestAmount;
    }
}
