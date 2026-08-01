using UnityEngine;

public class MenuUI : MonoBehaviour
{
    public void LoadGameScene()
    {
        if (FadeInOut.Instance != null)
            FadeInOut.Instance.FadeToScene("SampleScene");
        else
        {
            Time.timeScale = 1f;
            UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
        }
    }

    public void QuitGame()
    {
        if (FadeInOut.Instance != null)
            FadeInOut.Instance.FadeAndQuit();
        else
        {
            Time.timeScale = 1f;
            Application.Quit();
        }
    }
}
