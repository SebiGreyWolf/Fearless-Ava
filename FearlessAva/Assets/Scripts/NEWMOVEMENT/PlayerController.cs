using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    [Header("Jump Settings")]
    public float jumpForce = 10f;
    public float jumpHoldForce = 2.5f;
    public float jumpHoldDuration = 0.1f;

    [SerializeField] private float gravityScale = 2f; // Normal gravity scale
    [SerializeField] private float jumpGravityScale = 1f; // Gravity scale during jump
    [SerializeField] private float fallGravityScale = 3f; // Gravity scale when falling
    [SerializeField] private float jumpSpeed = 5f; // Horizontal speed during jump
    [SerializeField] private float backflipForceX = 5f; // Horizontal force for backflip
    [SerializeField] private float backflipForceY = 7f; // Vertical force for backflip
    [SerializeField] private float backflipGravityScale = 2f; // Gravity scale during backflip
    [SerializeField] private float backflipBufferTime = 0.2f; // Time buffer to check for backflip

    [Header("Ground Check Settings")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    public bool isFacingRight = true;
    public Animator animator;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isJumping;
    private bool backflipBufferActive;
    private float jumpTimeCounter;
    private float moveInput;
    private float backflipBufferCounter;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Ground check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Handle movement input
        moveInput = Input.GetAxisRaw("Horizontal");

        // Handle jump and backflip
        HandleJump();

        // Handle movement
        Move();

        // Update animator parameters
        animator.SetFloat("WalkingSpeed", Mathf.Abs(rb.velocity.x));
        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("isFalling", rb.velocity.y < 0 && !isGrounded);

        // Update backflip buffer
        if (backflipBufferActive)
        {
            backflipBufferCounter -= Time.deltaTime;
            if (backflipBufferCounter <= 0)
            {
                backflipBufferActive = false;
            }
        }

        Debug.Log($"Update - isFacingRight: {isFacingRight}, moveInput: {moveInput}, isGrounded: {isGrounded}, backflipBufferActive: {backflipBufferActive}");
    }

    void Move()
    {
        if (moveInput != 0)
        {
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        }
        else
        {
            // Ensure the player stops moving when there is no input
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        // Only flip sprite if not attempting a backflip
        if (!isJumping && !backflipBufferActive)
        {
            FlipPlayerSprite(moveInput);
        }
    }

    void FlipPlayerSprite(float direction)
    {
        if (direction > 0 && !isFacingRight)
        {
            isFacingRight = true;
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (direction < 0 && isFacingRight)
        {
            isFacingRight = false;
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            // Normal jump initiation
            isJumping = true;
            jumpTimeCounter = jumpHoldDuration;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce); // Apply initial jump force
            rb.gravityScale = jumpGravityScale; // Apply lower gravity scale during jump

            // Trigger normal jump animation
            animator.SetTrigger("NormalJumpTrigger");

            // Activate backflip buffer
            backflipBufferActive = true;
            backflipBufferCounter = backflipBufferTime;

            Debug.Log("Jump initiated");
        }

        if (Input.GetKey(KeyCode.Space) && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                // Apply sustained jump force
                rb.velocity = new Vector2(rb.velocity.x, jumpForce + jumpHoldForce);
                jumpTimeCounter -= Time.deltaTime; // Decrease the counter
            }
            else
            {
                // Time's up, stop the sustained jump
                isJumping = false;
                rb.gravityScale = gravityScale; // Revert to normal gravity scale
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            // Jump button released, stop the sustained jump
            isJumping = false;
            rb.gravityScale = gravityScale; // Revert to normal gravity scale
        }

        // Additional condition to stop the jump if player is falling down
        if (rb.velocity.y < 0)
        {
            isJumping = false;
            rb.gravityScale = fallGravityScale; // Apply higher gravity scale when falling
        }

        // Apply horizontal speed during jump
        if (isJumping || rb.velocity.y < 0)
        {
            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x + jumpSpeed * Input.GetAxis("Horizontal"), -jumpSpeed, jumpSpeed), rb.velocity.y);
        }
        else
        {
            rb.gravityScale = gravityScale; // Ensure normal gravity when grounded
        }

        // Check if backflip buffer is active and opposite direction is pressed
        if (backflipBufferActive)
        {
            bool isOppositeDirection = (isFacingRight && moveInput < 0) || (!isFacingRight && moveInput > 0);
            if (isOppositeDirection)
            {
                PerformBackflip();
                backflipBufferActive = false;
            }

            Debug.Log($"Backflip Buffer Active - isOppositeDirection: {isOppositeDirection}");
        }
    }

    void PerformBackflip()
    {
        isJumping = true;
        jumpTimeCounter = jumpHoldDuration;
        float backflipDirection = isFacingRight ? -1 : 1;
        rb.velocity = new Vector2(backflipForceX * backflipDirection, backflipForceY); // Apply backflip force
        rb.gravityScale = backflipGravityScale; // Apply backflip gravity scale

        // Trigger backflip animation
        animator.SetTrigger("BackflipTrigger");

        Debug.Log("Backflip performed");
    }

    void HandleFastFall()
    {
        if (rb.velocity.y < 0 && !isGrounded && Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyCode.Space))
        {
            // Increase fall speed
            rb.velocity = new Vector2(rb.velocity.x, -jumpForce);
            rb.gravityScale = fallGravityScale; // Apply higher gravity scale when falling
        }
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
