using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class RangerEnemy : EnemyBase
{
    private LinkedList<Node> _path;
    private Node _targetNode;
    private Node _currentTargetNode;

    protected new void Start()
    {
        base.Start();
    }

    protected new void Update()
    {
        base.Update();
    }

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void UpdateRotation()
    {
        Vector2 targetDir = Target.transform.position - transform.position;
        float rotation = (Mathf.Atan2(targetDir.y, targetDir.x) + Mathf.PI / 2) * Mathf.Rad2Deg;
        sprite.transform.rotation = Quaternion.AngleAxis(rotation, Vector3.forward);
    }

    protected override void UpdateMovement()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, Target.transform.position);

        if (distanceToPlayer <= entity.range)
        {
            moveDir = Vector3.zero; // Stop moving if too close
            return;
        }

        if (_path != null && _path.Count > 0)
        {
            Node nextNode = _path.Last();

            if (Vector3.Distance(nextNode.transform.position, transform.position) <= 0.1) {
                _currentTargetNode = nextNode;
                _path.RemoveLast();
            }

            moveDir = nextNode.transform.position - transform.position;
            moveDir.Normalize();
        }
        else
        {
            moveDir = Vector3.zero;
        }
    }

    public override bool CanSpawnInLocation(Vector3 location) {
        if (NodeGraph.instance.IsUnityNull()) return false;

        return !NodeGraph.PositionToNodePos(NodeGraph.instance, location).IsUnityNull();
    }

    protected override void UpdatePathfinding()
    {
        if (NodeGraph.instance != null)
        {
            Node targetNode = NodeGraph.PositionToNodePos(NodeGraph.instance, Target.transform.position);

            if (_currentTargetNode == null)
                _currentTargetNode = NodeGraph.PositionToNodePos(NodeGraph.instance, transform.position);

            if (_targetNode != targetNode || _path == null || _path.Count < 0)
            {
                _targetNode = targetNode;

                if (_currentTargetNode != null && _targetNode != null && _currentTargetNode != _targetNode)
                    _path = Dijkstra.GeneratePath(NodeGraph.instance, _currentTargetNode, _targetNode);
            }
        }
    }

    void OnDrawGizmos()
    {
        if (_path != null)
        {
            Gizmos.color = Color.red;
            Node previousNode = null;

            foreach (Node node in _path)
            {
                if (previousNode != null)
                    Gizmos.DrawLine(previousNode.transform.position, node.transform.position);

                previousNode = node;
            }
        }
    }
}
