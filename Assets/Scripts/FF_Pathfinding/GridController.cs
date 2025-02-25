using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridController : MonoBehaviour
{
    public static GridController Instance { get; private set; }
    public static Transform Player; // Reference to the player's Transform
    public Tilemap groundTilemap;
    public float cellRadius = 0.5f;

    [NonSerialized]
    public Vector2Int gridSize;
    public FlowField curFlowField;
    public GridDebug gridDebug;
    
    private Vector3 lastPlayerPosition; // Store the last known player position

    private void Start()
    {
        Player = PlayerController.PlayerInstance.transform;
        InitializeFlowField();
        Instance = this;
    }

    private void Update()
    {
        if (Player.IsUnityNull()) return;

        // Only update if the player has moved significantly
        if (Vector3.Distance(Player.position, lastPlayerPosition) > cellRadius * 0.5f)
            UpdateFlowField();
    }

    private void InitializeFlowField()
    {
        BoundsInt gridBounds = groundTilemap.cellBounds;
        gridSize = new(gridBounds.size.x, gridBounds.size.y);
        curFlowField = new FlowField(cellRadius, gridSize);
        curFlowField.CreateGrid(transform.position + gridBounds.position); // Passes GridController's position offset from the grid bounds
        gridDebug.SetFlowField(curFlowField);

        UpdateFlowField(); // initial update
    }

    private void UpdateFlowField()
    {
        curFlowField.CreateCostField();

        // Get the player's position dynamically
        Vector3 worldPlayerPos = Player.position;

        // Offset position by one grid cell upwards (assuming Y is up in 2D)
        worldPlayerPos.y += 1;

        // Convert world position to a valid grid cell
        Cell destinationCell = curFlowField.GetCellFromWorldPos(worldPlayerPos);

        if (destinationCell == null || destinationCell == curFlowField.destinationCell) {
            return; // Skip unnecessary updates
        }

        //Debug.Log($"New Destination (1 Grid Higher): {destinationCell.gridIndex}");

        curFlowField.CreateIntegrationField(destinationCell);
        curFlowField.CreateFlowField();

        gridDebug.DrawFlowField();

        lastPlayerPosition = Player.position; // Update the last known position
    }

}
