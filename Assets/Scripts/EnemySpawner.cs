using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private float minimumSpawnTime;

    [SerializeField]
    private float maximumSpawnTime;

    private float spawnTime;
    public Transform minSpawnPoint, maxSpawnPoint;

    private Transform target;

    private void Awake()
    {
        ResetSpawnTimer();
    }
    private void Start()
    {

    }

    void Update()
    {
        spawnTime -= Time.deltaTime;

        if (spawnTime <= 0)
        {
            Instantiate(enemyPrefab, SelectSpawnPoint(), transform.rotation);
            ResetSpawnTimer();
        }
    }

    private void ResetSpawnTimer()
    {
        spawnTime = Random.Range(minimumSpawnTime, maximumSpawnTime);
    }

    public Vector3 SelectSpawnPoint()
    {
        Vector3 spawnPoint = Vector3.zero;
        bool spawnVerticalEdge = Random.Range(0f, 1f) > 0.5;

        if (spawnVerticalEdge)
        {
            spawnPoint.y = Random.Range(minSpawnPoint.position.y, maxSpawnPoint.position.y);

            if (Random.Range(0f, 1f) > 0.5f)
            {
                spawnPoint.x = maxSpawnPoint.position.x;
            }
            else
            {
                spawnPoint.x = minSpawnPoint.position.x;
            }
        }
        else
        {
            spawnPoint.x = Random.Range(minSpawnPoint.position.x, maxSpawnPoint.position.x);

            if (Random.Range(0f, 1f) > 0.5f)
            {
                spawnPoint.y = maxSpawnPoint.position.y;
            }
            else
            {
                spawnPoint.y = minSpawnPoint.position.y;
            }
        }

        return spawnPoint;
    }
}
