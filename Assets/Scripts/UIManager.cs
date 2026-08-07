using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreUI;
    [SerializeField] private GameObject startMenuUI;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject pauseButton;

    [SerializeField] private TextMeshProUGUI gameOverScoreUI;
    [SerializeField] private TextMeshProUGUI gameOverHighScoreUI;

    private string lastScoreText;

    private void Start()
    {
        GameManager.Instance.onGameOver.AddListener(ActivateGameOverUI);

        if (startMenuUI != null && startMenuUI.activeSelf)
        {
            SetPauseButtonVisible(false);
            PauseGame();
        }
    }

    private void Update()
    {
        if (scoreUI == null || GameManager.Instance == null)
            return;

        string scoreText = GameManager.Instance.PrettyScore();
        if (scoreText == lastScoreText)
            return;

        lastScoreText = scoreText;
        scoreUI.text = scoreText;
    }

    public void PlayButtomHandler()
    {
        if (FadeInOut.Instance != null)
        {
            FadeInOut.Instance.FadeAndReplay(ReplayGame);
            return;
        }

        ReplayGame();
    }

    private void ReplayGame()
    {
        if (startMenuUI != null)
            startMenuUI.SetActive(false);

        if (gameOverUI != null)
            gameOverUI.SetActive(false);

        SetPauseButtonVisible(true);
        ResumeGame();
        GameManager.Instance.StartGame();
    }

    public void ResumeButtonHandler()
    {
        if (startMenuUI != null)
            startMenuUI.SetActive(false);

        SetPauseButtonVisible(true);
        ResumeGame();
    }

    public void QuitButtonHandler()
    {
        if (FadeInOut.Instance != null)
            FadeInOut.Instance.FadeToScene("main menu");
        else
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("main menu");
        }
    }

    public void ActivateStartMenuUI()
    {
        if (startMenuUI != null)
            startMenuUI.SetActive(true);

        SetPauseButtonVisible(false);
        PauseGame();
    }

    public void ActivateGameOverUI()
    {
        if (gameOverUI != null)
            gameOverUI.SetActive(true);

        SetPauseButtonVisible(false);
        PauseGame();
        UpdateGameOverScores();
    }

    private void UpdateGameOverScores()
    {
        if (GameManager.Instance == null)
            return;

        if (gameOverScoreUI != null)
            gameOverScoreUI.text = "Score: " + GameManager.Instance.PrettyScore();

        if (gameOverHighScoreUI != null)
            gameOverHighScoreUI.text = "High Score: " + GameManager.Instance.PrettyHighScore();
    }

    private void SetPauseButtonVisible(bool visible)
    {
        if (pauseButton != null)
            pauseButton.SetActive(visible);
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
        if (GameManager.Instance != null)
            GameManager.Instance.PauseMusic();
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f;
        if (GameManager.Instance != null)
            GameManager.Instance.ResumeMusic();
    }
}
