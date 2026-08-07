using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D rb;
    [SerializeField] private float pointsPerHit = 100f;
    [SerializeField] private float pointsPerEnemyHit = 500f;
    [SerializeField] private float hitRadius = 0.25f;
    [SerializeField] private float lifeTime = 3f;

    private bool hasHit;

    private void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;

        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        Destroy(gameObject, lifeTime);
    }

    private void FixedUpdate()
    {
        if (hasHit || rb == null)
            return;

        Vector2 direction = transform.right;
        float distance = speed * Time.fixedDeltaTime;
        Vector2 origin = rb.position;

        RaycastHit2D hit = Physics2D.CircleCast(origin, hitRadius, direction, distance);

        if (hit.collider != null)
            HandleHit(hit.collider);

        if (hasHit)
            return;

        rb.MovePosition(origin + direction * distance);
    }

    private void HandleHit(Collider2D other)
    {
        if (hasHit || other == null)
            return;

        // Check parents — obstacles are under Spawner, so transform.root is wrong
        if (HasTagInParents(other.transform, "double obstacle"))
        {
            hasHit = true;
            Destroy(gameObject);
            return;
        }

        Transform target = FindTaggedAncestor(other.transform, "obstacle", "enemy");
        if (target == null)
            return;

        hasHit = true;

        if (target.CompareTag("enemy"))
        {
            if (GameManager.Instance != null)
                GameManager.Instance.currentScore += pointsPerEnemyHit;
        }
        else
        {
            if (GameManager.Instance != null)
                GameManager.Instance.currentScore += pointsPerHit;
        }

        Destroy(target.gameObject);
        Destroy(gameObject);
    }

    private static Transform FindTaggedAncestor(Transform start, params string[] tags)
    {
        Transform current = start;
        Transform found = null;

        while (current != null)
        {
            foreach (string tag in tags)
            {
                if (current.CompareTag(tag))
                {
                    found = current;
                    break;
                }
            }

            current = current.parent;
        }

        return found;
    }

    private static bool HasTagInParents(Transform start, string tag)
    {
        Transform current = start;
        while (current != null)
        {
            if (current.CompareTag(tag))
                return true;
            current = current.parent;
        }

        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, hitRadius);
    }
}
