using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AStar : MonoBehaviour
{
    public delegate ulong Heuristic(Node start, Node end);

    public static LinkedList<Node> GeneratePath(NodeGraph nodeGraph, Node start, Node end, Heuristic heuristic) {
        Dictionary<Node, AStarData> astarNodeDictionary= new();
        List<Node> astarNodes = new();

        foreach (Node node in nodeGraph.nodes.Values) {
            astarNodeDictionary.Add(node, new(ulong.MaxValue));
        }

        astarNodeDictionary[start].g = 0;
        astarNodeDictionary[start].h = heuristic(start, end);
        astarNodes.Add(start);

        while(astarNodes.Count > 0)
        {
            Node currentNode = astarNodes.First();

            // Get node with lowest weight (g + h)
            foreach (Node node in astarNodes)
                if (CalculateWeight(astarNodeDictionary[node]) < CalculateWeight(astarNodeDictionary[currentNode]))
                    currentNode = node;

            // Mark current node as visited
            astarNodes.Remove(currentNode);
            astarNodeDictionary[currentNode].visited = true;

            if (currentNode == end) {
                LinkedList<Node> path = new();

                path.AddFirst(end);

                while (currentNode != start) {
                    path.AddLast(currentNode);
                    currentNode = astarNodeDictionary[currentNode].parent;
                }

                return path;
            }

            foreach (Node neighbor in currentNode.connections.Keys) {
                ulong gNew = astarNodeDictionary[currentNode].g + currentNode.connections[neighbor];
                if (astarNodeDictionary[neighbor].g > gNew) {
                    astarNodeDictionary[neighbor].g = gNew;
                    astarNodeDictionary[neighbor].parent = currentNode;
                    astarNodeDictionary[neighbor].h = heuristic(neighbor, end);

                    if (!astarNodes.Contains(neighbor))
                        astarNodes.Add(neighbor);
                }
            }
        }
        return null;
    }

    public static ulong CalculateWeight(ulong g, ulong h) {
        return g + h;
    }

    public static ulong CalculateWeight(AStarData nodeData)
    {
        return CalculateWeight(nodeData.g, nodeData.h);
    }
}

public class AStarData
{
    public ulong g, h;
    public bool visited;
    public Node parent;

    public AStarData(ulong g) {
        this.g = g;
        parent = null;
        visited = false;
    }
}

public class AStarHeuristic
{
    public static ulong Manhattan(Node start, Node end)
    {
        int x = end.x - start.x;
        int y= end.y - start.y;
        return (ulong)(Math.Abs(x) + Math.Abs(y));
    }

    public static ulong EuclideanSquared(Node start, Node end)
    {
        int x = end.x - start.x;
        int y = end.y - start.y;
        return (ulong)(x * x + y * y);
    }
}