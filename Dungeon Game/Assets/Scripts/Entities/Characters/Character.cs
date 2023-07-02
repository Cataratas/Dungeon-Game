using Interfaces;
using UnityEngine;

namespace Entities.Characters {
    public class Character : MonoBehaviour, IDamageable {
        [SerializeField] protected float maxHealth;
        [SerializeField] protected float defaultSpeed = 275f;
        protected float speed;
        protected float health;

        public void Awake() {
            health = maxHealth;
            speed = defaultSpeed;
        }
    
        public virtual void Damage(float damage) {
            health -= damage;
        }
    }
}
