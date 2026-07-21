using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    #region Singleton

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    public float currentScore = 0f;

    public bool isGameOver = false;

    public UnityEvent onPlay = new UnityEvent();
    public UnityEvent onGameOver = new UnityEvent();

    public void Update(){
        if(isGameOver){
            currentScore += Time.deltaTime;
        }
    }

    public void StartGame(){
        onPlay.Invoke();
        isGameOver = true;
    }

    public void GameOver(){
        onGameOver.Invoke();
        isGameOver = false;
        currentScore = 0f;

    }

    public string PrettyScore(){
        return Mathf.RoundToInt (currentScore).ToString ();
}
}
