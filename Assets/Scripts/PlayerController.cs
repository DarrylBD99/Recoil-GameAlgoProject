using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float _speed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        Entity entity = GetComponent<Entity>();
        _speed = entity.speed;
        entity.OnDamaged += DamageFlash;
    }

    // Update is called once per fixed frame
    void FixedUpdate() {
        // Move player
        Vector3 velocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        transform.position += velocity * Time.fixedDeltaTime * _speed;
    }

    void DamageFlash() {
        GetComponent<SpriteRenderer>().color = Color.red;
    }
}
