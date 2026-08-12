using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float maxJumpHoldTime = 0.25f;
    [SerializeField] private float jumpCutMultiplier = 0.4f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform feetPos;
    [SerializeField] private float groundDistance = 0.25f;
    [SerializeField] private bool showDebugLogs = false;
    [SerializeField] private Animator animator;
    [SerializeField] private Spawner spawner;
    [SerializeField] private float baseAnimSpeed = 1f;
    [Range(0f, 1f)] [SerializeField] private float animSpeedFactor = 0.5f;
    [SerializeField] private float maxAnimSpeed = 3f;

    private bool isGrounded;
    private bool isJumping;
    private float jumpHoldTimer;
    private readonly List<RaycastResult> uiRaycastResults = new List<RaycastResult>();

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
            Touchscreen.current.primaryTouch.press.wasPressedThisFrame &&
            !IsTouchOverUI())
            return true;

        if (Keyboard.current != null &&
            Keyboard.current.spaceKey.wasPressedThisFrame)
            return true;

        return false;
    }

    private bool IsJumpHeld()
    {
        if (Touchscreen.current != null &&
            Touchscreen.current.primaryTouch.press.isPressed &&
            !IsTouchOverUI())
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

        if (Keyboard.current != null &&
            Keyboard.current.spaceKey.wasReleasedThisFrame)
            return true;

        return false;
    }

    private bool IsTouchOverUI()
    {
        if (EventSystem.current == null || Touchscreen.current == null)
            return false;

        Vector2 touchPos = Touchscreen.current.primaryTouch.position.ReadValue();
        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = touchPos
        };

        uiRaycastResults.Clear();
        EventSystem.current.RaycastAll(eventData, uiRaycastResults);
        return uiRaycastResults.Count > 0;
    }

    private void OnDrawGizmosSelected()
    {
        if (feetPos == null)
            return;

        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(feetPos.position, groundDistance);
    }
}
