using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float jumpForce = 12f;

    public float acceleration = 20f;

    public float coyoteTime = 0.1f;
    private float coyoteCounter;

    private Rigidbody2D rb;
    private float moveInput;
    private float currentSpeed;
    private Animator animator;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public LayerMask groundLayer;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        bool isMoving = Mathf.Abs(moveInput) > 0.01f;

        // 🪶 Coyote time
        if (isGrounded)
            coyoteCounter = coyoteTime;
        else
{
    coyoteCounter -= Time.deltaTime;
    coyoteCounter = Mathf.Max(coyoteCounter, 0);
}

        // ⚔️ Jump (ใช้แค่ระบบเดียว)
        if (Input.GetKeyDown(KeyCode.Space) && coyoteCounter > 0)
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        coyoteCounter = 0;
    }

        // 🔄 หันหน้า
        if (moveInput != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(moveInput), 1, 1);
        }

            animator.SetBool("isRunning", isMoving && Input.GetKey(KeyCode.LeftShift));
            animator.SetBool("isWalking", isMoving && !Input.GetKey(KeyCode.LeftShift));
            animator.SetBool("isGrounded", isGrounded);
            animator.SetFloat("yVelocity", rb.linearVelocity.y);
    }

    void FixedUpdate()
    {
        float targetSpeed = moveInput * currentSpeed;

        float speedDiff = targetSpeed - rb.linearVelocity.x;

        float movement = speedDiff * acceleration * Time.fixedDeltaTime;

        rb.linearVelocity = new Vector2(
            rb.linearVelocity.x + movement,
            rb.linearVelocity.y
        );
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }
    }
}