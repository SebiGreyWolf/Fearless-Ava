using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float changeDirectionDelay = 0.2f;

    [Header("Ground Check Settings")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    public Animator animator;

    private Rigidbody2D rb;
    private bool isGrounded;
    private float lastMoveDirection = 0f;
    private bool isChangingDirection = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Ground check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Handle movement
        if (isGrounded && !isChangingDirection)
        {
            Move();
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        // Update animator with speed
        animator.SetFloat("WalkingSpeed", Mathf.Abs(rb.velocity.x));
    }

    void Move()
    {
        float moveInput = Input.GetAxis("Horizontal");

        if (moveInput != 0)
        {
            // Check for direction change
            if (Mathf.Sign(moveInput) != Mathf.Sign(lastMoveDirection) && lastMoveDirection != 0)
            {
                StartCoroutine(ChangeDirectionCoroutine(Mathf.Sign(moveInput)));
            }
            else
            {
                rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
                lastMoveDirection = moveInput;
            }
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    IEnumerator ChangeDirectionCoroutine(float newDirection)
    {
        isChangingDirection = true;
        animator.SetBool("IsChangingDirection", true);

        if (newDirection > 0)
        {
            animator.SetTrigger("ChangeDirectionRight");
        }
        else
        {
            animator.SetTrigger("ChangeDirectionLeft");
        }

        yield return new WaitForSeconds(changeDirectionDelay);

        // Flip the player sprite after the delay
        FlipPlayerSprite(newDirection);

        lastMoveDirection = newDirection;
        isChangingDirection = false;
        animator.SetBool("IsChangingDirection", false);
    }

    void FlipPlayerSprite(float direction)
    {
        if (direction > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (direction < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
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
