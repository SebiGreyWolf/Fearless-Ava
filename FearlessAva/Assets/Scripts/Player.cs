using UnityEngine;

public class SimplePlayerMovement : MonoBehaviour
{
    //Note that walljumps should just be performed at certain walls
    //adding a wallLayer, changin isTouchingLeftWall

    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float maxJumpTime = 0.5f;
    public float gravityScaleAtApex = 3f;
    public Transform groundCheck;
    public Transform leftWallCheck;
    public Transform rightWallCheck;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isTouchingLeftWall;
    private bool isTouchingRightWall;
    private bool isFacingRight = true;
    private float jumpTimeCounter;
    private bool isJumping;
    private float originalGravityScale;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        originalGravityScale = rb.gravityScale;
    }

    private void Update()
    {
        HandleMovement();
        HandleJump();
        FlipSprite();
    }

    private void HandleMovement()
    {
        float moveInput = Input.GetAxis("Horizontal");
        Vector2 moveVelocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        rb.velocity = moveVelocity;
    }

    private void HandleJump()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
        isTouchingLeftWall = Physics2D.OverlapCircle(leftWallCheck.position, 0.1f, groundLayer);
        isTouchingRightWall = Physics2D.OverlapCircle(rightWallCheck.position, 0.1f, groundLayer);

        if (isGrounded)
        {
            rb.gravityScale = originalGravityScale;
            isJumping = false;
        }

        if (Input.GetButtonDown("Jump") && (isGrounded || isTouchingLeftWall || isTouchingRightWall))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isJumping = true;
            jumpTimeCounter = maxJumpTime;

            if (isTouchingLeftWall)
                WallJump(1);
            else if (isTouchingRightWall)
                WallJump(-1);
        }

        if (Input.GetButton("Jump") && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce * (jumpTimeCounter / maxJumpTime)); // Adjust jump force based on the remaining jump time
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
        }

        if (!isGrounded && rb.velocity.y < 0)
        {
            rb.gravityScale = gravityScaleAtApex;
        }
    }

    private void WallJump(int direction)
    {
        rb.velocity = new Vector2(direction * moveSpeed, jumpForce);
        Flip(direction == 1);
    }

    private void FlipSprite()
    {
        float moveInput = Input.GetAxis("Horizontal");

        if (moveInput > 0 && !isFacingRight)
        {
            Flip(true);
        }
        else if (moveInput < 0 && isFacingRight)
        {
            Flip(false);
        }
    }

    private void Flip(bool faceRight)
    {
        if (faceRight != isFacingRight)
        {
            isFacingRight = faceRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }
}