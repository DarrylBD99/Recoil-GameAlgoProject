using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    public static Entity Target;
    protected GameObject sprite;
    protected Entity entity;
    protected Vector3 moveDir;

    protected LinkedList<Node> path;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        sprite = gameObject.transform.Find("Sprite").gameObject;
        entity = gameObject.GetComponent<Entity>();

        moveDir = Vector3.zero;
    }

    // Update is called once per frame
    void Update() {
        UpdatePathfinding();
        UpdateMovement();
        UpdateRotation();
    }

    void FixedUpdate() {
        MovePosition();
    }

    protected abstract void MovePosition();

    protected abstract void UpdateRotation();

    protected abstract void UpdatePathfinding();

    protected abstract void UpdateMovement();
}
