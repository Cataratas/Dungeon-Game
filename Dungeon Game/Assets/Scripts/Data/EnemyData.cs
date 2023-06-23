using UnityEngine;

namespace Data {
    [CreateAssetMenu(fileName = "EnemyData_", menuName = "NewData/EnemyData")]
    public class EnemyData : ScriptableObject {
        public int maxHealth;
        public int damage;
        public float speed;
        public float range;
        public float attackRange;
        public float attackSpeed;
    }
}
