using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    [Header("Jump Settings")]
    public float jumpForceX = 0f; // Horizontal force applied when jumping
    public float jumpForceY = 10f; // Vertical force applied when jumping
    public float lowJumpForceY = 5f; // Vertical force for a low jump
    public float jumpPressDurationThreshold = 0.2f; // Threshold to differentiate between low and high jumps
    private float jumpTime;
    private bool isJumping;
    private bool jumpKeyHeld;

    [Header("Wall Jump Settings")]
    public float wallJumpForceX = 10f;
    public float wallJumpForceY = 10f;
    private bool isWallJumping;
    private float wallJumpDirection;

    [Header("Wall Slide Settings")]
    public float wallSlideSpeed = 2f;
    public float wallSlideDelay = 0.5f; // Delay before sliding
    private bool isWallSliding;
    private float wallTouchTime;

    [Header("Check Settings")]
    public Transform groundCheck;
    public Transform wallCheck;
    public float checkRadius;
    public LayerMask groundLayer;
    public LayerMask wallLayer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isTouchingWall;

    private Vector3 initialScale;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialScale = transform.localScale; // Store the initial scale
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        HandleWallSlide();
    }

    void HandleMovement()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        if (moveInput != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(moveInput) * Mathf.Abs(initialScale.x), initialScale.y, initialScale.z);
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        isTouchingWall = Physics2D.OverlapCircle(wallCheck.position, checkRadius, wallLayer);

        if (isGrounded)
        {
            isWallSliding = false;
        }

        if (isTouchingWall && !isGrounded && rb.velocity.y < 0)
        {
            wallTouchTime = Time.time;
        }
    }

    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && (isGrounded || isTouchingWall))
        {
            isJumping = true;
            jumpKeyHeld = true;
            jumpTime = 0;
        }

        if (Input.GetButtonUp("Jump") && isJumping)
        {
            jumpKeyHeld = false;
            ApplyJumpForce();
        }

        if (jumpKeyHeld)
        {
            jumpTime += Time.deltaTime;

            if (jumpTime >= jumpPressDurationThreshold)
            {
                jumpKeyHeld = false;
                ApplyJumpForce();
            }
        }
    }

    void ApplyJumpForce()
    {
        float appliedJumpForceY = jumpTime < jumpPressDurationThreshold ? lowJumpForceY : jumpForceY;
        rb.velocity = new Vector2(rb.velocity.x + jumpForceX, appliedJumpForceY);
        isJumping = false;
    }

    void WallJump()
    {
        isWallJumping = true;
        wallJumpDirection = -Mathf.Sign(transform.localScale.x);
        rb.velocity = new Vector2(wallJumpDirection * wallJumpForceX, wallJumpForceY);
        Invoke("SetWallJumpingToFalse", 0.2f);
    }

    void SetWallJumpingToFalse()
    {
        isWallJumping = false;
    }

    void HandleWallSlide()
    {
        if (isTouchingWall && !isGrounded && rb.velocity.y < 0)
        {
            if (Time.time > wallTouchTime + wallSlideDelay)
            {
                isWallSliding = true;
                rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
            }
        }
        else
        {
            isWallSliding = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        Gizmos.DrawWireSphere(wallCheck.position, checkRadius);
    }
}
