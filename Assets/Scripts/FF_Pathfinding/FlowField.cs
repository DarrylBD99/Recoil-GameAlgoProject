using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowField
{
    public Cell[,] grid { get; private set; }
    public Vector2Int gridSize { get; private set; }
    public float cellRadius { get; private set; }

    private float cellDiameter;

    public Cell destinationCell;

    GridController gridController;

    GridDebug gridDebug;


    public FlowField(float _cellRadius, Vector2Int _gridSize)
    {
        cellRadius = _cellRadius;
        cellDiameter = cellRadius * 2f;
        gridSize = _gridSize;
    }

    public void CreateGrid(Vector3 startPosition)
    {
        grid = new Cell[gridSize.x, gridSize.y];

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector3 worldPos = new Vector3(
                    startPosition.x + (cellDiameter * x + cellRadius),
                    startPosition.y + (cellDiameter * y + cellRadius),
                    0
                );

                grid[x, y] = new Cell(worldPos, new Vector2Int(x, y));
            }
        }
    }


    public void CreateCostField()
    {
        Vector2 cellHalfExtents = Vector2.one * cellRadius;
        int terrainMask = LayerMask.GetMask("Obstacles");
        foreach (Cell curCell in grid)
        {
            Collider2D[] obstacles = Physics2D.OverlapBoxAll(curCell.worldPos, cellHalfExtents, 0f, terrainMask);
            bool hasIncreasedCost = false;
            foreach (Collider2D col in obstacles)
            {
                if (col.gameObject.layer <= 6)
                {
                    curCell.IncreaseCost(255);
                    continue;
                }
                else if (!hasIncreasedCost && col.gameObject.layer == 9)
                {
                    curCell.IncreaseCost(3);
                    hasIncreasedCost = true;
                }
                
            }
        }
    }

    public void CreateIntegrationField(Cell _destinationCell)
    {
        destinationCell = _destinationCell;

        // Reset only bestCost, NOT terrain cost
        foreach (Cell cell in grid)
        {
            cell.bestCost = ushort.MaxValue; // Reset pathfinding values
        }

        // Set destination bestCost but DO NOT modify terrain cost
        destinationCell.bestCost = 0;

        Queue<Cell> cellsToCheck = new Queue<Cell>();
        cellsToCheck.Enqueue(destinationCell);

        while (cellsToCheck.Count > 0)
        {
            Cell curCell = cellsToCheck.Dequeue();
            List<Cell> curNeighbors = GetNeighborCells(curCell.gridIndex, GridDirection.CardinalDirections);

            foreach (Cell curNeighbor in curNeighbors)
            {
                if (curNeighbor.cost == byte.MaxValue) continue; // Skip impassable obstacles

                // Correct cost propagation: bestCost is terrain cost + previous bestCost
                ushort newCost = (ushort)(curCell.bestCost + curNeighbor.cost);

                if (newCost < curNeighbor.bestCost)
                {
                    curNeighbor.bestCost = newCost;
                    cellsToCheck.Enqueue(curNeighbor);
                }
            }
        }
    }



    public void CreateFlowField()
    {
        foreach (Cell curCell in grid)
        {
            if (curCell == destinationCell)
            {
                curCell.bestDirection = GridDirection.None; // No movement at the destination
                continue;
            }

            List<Cell> curNeighbors = GetNeighborCells(curCell.gridIndex, GridDirection.AllDirections);
            int bestCost = curCell.bestCost;
            Cell bestNeighbor = null;

            foreach (Cell curNeighbor in curNeighbors)
            {
                if (curNeighbor.bestCost < bestCost)
                {
                    bestCost = curNeighbor.bestCost;
                    bestNeighbor = curNeighbor;
                }
            }

            if (bestNeighbor != null)
            {
                curCell.bestDirection = GridDirection.GetDirectionFromV2I(bestNeighbor.gridIndex - curCell.gridIndex);
            }
        }
    }

    private List<Cell> GetNeighborCells(Vector2Int nodeIndex, List<GridDirection> directions)
    {
        List<Cell> neighborCells = new List<Cell>();

        foreach (Vector2Int curDirection in directions)
        {
            Cell newNeighbor = GetCellAtRelativePos(nodeIndex, curDirection);
            if (newNeighbor != null)
            {
                neighborCells.Add(newNeighbor);
            }
        }
        return neighborCells;
    }

    private Cell GetCellAtRelativePos(Vector2Int orignPos, Vector2Int relativePos)
    {
        Vector2Int finalPos = orignPos + relativePos;

        if (finalPos.x < 0 || finalPos.x >= gridSize.x || finalPos.y < 0 || finalPos.y >= gridSize.y)
        {
            return null;
        }

        else { return grid[finalPos.x, finalPos.y]; }
    }

    public Cell GetCellFromWorldPos(Vector3 worldPos)
    {
        // Convert world position relative to the grid's starting position
        Vector3 relativePos = worldPos - grid[0, 0].worldPos;

        int x = Mathf.Clamp(Mathf.FloorToInt(relativePos.x / cellDiameter), 0, gridSize.x - 1);
        int y = Mathf.Clamp(Mathf.FloorToInt(relativePos.y / cellDiameter), 0, gridSize.y - 1);

        return grid[x, y];
    }

}