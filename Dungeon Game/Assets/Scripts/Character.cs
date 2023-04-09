using Interfaces;
using UnityEngine;

public class Character : MonoBehaviour, IDamageable {
    [SerializeField] protected int maxHealth;
    [SerializeField] protected float speed = 275f;
    protected int health;

    public void Damage() {
        throw new System.NotImplementedException();
    }
}
