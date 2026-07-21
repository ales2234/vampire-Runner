using UnityEngine;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] obstaclesPrefabs;
    public float spawnRate = 2f;
    private float timerUntilSpawn;
    public float spawnForce = 1f;

    private readonly List<GameObject> spawnedObstacles = new List<GameObject>();

    private void Start()
    {
        GameManager.Instance.onPlay.AddListener(ResetSpawner);
    }

    private void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.isGameOver)
            SpawnLoop();
    }

    private void ResetSpawner()
    {
        timerUntilSpawn = 0f;
        ClearSpawnedObstacles();
    }

    private void ClearSpawnedObstacles()
    {
        for (int i = spawnedObstacles.Count - 1; i >= 0; i--)
        {
            if (spawnedObstacles[i] != null)
                Destroy(spawnedObstacles[i]);
        }

        spawnedObstacles.Clear();
    }

    private void SpawnLoop()
    {
        timerUntilSpawn += Time.deltaTime;

        if (timerUntilSpawn >= spawnRate)
        {
            SpawnObstacle();
            timerUntilSpawn = 0f;
        }
    }

    private void SpawnObstacle()
    {
        if (obstaclesPrefabs == null || obstaclesPrefabs.Length == 0)
            return;

        GameObject obstacleToSpawn = obstaclesPrefabs[Random.Range(0, obstaclesPrefabs.Length)];
        GameObject newObstacle = Instantiate(obstacleToSpawn, transform.position, Quaternion.identity);

        spawnedObstacles.Add(newObstacle);

        Rigidbody2D obstacleRigidbody = newObstacle.GetComponent<Rigidbody2D>();
        if (obstacleRigidbody != null)
            obstacleRigidbody.linearVelocity = Vector2.left * spawnForce;
    }
}
