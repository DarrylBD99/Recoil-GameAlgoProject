using UnityEngine;
using UnityEngine.UI;

public class EntityCanvas : MonoBehaviour
{
    [SerializeField]
    protected Slider healthBar;

    private Entity target;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        // Get Entity
        GameObject parent = transform.parent.gameObject;
        target = parent.GetComponentInChildren<Entity>();

        // Setup health bar
        healthBar.maxValue = target.maxHealth;

        // Link signals
        target.OnInitialize += UpdateHealth;
        target.OnDamaged += UpdateHealth;
    }

    // Update Health Bar when signals are called
    void UpdateHealth() {
        healthBar.value = target.health;
    }
}
