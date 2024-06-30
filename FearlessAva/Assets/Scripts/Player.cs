using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Note that walljumps should just be performed at certain walls
    //adding a wallLayer, changin isTouchingLeftWall
    public ParticleSystem dust;
    public HealthBar healthBar;
    public AudioClip walkSound;

    public int maxHealth = 16;
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float maxJumpTime = 0.5f;
    public float gravityScaleAtApex = 3f;
    public Transform groundCheck;
    public Transform leftWallCheck;
    public Transform rightWallCheck;
    public LayerMask groundLayer;
    public float invulnerabilityDuration = 0.5f;
    public Vector2 wallJumpForce = new Vector2(2,2);


    private AudioSource audioSource;
    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isTouchingLeftWall;
    private bool isTouchingRightWall;
    private bool isFacingRight = true;
    private float jumpTimeCounter;
    private bool isJumping;
    private float originalGravityScale;
    private int currentHealth;
    private bool isInvulnerable = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        originalGravityScale = rb.gravityScale;
        healthBar.SetMaxHealth(maxHealth);
        audioSource = GetComponent<AudioSource>();
        currentHealth = maxHealth;
    }

    private void Update()
    {
        //Auskkommentiert weil Movement jetzt vom PlayerMovement-Script gehandeled wird

        //HandleMovement();
        //HandleJump();
        //FlipSprite();
    }

    private void HandleMovement()
    {
        audioSource.clip = walkSound;
        audioSource.Play();
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
        Vector2 force = new Vector2(wallJumpForce.x * direction, wallJumpForce.y);
        Debug.Log(force.x);
        Debug.Log(force.y);

        rb.AddForce(force, ForceMode2D.Impulse);
        Debug.Log(rb.velocity);

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
            CreateDust();
            isFacingRight = faceRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    public void TakeDamage(int amount)
    {
        if (!isInvulnerable)
        {
            currentHealth -= amount;
            healthBar.SetHealth(currentHealth);
            if (currentHealth <= 0)
            {
                Debug.Log("YOU ARE FUCKING DEAD! (LOSER)");
            }
            StartCoroutine(InvulnerabilityTimer());
        }
    }
    private IEnumerator InvulnerabilityTimer()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(invulnerabilityDuration);
        isInvulnerable = false;
    }

    private void CreateDust()
    {
        if (isGrounded)
            dust.Play();
    }
}