using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    public GameObject unitPrefab;
    public int numUnitsPerSpawn;
    //public float moveSpeed;

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

    //private void FixedUpdate()
    //{
    //    if (GridController.Instance.curFlowField == null) { return; }

    //    foreach (GameObject unit in unitsInGame)
    //    {
    //        Entity entity = unit.GetComponent<Entity>();
    //        if (entity == null) { continue; } // Ensure the unit has an Entity component

    //        Cell cellBelow = GridController.Instance.curFlowField.GetCellFromWorldPos(unit.transform.position);
    //        if (cellBelow == null) { continue; } // Prevent errors if unit is out of bounds

    //        Vector2 moveDirection = new Vector2(cellBelow.bestDirection.Vector.x, cellBelow.bestDirection.Vector.y);
    //        if (moveDirection == Vector2.zero) { continue; } // Skip if there's no movement direction

    //        Rigidbody2D unitRB = unit.GetComponent<Rigidbody2D>();
    //        if (unitRB != null)
    //        {
    //            unitRB.linearVelocity = moveDirection * entity.speed; // Use Entity's movement speed

    //            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
    //            unit.transform.rotation = Quaternion.Euler(0, 0, angle + 90);
    //        }
    //    }
    //}

    private void SpawnUnits()
    {
        Vector2Int gridSize = GridController.Instance.gridSize;
        float nodeRadius = GridController.Instance.cellRadius;
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