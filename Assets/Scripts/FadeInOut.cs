using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class FadeInOut : MonoBehaviour
{
    public static FadeInOut Instance { get; private set; }

    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private Color fadeColor = Color.black;

    private CanvasGroup canvasGroup;
    private bool isFading;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        EnsureFadeUI();
    }

    private void Start()
    {
        // Start covered, then fade in
        canvasGroup.alpha = 1f;
        StartCoroutine(Fade(0f));
    }

    public void FadeToScene(string sceneName)
    {
        if (isFading)
            return;

        StartCoroutine(FadeOutThenLoad(sceneName));
    }

    public void FadeAndReplay(Action onMidFade)
    {
        if (isFading)
            return;

        StartCoroutine(FadeOutActionFadeIn(onMidFade));
    }

    public void FadeAndQuit()
    {
        if (isFading)
            return;

        StartCoroutine(FadeOutThenQuit());
    }

    private IEnumerator FadeOutThenLoad(string sceneName)
    {
        isFading = true;
        Time.timeScale = 1f;
        yield return Fade(1f);
        SceneManager.LoadScene(sceneName);
        // New scene's FadeInOut Start will fade in if this object is destroyed;
        // with DontDestroyOnLoad we fade in after load:
        yield return null;
        yield return Fade(0f);
        isFading = false;
    }

    private IEnumerator FadeOutActionFadeIn(Action onMidFade)
    {
        isFading = true;
        yield return Fade(1f);
        onMidFade?.Invoke();
        yield return Fade(0f);
        isFading = false;
    }

    private IEnumerator FadeOutThenQuit()
    {
        isFading = true;
        Time.timeScale = 1f;
        yield return Fade(1f);
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private IEnumerator Fade(float targetAlpha)
    {
        if (canvasGroup == null)
            yield break;

        canvasGroup.blocksRaycasts = true;
        float startAlpha = canvasGroup.alpha;
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = targetAlpha;
        canvasGroup.blocksRaycasts = targetAlpha > 0.9f;
    }

    private void EnsureFadeUI()
    {
        canvasGroup = GetComponentInChildren<CanvasGroup>();
        if (canvasGroup != null)
            return;

        GameObject canvasObj = new GameObject("FadeCanvas");
        canvasObj.transform.SetParent(transform);

        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 9999;
        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();

        canvasGroup = canvasObj.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        GameObject imageObj = new GameObject("FadeImage");
        imageObj.transform.SetParent(canvasObj.transform, false);

        Image image = imageObj.AddComponent<Image>();
        image.color = fadeColor;
        image.raycastTarget = true;

        RectTransform rect = image.rectTransform;
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }
}
