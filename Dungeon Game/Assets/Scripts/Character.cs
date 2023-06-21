using System;
using Interfaces;
using UnityEngine;

public class Character : MonoBehaviour, IDamageable {
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float speed = 275f;
    protected float health;

    public void Awake() {
        health = maxHealth;
    }
    
    public void Damage(float damage) {
        health -= damage;
    }
}
