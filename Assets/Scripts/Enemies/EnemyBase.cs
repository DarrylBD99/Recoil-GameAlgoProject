using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyBase : MonoBehaviour
{
    public static Entity Target;
    protected GameObject sprite;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        sprite = gameObject.transform.Find("Sprite").gameObject;
    }

    // Update is called once per frame
    void Update() {
        UpdatePathfinding();
        UpdateMovement();
        UpdateRotation();
    }

    protected abstract void UpdateRotation();

    protected abstract void UpdatePathfinding();

    protected abstract void UpdateMovement();
}
