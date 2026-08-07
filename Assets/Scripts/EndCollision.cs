using UnityEngine;

public class EndCollision : MonoBehaviour
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
        GameManager.Instance.onPlay.AddListener(ActivateEndRoad);
    }

    private void ActivateEndRoad()
    {
        transform.position = startPosition;

        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        gameObject.SetActive(true);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("obstacle"))
            Destroy(other.gameObject);
        if (other.transform.CompareTag("Bullet"))
            Destroy(other.gameObject);
        if (other.transform.CompareTag("enemy"))
            Destroy(other.gameObject);
        if (other.transform.CompareTag("double obstacle"))
            Destroy(other.gameObject);
    }
}
