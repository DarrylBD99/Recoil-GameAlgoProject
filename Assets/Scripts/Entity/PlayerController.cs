using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Entity _entity;
    private GameObject _sprite;
    private Vector3 _mouseDir;
    private float _rotation;
    private bool _isCooldown = false;
    private float _cooldown = 0f;
    private bool _isGrappleColliding = false;

    public Transform bulletStart;
    public InputActionReference attackAction;
    public static Entity PlayerInstance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        _entity = GetComponent<Entity>();
        _sprite = gameObject.transform.Find("Sprite").gameObject;
        attackAction.action.started += SpawnBullet;

        EnemyBase.Target = _entity;
        PlayerInstance = _entity;
    }

    // Spawn bullet when attack action is pressed
    void SpawnBullet(InputAction.CallbackContext context) {
        if (PlayerInstance.IsUnityNull()) return;

        if (!_isCooldown){
            _entity.SpawnBullet(_mouseDir, bulletStart.position, _rotation);
            _isCooldown = true;
        }
    }

    // Update is called once per frame
    void Update() {
        if (_isCooldown) {
            _cooldown += Time.deltaTime;
            if (_cooldown >= _entity.attackCooldown){
                _isCooldown = false;
                _cooldown = 0f;
            }
        }
    }

    // Update is called once per physics frame
    void FixedUpdate() {
        // Move player
        Vector3 moveDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (_isGrappleColliding)
        {
            moveDir = Vector3.zero;
        }

        _entity.MoveEntityRigidbody(moveDir);

        // Look at Mouse
        _mouseDir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        _rotation = (Mathf.Atan2(_mouseDir.y, _mouseDir.x) + Mathf.PI/2) * Mathf.Rad2Deg;
        _sprite.transform.rotation = Quaternion.AngleAxis(_rotation, Vector3.forward);
    }

    // Detect collision with "Grapple" layer objects
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Grapple"))
        {
            _isGrappleColliding = true;
        }
    }

    // Reset movement when leaving "Grapple" objects
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Grapple"))
        {
            _isGrappleColliding = false;
        }
    }
}
