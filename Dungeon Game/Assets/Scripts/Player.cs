using UnityEngine;

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
        if (move.x > 0) {
            spriteRenderer.flipX = false;
        } else if (move.x < 0) {
            spriteRenderer.flipX = true;
        }
    }

    private void Update() {
        move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (Input.GetKeyDown(KeyCode.Space)) {
            Damage(1f);
        }
    }

    private new void Damage(float damage) {
        health -= damage;
        healthBar.setHealth(maxHealth, health);
    }
}
