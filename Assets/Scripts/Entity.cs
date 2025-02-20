using System;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public delegate void IsDamaged();
    public delegate void IsDead();

    public event IsDamaged OnDamaged;
    public event IsDead OnDeath;

    public float attack;
    public float speed;
    public float attackSpeed;
    public float maxHealth;

    [NonSerialized]
    public float health;

    private Rigidbody2D _rigidBody;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        health = maxHealth;
        _rigidBody = GetComponent<Rigidbody2D>();
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
        Debug.Log(name + ": Hit (" + health + "/" + maxHealth + ")");
        OnDamaged?.Invoke();

        if (health <= 0) {
            Debug.Log(name + ": Dead");
            OnDeath?.Invoke();
            Destroy(gameObject);
        }
    }

    // Move entity by direction
    public void MoveEntityRigidbody(Vector3 moveDir) {
        if (Mathf.Sqrt(Mathf.Pow(moveDir.x, 2) + Mathf.Pow(moveDir.y, 2)) > 1f)
            moveDir.Normalize();

        Vector2 newPos = transform.position + moveDir * Time.fixedDeltaTime * speed;

        _rigidBody.MovePosition(newPos);
    }
}
