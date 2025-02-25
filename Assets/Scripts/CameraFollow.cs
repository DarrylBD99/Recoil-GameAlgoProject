using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public BoundsInt playerBoundBox;
    public BoxCollider2D cameraBounds;

    // Update is called once per frame
    void Update() {
        if (target == null) return;

        Vector3 targetPos = target.position;
        Vector2 distance = transform.position - targetPos;
        
        Vector3 movePos = Vector2.zero;

        if (distance.x < playerBoundBox.x) movePos.x = playerBoundBox.x - distance.x;
        else if (distance.x > playerBoundBox.xMax) movePos.x = playerBoundBox.xMax - distance.x;

        if (distance.y < playerBoundBox.y) movePos.y = playerBoundBox.y - distance.y;
        else if (distance.y > playerBoundBox.yMax) movePos.y = playerBoundBox.yMax - distance.y;

        transform.position += movePos;
    }
}
