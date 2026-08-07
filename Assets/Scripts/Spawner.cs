using UnityEngine;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    [System.Serializable]
    public class SpawnEntry
    {
        public GameObject prefab;
        [Tooltip("Relative chance. Example: 70 and 30 = 70% / 30%. Values do not need to add to 100.")]
        [Range(0f, 100f)] public float chance = 25f;
    }

    [SerializeField] private SpawnEntry[] spawnEntries;
    [SerializeField] private Transform obstacleParent;

    public float spawnRate = 3f;
    [Range(0f, 1f)] public float spawnRateFactor = 0.5f;
    public float spawnForce = 1f;
    [Range(0f, 1f)] public float spawnForceFactor = 0.5f;
    [SerializeField] private float minSpawnInterval = 0.45f;
    [SerializeField] private float maxSpawnForce = 18f;
    [SerializeField] private int maxAliveObstacles = 25;
    [SerializeField] private float despawnX = -20f;

    private float timerUntilSpawn;

    public float _spawnRate;
    public float _spawnForce;
    public float TimeAlive => timeAlive;

    private float timeAlive;

    private readonly List<GameObject> spawnedObstacles = new List<GameObject>();

    private void Start()
    {
        GameManager.Instance.onPlay.AddListener(ResetSpawner);
        GameManager.Instance.onGameOver.AddListener(ClearSpawnedObstacles);
        GameManager.Instance.onGameOver.AddListener(ResetFactors);
    }

    private void Update()
    {
        if (GameManager.Instance == null || !GameManager.Instance.isGameOver)
            return;

        timeAlive += Time.deltaTime;
        CalculateFactors();
        CleanupOffscreenObstacles();
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

        if (obstacleParent == null)
            return;

        for (int i = obstacleParent.childCount - 1; i >= 0; i--)
        {
            Transform child = obstacleParent.GetChild(i);
            if (child != null)
                Destroy(child.gameObject);
        }
    }

    private void SpawnLoop()
    {
        timerUntilSpawn += Time.deltaTime;

        if (timerUntilSpawn < _spawnRate)
            return;

        if (CountAliveObstacles() >= maxAliveObstacles)
            return;

        SpawnObstacle();
        timerUntilSpawn = 0f;
    }

    private void CalculateFactors()
    {
        _spawnRate = spawnRate / Mathf.Pow(timeAlive, spawnRateFactor);
        _spawnRate = Mathf.Max(_spawnRate, minSpawnInterval);

        _spawnForce = spawnForce * Mathf.Pow(timeAlive, spawnForceFactor);
        _spawnForce = Mathf.Min(_spawnForce, maxSpawnForce);
    }

    private void ResetFactors()
    {
        timeAlive = 1f;
        _spawnRate = spawnRate;
        _spawnForce = spawnForce;
        timerUntilSpawn = 0f;
    }

    private void CleanupOffscreenObstacles()
    {
        for (int i = spawnedObstacles.Count - 1; i >= 0; i--)
        {
            GameObject obstacle = spawnedObstacles[i];
            if (obstacle == null)
            {
                spawnedObstacles.RemoveAt(i);
                continue;
            }

            if (obstacle.transform.position.x < despawnX)
            {
                Destroy(obstacle);
                spawnedObstacles.RemoveAt(i);
            }
        }
    }

    private int CountAliveObstacles()
    {
        int count = 0;
        for (int i = 0; i < spawnedObstacles.Count; i++)
        {
            if (spawnedObstacles[i] != null)
                count++;
        }

        return count;
    }

    private void SpawnObstacle()
    {
        GameObject obstacleToSpawn = GetRandomPrefab();
        if (obstacleToSpawn == null)
            return;

        GameObject newObstacle = Instantiate(obstacleToSpawn, transform.position, Quaternion.identity);

        newObstacle.transform.parent = obstacleParent;
        spawnedObstacles.Add(newObstacle);

        Rigidbody2D obstacleRigidbody = newObstacle.GetComponent<Rigidbody2D>();
        if (obstacleRigidbody != null)
            obstacleRigidbody.linearVelocity = Vector2.left * _spawnForce;
    }

    private GameObject GetRandomPrefab()
    {
        if (spawnEntries == null || spawnEntries.Length == 0)
            return null;

        float totalChance = 0f;
        for (int i = 0; i < spawnEntries.Length; i++)
        {
            if (spawnEntries[i].prefab != null && spawnEntries[i].chance > 0f)
                totalChance += spawnEntries[i].chance;
        }

        if (totalChance <= 0f)
            return null;

        float roll = Random.Range(0f, totalChance);
        float cumulative = 0f;

        for (int i = 0; i < spawnEntries.Length; i++)
        {
            if (spawnEntries[i].prefab == null || spawnEntries[i].chance <= 0f)
                continue;

            cumulative += spawnEntries[i].chance;
            if (roll <= cumulative)
                return spawnEntries[i].prefab;
        }

        for (int i = spawnEntries.Length - 1; i >= 0; i--)
        {
            if (spawnEntries[i].prefab != null)
                return spawnEntries[i].prefab;
        }

        return null;
    }
}
