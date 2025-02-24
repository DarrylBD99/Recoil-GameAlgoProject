using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    public static Entity Target;
    protected GameObject sprite;
    protected Entity entity;

    // Pathfinding Variables
    public AStar.Heuristic aStarHeuristic = AStarHeuristic.Manhattan;
    
    protected Vector3 moveDir;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected void Start() {
        sprite = gameObject.transform.Find("Sprite").gameObject;
        entity = gameObject.GetComponent<Entity>();
        entity.OnDeath += OnDeath;
    }

    // Update is called once per frame
    protected void Update() {
        UpdatePathfinding();
        UpdateMovement();
        UpdateRotation();
    }

    // Update is called once per fixed frame
    protected void FixedUpdate() {
        // Move enemy
        entity.MoveEntityRigidbody(moveDir);

        if (moveDir.magnitude > 0f)
            if (!(entity.stepAudio == GetComponent<AudioSource>().clip && GetComponent<AudioSource>().isPlaying))
            {
                GetComponent<AudioSource>().clip = entity.stepAudio;
                GetComponent<AudioSource>().Play();
            }
    }

    // Give xp on death
    protected void OnDeath() {
        HUD.AddXP(entity.xp);
    }

    // Update Rotation of enemy to face target
    protected abstract void UpdateRotation();

    // Get velocity of enemy to next node
    protected abstract void UpdateMovement();

    // Get Path
    protected abstract void UpdatePathfinding();
}
