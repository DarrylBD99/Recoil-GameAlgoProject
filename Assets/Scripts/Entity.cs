using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Entity : MonoBehaviour
{
    public delegate void IsDamaged();
    public delegate void IsDead();
    public delegate void Initialize();

    public event IsDamaged OnDamaged;
    public event IsDead OnDeath;
    public event Initialize OnInitialize;

    public float attack;
    public float speed;
    public float attackSpeed;
    public float maxHealth;

    [NonSerialized]
    public float health;

    private GameObject parent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        parent = transform.parent.gameObject;
        health = maxHealth;
        
        OnInitialize?.Invoke();
    }

    // Damages Entity
    public void damage(Entity source) {
        if (source != null) {
            damage(source.attack);
        }
    }

    // Damages Entity
    public void damage(float amount) {
        health -= amount;
        Debug.Log(parent.name + ": Hit (" + health + "/" + maxHealth + ")");
        OnDamaged?.Invoke();

        if (health <= 0) {
            Debug.Log(parent.name + ": Dead");
            OnDeath?.Invoke();
            Destroy(parent);
        }
    }

    // Move Entity
    public void moveEntity(Vector3 dir, float deltaTime) {
        parent.transform.position += dir * deltaTime * speed;
    }
}
