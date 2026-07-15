using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform feetPos;
    [SerializeField] private float groundDistance = 0.25f;
    [SerializeField] private bool showDebugLogs = true;
    [SerializeField] private Animator animator;

    private bool isGrounded;

    private void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    private void Update()
    {   
        


        if (rb == null || feetPos == null)
        {
            if (showDebugLogs)
                Debug.LogWarning("PlayerMovement: assign Rb and Feet Pos in the Inspector.");

            return;
        }

        isGrounded = Physics2D.OverlapCircle(feetPos.position, groundDistance, groundLayer);

        if (WasJumpPressed())
        {
            if (showDebugLogs)
                Debug.Log($"Jump input detected. Grounded: {isGrounded}");

            if (isGrounded)
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
        animator.SetFloat("Speed", rb.linearVelocity.x);
        animator.SetBool("IsGrounded", isGrounded);
    }

    private bool WasJumpPressed()
    {
        if (Touchscreen.current != null &&
            Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
            return true;

        if (Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame)
            return true;

        if (Keyboard.current != null &&
            Keyboard.current.spaceKey.wasPressedThisFrame)
            return true;

        return false;
    }

    private void OnDrawGizmosSelected()
    {
        if (feetPos == null)
            return;

        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(feetPos.position, groundDistance);
    }
}
