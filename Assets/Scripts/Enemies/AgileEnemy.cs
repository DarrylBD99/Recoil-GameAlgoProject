using System.Linq;
using Unity.VisualScripting;
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

            if (Vector3.Distance(nextNode.transform.position, transform.position) <= 0.1) {
                _currentNode = nextNode;
                path.RemoveLast();
            }

            moveDir = nextNode.transform.position - transform.position;
            moveDir.Normalize();
        }
    }

    protected override void MovePosition() {
        entity.MoveEntityRigidbody(moveDir);
    }

    protected override void UpdatePathfinding() {
        Node targetNode = NodeGraph.PositionToNodePos(NodeGraph.instance, Target.transform.position);

        if (_currentNode == null)
            _currentNode = NodeGraph.PositionToNodePos(NodeGraph.instance, transform.position);

        if (_targetNode != targetNode) {
            _targetNode = targetNode;

            if (_currentNode != _targetNode)
                path = AStar.GeneratePath(NodeGraph.instance, _currentNode, _targetNode, AStarHeuristic.Manhattan);
        }
    }

    // Draws A* Path
    //void OnDrawGizmos()
    //{
    //    if (!path.IsUnityNull())
    //    {
    //        Gizmos.color = Color.blue;
    //        Node previousNode = null;
    //        foreach (Node node in path)
    //        {
    //            if (previousNode != null)
    //                Gizmos.DrawLine(previousNode.transform.position, node.transform.position);

    //            previousNode = node;
    //        }

    //    }
    //}
}
