using UnityEngine;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] obstaclesPrefabs;
    [SerializeField] private Transform obstacleParent;

    public float spawnRate = 3f;
    [Range(0f, 1f)] public float spawnRateFactor = 0.5f;
    public float spawnForce = 1f;
    [Range(0f, 1f)] public float spawnForceFactor = 0.5f;
    private float timerUntilSpawn;

    public float _spawnRate;
    public float _spawnForce;
    public float TimeAlive => timeAlive;
    
    private float timeAlive;

    private readonly List<GameObject> spawnedObstacles = new List<GameObject>();

    private void Start()
    {
        GameManager.Instance.onPlay.AddListener(ResetSpawner);
        GameManager.Instance.onGameOver.AddListener(ClearObstacle);
        GameManager.Instance.onGameOver.AddListener(ResetFactors);
    }

    private void Update()
    {
        if (GameManager.Instance == null || !GameManager.Instance.isGameOver)
            return;

        timeAlive += Time.deltaTime;
        CalculateFactors();
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

        if (timerUntilSpawn >= _spawnRate)
        {
            SpawnObstacle();
            timerUntilSpawn = 0f;
        }
    }
    private void ClearObstacle(){
        foreach (Transform child in obstacleParent){
            Destroy(child.gameObject);
        }
    }
    private void CalculateFactors(){
        _spawnRate = spawnRate / Mathf.Pow(timeAlive, spawnRateFactor);
        _spawnForce = spawnForce * Mathf.Pow(timeAlive, spawnForceFactor);
    }
    private void ResetFactors(){
        timeAlive = 1f;
        _spawnRate = spawnRate;
        _spawnForce = spawnForce;
        timerUntilSpawn = 0f;
    }

    private void SpawnObstacle()
    {
        if (obstaclesPrefabs == null || obstaclesPrefabs.Length == 0)
            return;

        GameObject obstacleToSpawn = obstaclesPrefabs[Random.Range(0, obstaclesPrefabs.Length)];
        GameObject newObstacle = Instantiate(obstacleToSpawn, transform.position, Quaternion.identity);

        newObstacle.transform.parent = obstacleParent;

        spawnedObstacles.Add(newObstacle);

        Rigidbody2D obstacleRigidbody = newObstacle.GetComponent<Rigidbody2D>();
        if (obstacleRigidbody != null)
            obstacleRigidbody.linearVelocity = Vector2.left * _spawnForce;
    }
}
