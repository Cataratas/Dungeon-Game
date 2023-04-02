using Data;
using Interfaces;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable {
    [SerializeField]
    private EnemyData data;

    private int heath, damage;
    private float speed;

    public void Awake() {
        heath = data.health;
        damage = data.damage;
        speed = data.speed;
    }
    
    public void Damage() {
        
    }
}
