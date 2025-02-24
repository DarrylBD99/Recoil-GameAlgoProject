using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NodeGraph: MonoBehaviour
{
    [SerializeField]
    public static NodeGraph instance;
    public Grid grid;
    public Tilemap groundTilemap;
    public GameObject nodePrefab;

    [NonSerialized]
    public Dictionary<Vector2Int, Node> nodes;

    // Directions
    public static int[] moveX = { -1, -1, -1, 0, 0, 1, 1, 1 };
    public static int[] moveY = { -1, 0, 1, -1, 1, -1, 0, 1 };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        if (transform.childCount <= 0)
            GenerateNodes();
    }

    public Node PositionToNodePos(Vector2 position) {
        Vector2 cellBounds = new Vector2(groundTilemap.cellBounds.x, groundTilemap.cellBounds.y);
        Vector2 cellSize = groundTilemap.cellSize;
        Vector2 anchor = groundTilemap.tileAnchor;

        position = (position - (cellBounds + anchor));

        Vector2Int nodePos = new((int)(position.x / cellSize.x), (int)(position.y / cellSize.y));

        nodes.TryGetValue(nodePos, out Node ret);

        return ret;
    }

    public void GenerateNodes() {
        // Clear children
        while (transform.childCount > 0)
            DestroyImmediate(transform.GetChild(0).gameObject);

        nodes = new();

        // Get all tiles in tilemap
        BoundsInt bounds = groundTilemap.cellBounds;
        TileBase[] tileArray = groundTilemap.GetTilesBlock(groundTilemap.cellBounds);
        
        Vector3 groundAnchor = groundTilemap.tileAnchor;
        Vector2 tilePos = Vector2.zero;
        
        for (int y = 0; y < bounds.size.y; y++) {
            for (int x = 0; x < bounds.size.x; x++) {
                TileBase tile = tileArray[x + y * bounds.size.x];
                CreateNode(tile, x, y, groundAnchor, bounds, groundTilemap.cellSize);
            }
        }

        GenerateConnections(bounds);
        instance = this;
    }

    // Creates Nodes in specific tiles
    public void CreateNode(TileBase tile, int x, int y, Vector3 anchor, BoundsInt bounds, Vector3 cellSize) {
        if (tile != null) {
            Vector3 nodePos = new Vector3(
                (x * cellSize.x) + bounds.x + anchor.x,
                (y * cellSize.y) + bounds.y + anchor.y,
                bounds.z + anchor.z
            );

            if (HasObstacle(nodePos, cellSize)) return;

            GameObject node = Instantiate(nodePrefab, nodePos, Quaternion.identity, transform);
            node.name = "Node_" + nodes.Count;
            Node nodeComponent = node.GetComponent<Node>();
            nodeComponent.x = x;
            nodeComponent.y = y;
            
            nodes.Add(new(x, y), nodeComponent);
        }
    }

    // Creates Connections between nodes
    public void GenerateConnections(BoundsInt bounds)
    {
        int[] dirs = { 1, 3, 4, 6 }; // (Left = 1, Down = 3, Up = 4, Right = 6)
        int[] dirsDiagonal = { 0, 2, 5, 7 }; // (BottomLeft = 0, TopLeft = 2, BottomRight = 5, TopRight = 7)
        Vector3 cellSize = groundTilemap.cellSize;

        foreach (Node node in nodes.Values)
        {
            // Check for neighbors in non-diagonal (cost: 1)
            foreach (int dir in dirs)
            {
                Node neighbor = HasConnection(this, node, dir, bounds);

                if (neighbor != null)
                    node.connections[neighbor] = 1;
            }

            // Check for neighbors in diagonals (cost: 2)
            foreach (int dir in dirsDiagonal)
            {
                Node neighbor = HasConnection(this, node, dir, bounds);
                if (neighbor != null) node.connections[neighbor] = 2;
            }
        }
    }

    // Check if obstacle collision is in specific tile given world position
    public static bool HasObstacle(Vector3 nodePos, Vector3 cellSize, float rotation = 0f) {
        return Physics2D.BoxCast(nodePos, cellSize / 2, rotation, Vector2.zero, 0f, LayerMask.GetMask("Obstacles"));
    }

    // Check if obstacle collision is in specific positon between 2 nodes given world positions
    //public static bool HasObstacleBetween(Vector3 startPos, Vector3 endPos, Vector3 cellSize, float rotation = 0f)
    //{
    //    Vector3 origin = startPos + ((endPos - startPos) / 2);
    //    return HasObstacle(origin, cellSize, rotation);
    //}

    // Check if neighbor of node in specified direction exists
    public static Node HasConnection(NodeGraph nodeGraph, Node currentNode, int direction, BoundsInt bounds) {
        int neighborX = currentNode.x + moveX[direction];
        int neighborY = currentNode.y + moveY[direction];

        if ((0 <= neighborX && neighborX < bounds.size.x) && (0 <= neighborY && neighborY < bounds.size.y)) {
            Node neighborNode = GetNode(nodeGraph, neighborX, neighborY);

            return neighborNode;
        }

        return null;
    }

    // Get Node from node position
    public static Node GetNode(NodeGraph nodeGraph, int x, int y) {
        foreach (Node node in nodeGraph.nodes.Values) {
            if (node.x == x && node.y == y) { return node; }
        }
        return null;
    }

    public static Node PositionToNodePos(NodeGraph nodeGraph, Vector2 position) {
        return nodeGraph.PositionToNodePos(position);
    }
}