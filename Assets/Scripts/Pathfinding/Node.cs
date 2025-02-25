using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public int x, y;

    public Dictionary<Node, ulong> connections = new();

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        foreach (Node neighbor in connections.Keys){
            Gizmos.DrawLine(transform.position, neighbor.transform.position);
        }
    }
}
