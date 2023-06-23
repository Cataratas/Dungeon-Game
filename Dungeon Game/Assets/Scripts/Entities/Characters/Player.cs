using UnityEngine;

namespace Entities.Characters {
    public class Player : Character {
        private Vector2 move;
        private Rigidbody2D rigidBody;
        private SpriteRenderer spriteRenderer;
        private HealthBar healthBar;

        private void Start() {
            rigidBody = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            healthBar = GameObject.FindWithTag("Health Bar").GetComponentInChildren<HealthBar>();
            healthBar.setHealth(maxHealth, health);
        }

        private void FixedUpdate() {
            rigidBody.velocity = new Vector2(move.x * speed * Time.deltaTime, move.y * speed * Time.deltaTime);
            if (move.x is not 0) spriteRenderer.flipX = !(move.x > 0);
        }

        private void Update() {
            move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }
        
        public override void Damage(float damage) {
            health -= damage;
            healthBar.setHealth(maxHealth, health);
        }
    }
}
