using Data;
using UnityEngine;

public class Enemy : Character {
    [SerializeField] private EnemyData data;
    [SerializeField] private int damage = 10;

    public void Awake() {
        health = data.health;
        damage = data.damage;
        speed = data.speed;
    }
}
