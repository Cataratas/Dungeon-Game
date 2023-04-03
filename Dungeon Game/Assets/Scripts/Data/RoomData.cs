using System.Collections.Generic;
using UnityEngine;

namespace Data {
    [CreateAssetMenu(fileName = "RoomData_", menuName = "NewData/RoomData")]
    public class RoomData : ScriptableObject {
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
        public GameObject chest;
        public int minChestAmount;
        public int maxChestAmount;

        [Header("Enemies")]
        public int minEnemyCount;
        public int maxEnemyCount;
        public List<GameObject> undead;
        public List<GameObject> orcs;
        public List<GameObject> demons;
    }
}
