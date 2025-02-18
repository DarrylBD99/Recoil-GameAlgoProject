using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public delegate void IsDamaged();
    public event IsDamaged OnDamaged;

    public float attack;
    public float speed;
    public float attackSpeed;
    public float maxHealth;

    protected float health;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        health = maxHealth;
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
        //Debug.Log(name + ": Hit (" + health + "/" + maxHealth + ")");
        OnDamaged?.Invoke();

        if (health <= 0) {
            //Debug.Log(name + ": Dead");
            Destroy(gameObject);
        }
    }
}
