using UnityEngine;

public class Bullet : MonoBehaviour
{
    public AudioClip shootSfx;
    public Entity source;
    public Vector3 velocity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        AudioManager.PlaySoundEffect(shootSfx);
    }

    // Update is called once per frame
    void Update() {

    }

    // Fixed Update is called once per frame
    void FixedUpdate() {
        Vector3 newPos = transform.position + velocity * Time.fixedDeltaTime;

        RaycastHit2D[] hits = Physics2D.LinecastAll(transform.position, newPos);

        foreach (RaycastHit2D hit in hits)
        {
            GameObject obj = hit.collider.gameObject;
            if (obj.layer == LayerMask.NameToLayer("Obstacles")) Destroy(gameObject);
            
            if (obj.layer == LayerMask.NameToLayer("Entity")) {
                Entity hitEntity = obj.GetComponent<Entity>();
                if (hitEntity != null && hitEntity != source) {
                    hitEntity.damage(source.attack);
                    Destroy(gameObject);
                }
            }
        }

        GetComponent<Rigidbody2D>().MovePosition(newPos);
    }
}
