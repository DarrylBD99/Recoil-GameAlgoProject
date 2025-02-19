using System;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField]
    Transform target;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        UpdatePathfinding();
        UpdateMovement();
        UpdateRotation();
    }

    private void UpdateRotation() {
        // Look at Target
        float angle = (Mathf.Atan2(target.position.y, target.position.x) + Mathf.PI / 2) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    protected void UpdatePathfinding() {

    }

    protected void UpdateMovement() {

    }
}
