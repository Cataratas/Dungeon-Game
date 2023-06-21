using Data;
using UnityEngine;

public class Enemy : Character {
    [SerializeField] private EnemyData data;
    protected int damage;

    public void Start() {
        health = data.health;
        damage = data.damage;
        speed = data.speed;
    }
}
