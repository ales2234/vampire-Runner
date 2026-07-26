using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D rb;
    [SerializeField] private float pointsPerHit = 100f;

    private void Start()
    {
        rb.linearVelocity = transform.right * speed;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.transform.CompareTag("obstacle"))
            return;

        if (GameManager.Instance != null)
            GameManager.Instance.currentScore += pointsPerHit;

        Destroy(other.gameObject);
        Destroy(gameObject);
    }
}
