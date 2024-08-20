using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public ParticleSystem walk;

    public Animator animator;
    public PlayerData Data;

    #region COMPONENTS
    public Rigidbody2D RB { get; private set; }
    private BoxCollider2D bc;
    #endregion

    #region STATE PARAMETERS
    public bool IsFacingRight { get; private set; }
    public bool IsJumping { get; private set; }
    public bool IsWallJumping { get; private set; }
    public bool IsDashing { get; private set; }
    public bool IsSliding { get; private set; }


    public bool IsGrounded { get; private set; }
    private bool _isJumpCut;
    private bool _isJumpFalling;

    [Header("Audio Footsteps")]
    public float footstepInterval = 0.5f;
    private float nextFootstepTime = 0f;
    #endregion

    [Header("Layers & Tags")]
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _destroyableLayer;

    private Vector2 _moveInput;
    public float LastPressedJumpTime { get; private set; }
    public float LastPressedDashTime { get; private set; }


    [Header("Checks")]
    [SerializeField] private Transform _groundCheckPoint;
    [SerializeField] private Vector2 _groundCheckSize = new Vector2(0.49f, 0.03f);
    [Space(5)]
    [SerializeField] private Transform _frontWallCheckPoint;
    [SerializeField] private Transform _backWallCheckPoint;
    [SerializeField] private Vector2 _wallCheckSize = new Vector2(0.5f, 1f);




    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        SetGravityScale(Data.gravityScale);
        IsFacingRight = true;
    }

    private void Update()
    {
        // Handle timers
        LastPressedJumpTime -= Time.deltaTime;
        LastPressedDashTime -= Time.deltaTime;

        // Handle input
        _moveInput.x = Input.GetAxisRaw("Horizontal");

        if (_moveInput.x != 0)
            CheckDirectionToFace(_moveInput.x > 0);

        if (Input.GetKeyDown(KeyCode.Space))
            OnJumpInput();

        if (Input.GetKeyUp(KeyCode.Space))
            OnJumpUpInput();

        HandleAnimation(); // Update animation based on state

        // Ground Check
        IsGrounded = Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _groundLayer);


        if (IsJumping && RB.velocity.y < 0)
        {
            IsJumping = false;
            _isJumpFalling = true;
        }

        if (!IsJumping && !IsWallJumping && LastPressedJumpTime > 0 && CanJump())
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        if (!IsDashing)
        {
            Run(1); // Handle movement
        }
    }

    public void OnJumpInput()
    {
        LastPressedJumpTime = Data.jumpInputBufferTime;
    }

    public void OnJumpUpInput()
    {
        if (CanJumpCut())
            _isJumpCut = true;
    }
    public void SetGravityScale(float scale)
    {
        RB.gravityScale = scale;
    }
    private void HandleAnimation()
    {
        // Set walking/idle animation
        if (IsGrounded && Mathf.Abs(_moveInput.x) > 0.01f)
        {
            animator.SetBool("IsWalking", true);
            animator.SetBool("IsIdle", false);
        }
        else if (IsGrounded)
        {
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsIdle", true);
        }

        // Set jumping/falling animations
        if (IsJumping)
        {
            animator.SetBool("IsJumping", true);
            animator.SetBool("IsFalling", false);
        }
        else if (_isJumpFalling && !IsGrounded)
        {
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsFalling", true);
        }
        else
        {
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsFalling", false);
        }
    }
    private void Run(float lerpAmount)
    {
        float targetSpeed = _moveInput.x * Data.runMaxSpeed;
        targetSpeed = Mathf.Lerp(RB.velocity.x, targetSpeed, lerpAmount);

        float accelRate = LastPressedJumpTime > 0 ? Data.runAccelAmount : Data.runDeccelAmount;
        float speedDif = targetSpeed - RB.velocity.x;

        RB.AddForce(speedDif * accelRate * Vector2.right, ForceMode2D.Force);
    }

    private void Jump()
    {
        LastPressedJumpTime = 0;

        float force = Data.jumpForce;
        if (RB.velocity.y < 0)
            force -= RB.velocity.y;

        IsJumping = true;
        _isJumpFalling = false;

        RB.AddForce(Vector2.up * force, ForceMode2D.Impulse);
    }

    private bool CanJump()
    {
        return IsGrounded && !IsJumping;
    }

    private bool CanJumpCut()
    {
        return IsJumping && RB.velocity.y > 0;
    }

    public void CheckDirectionToFace(bool isMovingRight)
    {
        if (isMovingRight != IsFacingRight)
        {
            Turn();
        }
    }

    private void Turn()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        IsFacingRight = !IsFacingRight;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(_groundCheckPoint.position, _groundCheckSize);
    }
}