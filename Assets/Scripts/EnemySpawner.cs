using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject agilePrefab, rangerPrefab, tankPrefab;

    [SerializeField]
    private float minimumSpawnTime, maximumSpawnTime, rangerChance, tankChance;

    [SerializeField]
    private Tilemap groundTilemap;

    private float spawnTime;
    private float spawnFrequency = 0;
    public Transform minSpawnPoint, maxSpawnPoint;

    void Start() {
        ResetSpawnTimer();
        HUD.OnLevelUp += IncreaseFrequency;
    }

    void Update() {
        if (PlayerController.PlayerInstance.IsDestroyed()) return;

        spawnTime -= Time.deltaTime;
        if (spawnTime <= 0) {
            UnitController.Instance.IsUnityNull();

            SpawnEnemy(GetEnemyType());
            ResetSpawnTimer();
        }
    }

    void SpawnEnemy(GameObject enemyPrefab) {
        if (enemyPrefab.IsUnityNull()) {
            Debug.LogError("EnemyPrefab is missing or destroyed!");
            return;
        }

        CircleCollider2D collider = enemyPrefab.GetComponent<CircleCollider2D>();

        if (collider.IsUnityNull()) {
            Debug.LogError("EnemyPrefab is missing a collider!");
            return;
        }

        EnemyBase enemyBase = enemyPrefab.GetComponent<EnemyBase>();

        if (enemyBase == null) {
            Debug.LogError("Invalid Prefab for enemy: Missing EnemyBase!");
            return;
        }
        
        GameObject newEnemy = Instantiate(enemyPrefab, SelectSpawnPoint(collider.radius, enemyBase), Quaternion.identity);

        if (newEnemy.IsUnityNull()) {
            Debug.LogError("Failed to instantiate enemy!");
            return;
        }
    }

    private void ResetSpawnTimer() {
        spawnTime = Random.Range(minimumSpawnTime, maximumSpawnTime);
        spawnTime /= Mathf.Pow(2, spawnFrequency);
    }

    public Vector3 SelectSpawnPoint(float halfSize, EnemyBase enemyBase)
    {

        Vector3 spawnPoint = Vector3.zero;
        bool spawnVerticalEdge = Random.Range(0f, 1f) > 0.5;

        if (spawnVerticalEdge) {
            spawnPoint.y = Random.Range(minSpawnPoint.position.y, maxSpawnPoint.position.y);

            if (Random.Range(0f, 1f) > 0.5f)
                spawnPoint.x = maxSpawnPoint.position.x;
            else
                spawnPoint.x = minSpawnPoint.position.x;

        } else {
            spawnPoint.x = Random.Range(minSpawnPoint.position.x, maxSpawnPoint.position.x);

            if (Random.Range(0f, 1f) > 0.5f)
                spawnPoint.y = maxSpawnPoint.position.y;
            else
                spawnPoint.y = minSpawnPoint.position.y;
        }

        if (!groundTilemap.IsUnityNull()) {
            BoundsInt bounds = groundTilemap.cellBounds;
            
            if (!ValidPosition(spawnPoint, bounds, halfSize))
                return SelectSpawnPoint(halfSize, enemyBase);
        }

        if (enemyBase.CanSpawnInLocation(spawnPoint))
            return spawnPoint;

        return SelectSpawnPoint(halfSize, enemyBase);
    }

    private bool ValidPosition(Vector3 position, BoundsInt bounds, float halfSize) {
        if (position.x < bounds.x || position.x > bounds.xMax || position.y < bounds.y || position.y > bounds.yMax)
            return false;

        return Physics2D.OverlapBox(position, new Vector2(halfSize, halfSize) / 2, 0f, Entity.obstacleLayerMask).IsUnityNull();
    }

    void IncreaseFrequency() {
        spawnFrequency += 0.25f;
    }

    GameObject GetEnemyType() {


        if (HUD.Level >= 3 && Random.Range(1, rangerChance) == 1){
            Debug.Log("Ranger Type Spawned");
            return rangerPrefab;
        }
        
        if (HUD.Level >= 6 && Random.Range(1, tankChance) == 1){
            Debug.Log("Tank Type Spawned");
            return tankPrefab;
        }

        Debug.Log("Agile Type Spawned");
        return agilePrefab;
    }
}
