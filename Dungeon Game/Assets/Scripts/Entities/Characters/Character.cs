using Interfaces;
using UnityEngine;

namespace Entities.Characters {
    public class Character : MonoBehaviour, IDamageable {
        protected float health;
        protected float speed;

        public virtual void Damage(float damage) {
            health -= damage;
        }
    }
}
