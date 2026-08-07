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
    [SerializeField] private AudioSource musicSource;

    public float currentScore = 0f;
    public float highScore = 0f;
    public Data data;
    public bool isGameOver = false;

    public UnityEvent onPlay = new UnityEvent();
    public UnityEvent onGameOver = new UnityEvent();

    private void Start()
    {
        if (spawner == null)
            spawner = FindFirstObjectByType<Spawner>();

        if (musicSource == null)
            musicSource = GetComponent<AudioSource>();

        string dataToLoad = SaveSystem.load("data");
        if (dataToLoad != null)
            data = JsonUtility.FromJson<Data>(dataToLoad);
        else
            data = new Data();

        highScore = data.highScore;
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
        PlayMusic();
    }

    public void GameOver()
    {
        if (highScore < currentScore)
            highScore = currentScore;

        data.highScore = highScore;

        string dataToSave = JsonUtility.ToJson(data);
        SaveSystem.save("data", dataToSave);

        StopMusic();
        onGameOver.Invoke();

        isGameOver = false;
        currentScore = 0f;
    }

    public void StopMusic()
    {
        if (musicSource != null)
            musicSource.Stop();
    }

    public void PauseMusic()
    {
        if (musicSource != null)
            musicSource.Pause();
    }

    public void ResumeMusic()
    {
        if (musicSource != null)
            musicSource.UnPause();
    }

    public void PlayMusic()
    {
        if (musicSource != null && !musicSource.isPlaying)
            musicSource.Play();
    }

    public string PrettyScore()
    {
        return Mathf.RoundToInt(currentScore).ToString();
    }

    public string PrettyHighScore()
    {
        return Mathf.RoundToInt(highScore).ToString();
    }
}
