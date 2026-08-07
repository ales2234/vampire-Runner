using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D rb;
    [SerializeField] private float pointsPerHit = 100f;
    [SerializeField] private float pointsPerEnemyHit = 500f;

    private void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        // Trigger = detect hits without pushing other objects
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.isTrigger = true;

        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0f;
    }

    private void Start()
    {
        rb.linearVelocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Transform root = other.transform.root;

        if (HasTag(other.gameObject, root, "double obstacle"))
        {
            Destroy(gameObject);
            return;
        }

        if (HasTag(other.gameObject, root, "obstacle"))
        {
            if (GameManager.Instance != null)
                GameManager.Instance.currentScore += pointsPerHit;

            Destroy(root.gameObject);
            Destroy(gameObject);
        }
        if (HasTag(other.gameObject, root, "enemy")){
            if (GameManager.Instance != null)
                GameManager.Instance.currentScore += pointsPerEnemyHit;
            Destroy(root.gameObject);
            Destroy(gameObject);
        }
    }

    private static bool HasTag(GameObject hitObject, Transform root, string tag)
    {
        return hitObject.CompareTag(tag) || root.CompareTag(tag);
    }
}
