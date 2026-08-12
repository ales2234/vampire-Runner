using UnityEngine;

public class ConstantScrollSpeed : MonoBehaviour
{
    [SerializeField] private float speed;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;

        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        if (rb != null)
            rb.linearVelocity = Vector2.left * speed;
    }

    private void FixedUpdate()
    {
        if (rb == null)
            return;

        // Keep the X speed from spawn time; allow Y (gravity/jumps) if needed
        rb.linearVelocity = new Vector2(-speed, rb.linearVelocity.y);
    }
}
