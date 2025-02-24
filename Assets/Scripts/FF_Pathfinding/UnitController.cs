using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    public GridController gridController;
    public GameObject unitPrefab;
    public int numUnitsPerSpawn;
    public float moveSpeed;

    private List<GameObject> unitsInGame;

    private void Awake()
    {
        unitsInGame = new List<GameObject>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SpawnUnits();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            DestroyUnits();
        }
    }

    private void FixedUpdate()
    {
        if (gridController.curFlowField == null) { return; }

        foreach (GameObject unit in unitsInGame)
        {
            Cell cellBelow = gridController.curFlowField.GetCellFromWorldPos(unit.transform.position);
            if (cellBelow == null) { continue; } // Avoid errors if unit is out of bounds

            Vector2 moveDirection = new Vector2(cellBelow.bestDirection.Vector.x, cellBelow.bestDirection.Vector.y);

            Rigidbody2D unitRB = unit.GetComponent<Rigidbody2D>();
            if (unitRB != null)
            {
                unitRB.linearVelocity = moveDirection * moveSpeed;
            }

            Debug.Log($"Unit at {unit.transform.position}: Direction {cellBelow.bestDirection}");
        }
    }


    private void SpawnUnits()
    {
        Vector2Int gridSize = gridController.gridSize;
        float nodeRadius = gridController.cellRadius;
        Vector2 maxSpawnPos = new Vector2(gridSize.x * nodeRadius * 2 + nodeRadius, gridSize.y * nodeRadius * 2 + nodeRadius);
        int colMask = LayerMask.GetMask("Obstacles", "Entity");
        Vector3 newPos;
        for (int i = 0; i < numUnitsPerSpawn; i++)
        {
            GameObject newUnit = Instantiate(unitPrefab);
            newUnit.transform.parent = transform;
            unitsInGame.Add(newUnit);
            do
            {
                newPos = new Vector3(Random.Range(0, maxSpawnPos.x), Random.Range(0, maxSpawnPos.y), 0);
                newUnit.transform.position = newPos;
            }
            while (Physics.OverlapSphere(newPos, 0.25f, colMask).Length > 0);
        }
    }

    private void DestroyUnits()
    {
        foreach (GameObject go in unitsInGame)
        {
            Destroy(go);
        }
        unitsInGame.Clear();
    }
}