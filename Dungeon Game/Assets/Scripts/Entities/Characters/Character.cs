using Interfaces;
using UnityEngine;

namespace Entities.Characters {
    public class Character : MonoBehaviour, IDamageable {
        [SerializeField] protected float maxHealth;
        [SerializeField] protected float speed = 275f;
        protected float health;

        public void Awake() {
            health = maxHealth;
        }
    
        public virtual void Damage(float damage) {
            health -= damage;
        }
    }
}
