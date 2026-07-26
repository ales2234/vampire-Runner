using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    private Vector3 startPosition;
    private Rigidbody2D rb;

    private void Awake()
    {
        startPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        GameManager.Instance.onPlay.AddListener(ActivatePlayer);
    }

    private void ActivatePlayer()
    {
        transform.position = startPosition;

        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        gameObject.SetActive(true);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("obstacle"))
        {
            gameObject.SetActive(false);
            GameManager.Instance.GameOver();
        }
    }
}
