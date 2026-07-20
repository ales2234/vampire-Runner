using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] obstaclesPrefabs;
    public float spawnRate = 2f;
    private float TimerUntilSpawn;
    public float spawnForce = 1f;

    private void Update(){
        SpawnLoop();
    }

    private void SpawnLoop(){
        TimerUntilSpawn += Time.deltaTime;

        if (TimerUntilSpawn >= spawnRate){
            SpawnObstacle();
            TimerUntilSpawn = 0f;
        }
    }
    private void SpawnObstacle(){
        GameObject obstacleToSpawn = obstaclesPrefabs[Random.Range(0, obstaclesPrefabs.Length)];

        GameObject newObstacle = Instantiate(obstacleToSpawn, transform.position, Quaternion.identity);

        Rigidbody2D obstacleRigidbody = newObstacle.GetComponent<Rigidbody2D>();
        obstacleRigidbody.linearVelocity = Vector2.left * spawnForce;
}
}
