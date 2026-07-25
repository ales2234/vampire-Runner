using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float maxJumpHoldTime = 0.25f;
    [SerializeField] private float jumpCutMultiplier = 0.4f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform feetPos;
    [SerializeField] private float groundDistance = 0.25f;
    [SerializeField] private bool showDebugLogs = true;
    [SerializeField] private Animator animator;
    [SerializeField] private Spawner spawner;
    [SerializeField] private float baseAnimSpeed = 1f;
    [Range(0f, 1f)] [SerializeField] private float animSpeedFactor = 0.5f;
    [SerializeField] private float maxAnimSpeed = 3f;

    private bool isGrounded;
    private bool isJumping;
    private float jumpHoldTimer;

    private void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (spawner == null)
            spawner = FindFirstObjectByType<Spawner>();
    }

    private void Update()
    {
        if (rb == null || feetPos == null)
        {
            if (showDebugLogs)
                Debug.LogWarning("PlayerMovement: assign Rb and Feet Pos in the Inspector.");

            return;
        }

        UpdateAnimationSpeed();

        isGrounded = Physics2D.OverlapCircle(feetPos.position, groundDistance, groundLayer);

        if (isGrounded && WasJumpPressed())
        {
            isJumping = true;
            jumpHoldTimer = 0f;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

            if (showDebugLogs)
                Debug.Log("Jump started");
        }

        if (isJumping && IsJumpHeld() && jumpHoldTimer < maxJumpHoldTime)
        {
            jumpHoldTimer += Time.deltaTime;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        if (WasJumpReleased() && isJumping)
        {
            if (rb.linearVelocity.y > 0f)
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutMultiplier);

            isJumping = false;
        }

        if (isGrounded && rb.linearVelocity.y <= 0f)
            isJumping = false;
    }

    private void UpdateAnimationSpeed()
    {
        if (animator == null)
            return;

        if (spawner == null || GameManager.Instance == null || !GameManager.Instance.isGameOver)
        {
            animator.speed = baseAnimSpeed;
            return;
        }

        float speed = baseAnimSpeed * Mathf.Pow(spawner.TimeAlive, animSpeedFactor);
        animator.speed = Mathf.Min(speed, maxAnimSpeed);
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

    private bool IsJumpHeld()
    {
        if (Touchscreen.current != null &&
            Touchscreen.current.primaryTouch.press.isPressed)
            return true;

        if (Mouse.current != null &&
            Mouse.current.leftButton.isPressed)
            return true;

        if (Keyboard.current != null &&
            Keyboard.current.spaceKey.isPressed)
            return true;

        return false;
    }

    private bool WasJumpReleased()
    {
        if (Touchscreen.current != null &&
            Touchscreen.current.primaryTouch.press.wasReleasedThisFrame)
            return true;

        if (Mouse.current != null &&
            Mouse.current.leftButton.wasReleasedThisFrame)
            return true;

        if (Keyboard.current != null &&
            Keyboard.current.spaceKey.wasReleasedThisFrame)
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
