using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AStar : MonoBehaviour
{
    public delegate ulong Heuristic(Node start, Node end);

    public static LinkedList<Node> GeneratePath(NodeGraph nodeGraph, Node start, Node end, Heuristic heuristic) {
        List<Node> astarNodes = new();

        foreach (Node node in nodeGraph.nodes.Values) {
            node.g = ulong.MaxValue;
            node.visited = false;
            node.parent = null;
        }

        start.g = 0;
        start.h = heuristic(start, end);
        astarNodes.Add(start);

        while(astarNodes.Count > 0)
        {
            Node currentNode = astarNodes.First();

            // Get node with lowest weight (g + h)
            foreach (Node node in astarNodes)
                if (CalculateWeight(node) < CalculateWeight(currentNode))
                    currentNode = node;
            
            // Mark current node as visited
            astarNodes.Remove(currentNode);
            currentNode.visited = true;

            if (currentNode == end) {
                LinkedList<Node> path = new();

                path.AddFirst(end);

                while (currentNode != start) {
                    path.AddLast(currentNode);
                    currentNode = currentNode.parent;
                }

                return path;
            }

            foreach (Node neighbor in currentNode.connections.Keys) {
                // ignore visited neighbors
                if (neighbor.visited) continue;

                ulong gNew = currentNode.g + currentNode.connections[neighbor];
                if (neighbor.g > gNew) {
                    neighbor.g = gNew;
                    neighbor.parent = currentNode;
                    neighbor.h = heuristic(neighbor, end);

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

    public static ulong CalculateWeight(Node node)
    {
        return CalculateWeight(node.g, node.h);
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