using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class AgileEnemy : EnemyBase
{
    public GameObject dagger;
    public Transform daggerDamageTip;
    public Transform daggerDamageBottom;

    private bool _damagedTarget;
    private bool _cooldownBool;
    private float _cooldown;

    // A* Pathfinding
    private LinkedList<Node> _path;
    private Node _targetNode;
    private Node _currentTargetNode;

    // States for Agile Enemy
    protected enum State {
        Follow,
        Attack
    }

    private State _currentState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected new void Start() {
        _currentState = State.Follow;
        _damagedTarget = false;
        _cooldownBool = false;
        _cooldown = 0f;
        entity.OnDeath += OnDeath;

        dagger.SetActive(false);

        base.Start();
    }

    // Hotfix to ensure currentTargetNode's occupiedEntity is set to null before being destroyed
    private void OnDeath() {
        _currentTargetNode.occupiedEntity = null;
    }

    // Update is called once per frame
    protected new void Update() {
        if (_currentState == State.Follow) {
            if (Vector3.Distance(transform.position, Target.transform.position) <= entity.range && !_cooldownBool) {
                _currentState = State.Attack;
                StartCoroutine("Attack");
            }

            base.Update();
        }
        if (_cooldownBool) {
            _cooldown += Time.deltaTime;
            if (_cooldown >= entity.attackCooldown) {
                _cooldown = 0f;
                _cooldownBool = false;
            }
        }
    }

    // Update is called once per fixed frame
    protected new void FixedUpdate() {
        if (_currentState == State.Follow)
            base.FixedUpdate();
    }

    // Agile swings dagger to attack
    IEnumerator Attack() {
        dagger.transform.rotation = sprite.transform.rotation;
        dagger.transform.Rotate(new(0, 0, -50));
        dagger.SetActive(true);

        Vector3 daggerPosPreviousTip = daggerDamageTip.position;
        Vector3 daggerPosPreviousBottom= daggerDamageBottom.position;

        float attackTime = entity.attackSpeed;
        for (float x = 0.0f; x <= attackTime; x += Time.deltaTime) {
            dagger.transform.Rotate(new(0, 0, (attackTime - x) * 3));

            // Check if player is within dagger
            if (!_damagedTarget) {
                RaycastHit2D[] tipDamage = Physics2D.LinecastAll(daggerPosPreviousTip, daggerDamageTip.position);
                RaycastHit2D[] bottomDamage = Physics2D.LinecastAll(daggerPosPreviousBottom, daggerDamageBottom.position);

                RaycastHit2D[] hitObjects = tipDamage.Concat(bottomDamage).ToArray();

                foreach (RaycastHit2D hitObject in hitObjects)
                    if (hitObject.collider.gameObject.GetComponent<Entity>() == Target) {
                        Target.damage(entity.attack);
                        _damagedTarget = true;
                        break;
                    }
            }
            yield return null;
        }
        _currentState = State.Follow;
        _cooldownBool = true;
        dagger.SetActive(false);
        _damagedTarget = false;
    }

    // Update Rotation of enemy to face target
    protected override void UpdateRotation() {
        // Look at Target
        Vector2 targetDir = Target.transform.position - transform.position;
        float rotation = (Mathf.Atan2(targetDir.y, targetDir.x) + Mathf.PI / 2) * Mathf.Rad2Deg;
        sprite.transform.rotation = Quaternion.AngleAxis(rotation, Vector3.forward);
    }

    // Get velocity of enemy to next node
    protected override void UpdateMovement() {
        if (!_path.IsUnityNull() && _path.Count > 0) {
            Node nextNode = _path.Last();

            if (Vector3.Distance(nextNode.transform.position, transform.position) <= 0.1) {
                //_currentTargetNode.occupiedEntity = null;
                _currentTargetNode = nextNode;
                //_currentTargetNode.occupiedEntity = entity;
                _path.RemoveLast();
            }

            if (!(_currentTargetNode.occupiedEntity.IsUnityNull() || _currentTargetNode.occupiedEntity == entity)) {
                _path = null;
                return;
            }

            moveDir = nextNode.transform.position - transform.position;
            moveDir.Normalize();
        } else {
            moveDir = Vector3.zero;
        }
    }

    // Get A* Path
    protected override void UpdatePathfinding() {
        if (!NodeGraph.instance.IsUnityNull()) {
            Node targetNode = NodeGraph.PositionToNodePos(NodeGraph.instance, Target.transform.position);

            if (_currentTargetNode.IsUnityNull())
                _currentTargetNode = NodeGraph.PositionToNodePos(NodeGraph.instance, transform.position);

            if (_targetNode != targetNode || _path == null || _path.Count < 0) {
                _targetNode = targetNode;

                if (!_currentTargetNode.IsUnityNull() && !_targetNode.IsUnityNull() && _currentTargetNode != _targetNode)
                    _path = AStar.GeneratePath(NodeGraph.instance, _currentTargetNode, _targetNode, aStarHeuristic);
            }
        }
    }

    // Draws A* Path
    void OnDrawGizmos() {
        if (!_path.IsUnityNull()) {
            Gizmos.color = Color.blue;
            Node previousNode = null;

            foreach (Node node in _path) {
                if (previousNode != null)
                    Gizmos.DrawLine(previousNode.transform.position, node.transform.position);

                previousNode = node;
            }
        }
    }
}
