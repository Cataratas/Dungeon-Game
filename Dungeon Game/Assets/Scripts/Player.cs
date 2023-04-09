using Dungeon.Utils.unity_delaunay_mst.Assets.Scripts.DungeonGen;
using UnityEngine;

public class Player : Character {
    private Vector2 move;
    private Rigidbody2D rb;
    
    private void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        rb.velocity = new Vector2(move.x * speed * Time.deltaTime, move.y * speed * Time.deltaTime);
    }

    private void Update() {
        move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    public void setPos(Point pos) {
        transform.position = new Vector3(pos.x, pos.y);
    }
}
