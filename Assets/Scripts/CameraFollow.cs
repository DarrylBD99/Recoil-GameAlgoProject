using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector2 boundBox;

    // Update is called once per frame
    void Update() {
        if (target == null) return;

        Vector3 targetPos = target.position;
        Vector2 distance = transform.position - targetPos;
        Vector2 distanceAbs = new Vector2(Mathf.Abs(distance.x), Mathf.Abs(distance.y));

        Vector3 movePos = Vector2.zero;
        if (distanceAbs.x > boundBox.x) movePos.x = (boundBox.x - distanceAbs.x) * Mathf.Sign(distance.x);
        if (distanceAbs.y > boundBox.y) movePos.y = (boundBox.y - distanceAbs.y) * Mathf.Sign(distance.y);

        transform.position += movePos;
    }
}
