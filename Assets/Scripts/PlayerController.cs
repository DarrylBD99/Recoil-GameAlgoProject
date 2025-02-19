using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Entity _entity;
    private GameObject _sprite;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        _entity = GetComponent<Entity>();
        _sprite = gameObject.transform.Find("Sprite").gameObject;
    }

    // Update is called once per frame
    void Update() {
        // Move player
        Vector3 moveDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        _entity.moveEntity(moveDir, Time.deltaTime);

        // Look at Mouse
        Vector3 mouseDir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        float angle = (Mathf.Atan2(mouseDir.y, mouseDir.x) + Mathf.PI/2) * Mathf.Rad2Deg;
        _sprite.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
