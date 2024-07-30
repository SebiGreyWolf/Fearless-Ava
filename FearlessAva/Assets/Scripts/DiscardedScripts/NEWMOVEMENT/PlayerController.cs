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
    public float wallJumpForce = 10f;
    public float slideSpeed = 2f;
    public float slideAccel = 10f;

    [SerializeField] private float gravityScale = 2f;
    [SerializeField] private float jumpGravityScale = 1f;
    [SerializeField] private float fallGravityScale = 3f;
    [SerializeField] private float backflipForceX = 5f;
    [SerializeField] private float backflipForceY = 7f;
    [SerializeField] private float backflipGravityScale = 2f;
    [SerializeField] private float backflipBufferTime = 0.2f;

    [Header("Ground Check Settings")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Wall Check Settings")]
    public Transform wallCheck;
    public float wallCheckRadius = 0.2f;
    public LayerMask wallLayer;

    public bool isFacingRight = true;
    public Animator animator;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isTouchingWall;
    private bool isWallSliding;
    private bool isJumping;
    private bool backflipBufferActive;
    private float jumpTimeCounter;
    private float moveInput;
    private float backflipBufferCounter;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Ground check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Wall check
        isTouchingWall = Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, wallLayer);

        // Handle movement input
        moveInput = Input.GetAxisRaw("Horizontal");

        // Handle jump and backflip
        HandleJump();

        // Handle movement
        Move();

        // Handle wall slide
        HandleWallSlide();

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

        if (Input.GetKeyDown(KeyCode.Space) && isTouchingWall && !isGrounded)
        {
            PerformWallJump();
        }
    }

    private void FixedUpdate()
    {
        //Handle Slide
        if (isWallSliding)
        {
            Slide();
        }
    }

    void Move()
    {
        if (moveInput != 0)
        {
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        if (!isJumping && !backflipBufferActive)
        {
            FlipPlayerSprite(moveInput);
        }
    }

    void FlipPlayerSprite(float direction)
    {
        if (direction > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (direction < 0 && isFacingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.localScale = new Vector3(isFacingRight ? 1 : -1, 1, 1);
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                isJumping = true;
                jumpTimeCounter = jumpHoldDuration;
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                rb.gravityScale = jumpGravityScale;

                animator.SetTrigger("NormalJumpTrigger");

                backflipBufferActive = true;
                backflipBufferCounter = backflipBufferTime;
            }
            else if (isTouchingWall && !isGrounded && rb.velocity.y < 0)
            {
                rb.velocity = new Vector2(isFacingRight ? -jumpForce : jumpForce, jumpForce);
                Flip();
                animator.SetTrigger("WallJumpTrigger");
            }
        }

        if (Input.GetKey(KeyCode.Space) && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce + jumpHoldForce);
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
                rb.gravityScale = gravityScale;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
            rb.gravityScale = gravityScale;
        }

        if (rb.velocity.y < 0)
        {
            isJumping = false;
            rb.gravityScale = fallGravityScale;
        }

        if (backflipBufferActive)
        {
            bool isOppositeDirection = (isFacingRight && moveInput < 0) || (!isFacingRight && moveInput > 0);
            if (isOppositeDirection)
            {
                PerformBackflip();
                backflipBufferActive = false;
            }
        }
    }

    void HandleWallSlide()
    {
        bool isMovingTowardsWall = (isFacingRight && moveInput > 0) || (!isFacingRight && moveInput < 0);

        if (isTouchingWall && !isGrounded && isMovingTowardsWall)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, -slideSpeed);
        }
        else
        {
            isWallSliding = false;
        }

        animator.SetBool("isWallSliding", isWallSliding);
    }

    void PerformWallJump()
    {
        float jumpDirection = isFacingRight ? -1f : 1f;
        rb.velocity = new Vector2(jumpDirection * wallJumpForce, jumpForce);
        animator.SetTrigger("WallJumpTrigger");
    }

    void Slide()
    {
        if (rb.velocity.y > 0)
        {
            rb.AddForce(-rb.velocity.y * Vector2.up, ForceMode2D.Impulse);
        }

        float speedDif = slideSpeed - rb.velocity.y;
        float movement = speedDif * slideAccel;
        movement = Mathf.Clamp(movement, -Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime), Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime));

        animator.SetTrigger("Sliding");

        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(movement * Vector2.up);
    }

    void PerformBackflip()
    {
        isJumping = true;
        jumpTimeCounter = jumpHoldDuration;
        float backflipDirection = isFacingRight ? -1 : 1;
        rb.velocity = new Vector2(backflipForceX * backflipDirection, backflipForceY);
        rb.gravityScale = backflipGravityScale;
        animator.SetTrigger("BackflipTrigger");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(wallCheck.position, wallCheckRadius);
    }
}
