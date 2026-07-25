using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreUI;
    [SerializeField] private GameObject startMenuUI;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject pauseButton;

    private void Start()
    {
        GameManager.Instance.onGameOver.AddListener(ActivateGameOverUI);

        if (startMenuUI != null && startMenuUI.activeSelf)
        {
            SetPauseButtonVisible(false);
            PauseGame();
        }
    }

    public void PlayButtomHandler()
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
        Time.timeScale = 1f;
        SceneManager.LoadScene("main menu");
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
    }

    private void SetPauseButtonVisible(bool visible)
    {
        if (pauseButton != null)
            pauseButton.SetActive(visible);
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f;
    }

    private void OnGUI()
    {
        if (scoreUI == null || GameManager.Instance == null)
            return;

        scoreUI.text = GameManager.Instance.PrettyScore();
    }
}
