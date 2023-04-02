using UnityEngine;

namespace Data {
    [CreateAssetMenu(fileName = "EnemyData_", menuName = "NewData/EnemyData")]
    public class EnemyData : ScriptableObject {
        public int health;
        public int damage;
        public float speed;
    }
}
