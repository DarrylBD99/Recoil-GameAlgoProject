using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Dijkstra : MonoBehaviour
{
    public static LinkedList<Node> GeneratePath(NodeGraph nodeGraph, Node start, Node end)
    {
        Dictionary<Node, DijkstraData> dijkstraNodeDictionary = new();
        List<Node> dijkstraNodes = new();

        foreach (Node node in nodeGraph.nodes.Values)
        {
            dijkstraNodeDictionary.Add(node, new DijkstraData(ulong.MaxValue));
        }

        dijkstraNodeDictionary[start].g = 0;
        dijkstraNodes.Add(start);

        while (dijkstraNodes.Count > 0)
        {
            Node currentNode = dijkstraNodes.First();

            // Get node with the lowest g (cost)
            foreach (Node node in dijkstraNodes)
                if (dijkstraNodeDictionary[node].g < dijkstraNodeDictionary[currentNode].g)
                    currentNode = node;

            // Mark current node as visited
            dijkstraNodes.Remove(currentNode);
            dijkstraNodeDictionary[currentNode].visited = true;

            if (currentNode == end)
            {
                LinkedList<Node> path = new();
                path.AddFirst(end);

                while (currentNode != start)
                {
                    path.AddLast(currentNode);
                    currentNode = dijkstraNodeDictionary[currentNode].parent;
                }

                return path;
            }

            foreach (Node neighbor in currentNode.connections.Keys)
            {
                ulong gNew = dijkstraNodeDictionary[currentNode].g + currentNode.connections[neighbor];
                if (dijkstraNodeDictionary[neighbor].g > gNew)
                {
                    dijkstraNodeDictionary[neighbor].g = gNew;
                    dijkstraNodeDictionary[neighbor].parent = currentNode;

                    if (!dijkstraNodes.Contains(neighbor))
                        dijkstraNodes.Add(neighbor);
                }
            }
        }
        return null;
    }
}

public class DijkstraData
{
    public ulong g;
    public bool visited;
    public Node parent;

    public DijkstraData(ulong g)
    {
        this.g = g;
        parent = null;
        visited = false;
    }
}
