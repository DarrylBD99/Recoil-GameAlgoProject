using Unity.VisualScripting;
using UnityEngine;

public class Tank : EnemyBase
{
    private Cell _cellBelow;

    protected override void UpdateMovement() {
        if (!_cellBelow.IsUnityNull()) { // Prevent errors if unit is out of bounds
            moveDir = new Vector3(_cellBelow.bestDirection.Vector.x, _cellBelow.bestDirection.Vector.y);

            entity.MoveEntityRigidbody(moveDir);
        }
    }

    protected override void UpdatePathfinding() {
        if (!GridController.Instance.curFlowField.IsUnityNull())
            _cellBelow = GridController.Instance.curFlowField.GetCellFromWorldPos(transform.position);
    }

    protected override void UpdateRotation() {
        float rotation = (Mathf.Atan2(moveDir.y, moveDir.x) + Mathf.PI / 2) * Mathf.Rad2Deg;
        sprite.transform.rotation = Quaternion.AngleAxis(rotation, Vector3.forward);
    }
}
