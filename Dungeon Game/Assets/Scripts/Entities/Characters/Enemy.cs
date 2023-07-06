using Data;
using UnityEngine;

namespace Entities.Characters {
    public class Enemy : Character {
        [SerializeField] private EnemyData data;
        private SpriteRenderer spriteRenderer;
        private GameObject playerGameObject;
        private Character player;
        private Transform target;
        private float damage;
        private float range;
        private float attackRange;
        private float attackSpeed;
        private float attackTimer;

        public void Start() {
            spriteRenderer = GetComponent<SpriteRenderer>();
            playerGameObject = GameObject.FindGameObjectWithTag("Player");
            target = playerGameObject.GetComponent<Transform>();
            player = playerGameObject.GetComponent<Player>();
            
            health = data.maxHealth;
            damage = data.damage;
            speed = data.speed;
            range = data.range;
            attackRange = data.attackRange;
            attackSpeed = data.attackSpeed;
        }

        protected void Update() {
            if (health <= 0) Kill();
            
            Move();
            Attack();
        }

        private void Move() {
            if (Vector2.Distance(transform.position, target.position) < range) {
                transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            }
            spriteRenderer.flipX = transform.position.x > target.position.x;
        }

        private void Attack() {
            if (Vector2.Distance(transform.position, target.position) > attackRange)
                return;
            
            if (attackTimer <= 0f) {
                player.Damage(damage);
                attackTimer = 1f / attackSpeed;
            } else {
                attackTimer -= Time.deltaTime;
            }
        }

        private void Kill() {
            Destroy(gameObject);
        }
    }
}
