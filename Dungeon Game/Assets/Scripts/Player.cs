using UnityEngine;

public class Player : MonoBehaviour {
    public float moveSpeed = 200f;
    private Vector2 move;
    private Rigidbody2D rb;
    
    private void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        rb.velocity = new Vector2(move.x * moveSpeed * Time.deltaTime, move.y * moveSpeed * Time.deltaTime);
    }

    private void Update() {
        move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    public void setPos(Vector2 pos) {
        transform.position = new Vector3(pos.x, pos.y);
    }
}
