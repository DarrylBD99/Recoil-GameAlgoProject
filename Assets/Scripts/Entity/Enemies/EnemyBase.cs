using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    public static Entity Target;
    protected GameObject sprite;
    protected Entity entity;

    // Pathfinding Variables
    public AStar.Heuristic aStarHeuristic = AStarHeuristic.Manhattan;
    
    private LinkedList<Node> _path;
    private Vector3 _moveDir;
    private Node _targetNode;
    private Node _currentTargetNode;

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
        MovePosition();
    }

    // Hotfix to ensure currentTargetNode's occupiedEntity is set to null before being destroyed
    void OnDeath() {
        _currentTargetNode.occupiedEntity = null;
    }

    // Update Rotation of enemy to face target
    private void UpdateRotation() {
        // Look at Target
        Vector2 targetDir = Target.transform.position - transform.position;
        float rotation = (Mathf.Atan2(targetDir.y, targetDir.x) + Mathf.PI / 2) * Mathf.Rad2Deg;
        sprite.transform.rotation = Quaternion.AngleAxis(rotation, Vector3.forward);
    }

    // Get velocity of enemy to next node
    private void UpdateMovement() {
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

            _moveDir = nextNode.transform.position - transform.position;
            _moveDir.Normalize();
        } else {
            _moveDir = Vector3.zero;
        }
    }

    // Move enemy to next node in path
    private void MovePosition() {
        entity.MoveEntityRigidbody(_moveDir);

        if (_moveDir.magnitude > 0f)
            if (!(entity.stepAudio == GetComponent<AudioSource>().clip && GetComponent<AudioSource>().isPlaying)) {
                GetComponent<AudioSource>().clip = entity.stepAudio;
                GetComponent<AudioSource>().Play();
            }
    }

    // Get A* Path
    private void UpdatePathfinding() {
        Node targetNode = NodeGraph.PositionToNodePos(NodeGraph.instance, Target.transform.position);

        if (_currentTargetNode.IsUnityNull())
            _currentTargetNode = NodeGraph.PositionToNodePos(NodeGraph.instance, transform.position);

        if (_targetNode != targetNode || _path == null || _path.Count < 0) {
            _targetNode = targetNode;

            if (!_currentTargetNode.IsUnityNull() && !_targetNode.IsUnityNull() && _currentTargetNode != _targetNode)
                _path = AStar.GeneratePath(NodeGraph.instance, _currentTargetNode, _targetNode, aStarHeuristic);
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
