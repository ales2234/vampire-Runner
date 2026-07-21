using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreUI;
    [SerializeField] private GameObject startMenuUI;
    [SerializeField] private GameObject gameOverUI;

    private void Start()
    {
        GameManager.Instance.onGameOver.AddListener(ActivateGameOverUI);
    }

    public void PlayButtomHandler()
    {


        if (startMenuUI != null)
            startMenuUI.SetActive(false);

        if (gameOverUI != null)
            gameOverUI.SetActive(false);

        GameManager.Instance.StartGame();
    }

    public void ActivateGameOverUI()
    {
        if (gameOverUI != null)
            gameOverUI.SetActive(true);
    }

    private void OnGUI()
    {
        if (scoreUI == null || GameManager.Instance == null)
            return;

        scoreUI.text = GameManager.Instance.PrettyScore();
    }
}
