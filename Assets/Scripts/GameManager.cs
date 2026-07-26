using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    #region Singleton

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    #endregion

    [SerializeField] private Spawner spawner;
    [SerializeField] private float scoreMultiplier = 1f;

    public float currentScore = 0f;
    public bool isGameOver = false;

    public UnityEvent onPlay = new UnityEvent();
    public UnityEvent onGameOver = new UnityEvent();

    private void Start()
    {
        if (spawner == null)
            spawner = FindFirstObjectByType<Spawner>();
    }

    private void Update()
    {
        if (!isGameOver || spawner == null)
            return;

        currentScore += Time.deltaTime * scoreMultiplier * spawner._spawnForce;
    }

    public void StartGame()
    {
        onPlay.Invoke();
        isGameOver = true;
    }

    public void GameOver()
    {
        onGameOver.Invoke();
        isGameOver = false;
        currentScore = 0f;
    }

    public string PrettyScore()
    {
        return Mathf.RoundToInt(currentScore).ToString();
    }
}
