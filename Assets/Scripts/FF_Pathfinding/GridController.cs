using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public Vector2Int gridSize;
    public float cellRadius = 0.5f;
    public FlowField curFlowField;
    public GridDebug gridDebug;
    public Transform player; // Reference to the player's Transform

    private Vector3 lastPlayerPosition; // Store the last known player position

    private void Start()
    {
        InitializeFlowField();
        lastPlayerPosition = player.position; // Save the initial position
    }

    private void Update()
    {
        if (player == null) return;

        // Only update if the player has moved significantly
        if (Vector3.Distance(player.position, lastPlayerPosition) > cellRadius * 0.5f)
        {
            UpdateFlowField();
            lastPlayerPosition = player.position; // Update the last known position
        }
    }

    private void InitializeFlowField()
    {
        curFlowField = new FlowField(cellRadius, gridSize);
        curFlowField.CreateGrid(transform.position); // Passes GridController's position
        gridDebug.SetFlowField(curFlowField);
    }

    private void UpdateFlowField()
    {
        curFlowField.CreateCostField();

        // Get the player's position dynamically
        Vector3 worldPlayerPos = player.position;

        // Convert world position to a valid grid cell
        Cell destinationCell = curFlowField.GetCellFromWorldPos(worldPlayerPos);

        if (destinationCell == null || destinationCell == curFlowField.destinationCell)
        {
            return; // Skip unnecessary updates
        }

        Debug.Log($"New Destination: {destinationCell.gridIndex}");

        curFlowField.CreateIntegrationField(destinationCell);
        curFlowField.CreateFlowField();

        gridDebug.DrawFlowField();
    }
}
