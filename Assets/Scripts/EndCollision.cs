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
        TryDestroyTarget(other.collider);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        TryDestroyTarget(other);
    }

    private void TryDestroyTarget(Collider2D other)
    {
        if (other == null)
            return;

        Transform target = FindTaggedAncestor(other.transform, "obstacle", "enemy", "double obstacle", "Bullet");
        if (target != null)
            Destroy(target.gameObject);
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
}
