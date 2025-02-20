using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Entity _entity;
    private GameObject _sprite;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        _entity = GetComponent<Entity>();
        _sprite = gameObject.transform.Find("Sprite").gameObject;

        EnemyBase.Target = _entity;
    }

    // Update is called once per physics frame
    void FixedUpdate() {
        // Move player
        Vector3 moveDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        _entity.MoveEntityRigidbody(moveDir);

        // Look at Mouse
        Vector3 mouseDir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        float angle = (Mathf.Atan2(mouseDir.y, mouseDir.x) + Mathf.PI/2) * Mathf.Rad2Deg;
        _sprite.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
