using System.Linq;
using UnityEngine;

public class AgileEnemy : EnemyBase
{
    private Node _targetNode;
    private Node _currentNode;

    protected override void UpdateRotation() {
        // Look at Target
        Vector2 targetDir = Target.transform.position - transform.position;
        float angle = (Mathf.Atan2(targetDir.y, targetDir.x) + Mathf.PI / 2) * Mathf.Rad2Deg;
        sprite.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    protected override void UpdateMovement() {
        if (path != null && path.Count > 0) {
            Node nextNode = path.Last();
            if (nextNode == _currentNode)
                path.RemoveLast();

            moveDir = nextNode.transform.position - transform.position;
        }
    }

    protected override void MovePosition() {
        entity.MoveEntityRigidbody(moveDir);
        _currentNode = NodeGraph.PositionToNodePos(NodeGraph.instance, transform.position);
    }

    protected override void UpdatePathfinding() {
        if (_currentNode == null)
            _currentNode = NodeGraph.PositionToNodePos(NodeGraph.instance, transform.position);

        Node targetNode = NodeGraph.instance.PositionToNodePos(Target.transform.position);

        if (_targetNode != targetNode) {
            _targetNode = targetNode;
            path = AStar.GeneratePath(NodeGraph.instance, _currentNode, targetNode, AStarHeuristic.Manhattan);
        }
    }
}
