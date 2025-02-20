using UnityEngine;

public class AgileEnemy : EnemyBase
{
    protected override void UpdateRotation() {
        // Look at Target
        Vector2 targetDir = Target.transform.position - transform.position;
        float angle = (Mathf.Atan2(targetDir.y, targetDir.x) + Mathf.PI / 2) * Mathf.Rad2Deg;
        sprite.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    protected override void UpdateMovement() {
        return;
    }

    protected override void UpdatePathfinding() {
        return;
    }
}
