using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public BoundsInt boundBox;

    // Update is called once per frame
    void Update() {
        if (target == null) return;

        Vector3 targetPos = target.position;
        Vector2 distance = transform.position - targetPos;
        
        Vector3 movePos = Vector2.zero;

        if (distance.x < boundBox.x) movePos.x = boundBox.x - distance.x;
        else if (distance.x > boundBox.xMax) movePos.x = boundBox.xMax - distance.x;

        if (distance.y < boundBox.y) movePos.y = boundBox.y - distance.y;
        else if (distance.y > boundBox.yMax) movePos.y = boundBox.yMax - distance.y;

        transform.position += movePos;
    }
}
