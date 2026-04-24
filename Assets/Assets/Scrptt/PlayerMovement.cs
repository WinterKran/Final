using System.Collections;
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

    public float knockbackForce = 12f;
    public float knockbackDuration = 0.2f;

    bool isInvincible = false;
    public float invincibleTime = 0.5f;

    private bool isKnocked = false;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

   void Update()
{
    if (isKnocked) return; // ❗ หยุดควบคุมตอนโดน knockback

    moveInput = Input.GetAxisRaw("Horizontal");

    currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

    isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

    bool isMoving = Mathf.Abs(moveInput) > 0.01f;

    if (isGrounded)
        coyoteCounter = coyoteTime;
    else
    {
        coyoteCounter -= Time.deltaTime;
        coyoteCounter = Mathf.Max(coyoteCounter, 0);
    }

    if (Input.GetKeyDown(KeyCode.Space) && coyoteCounter > 0)
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        coyoteCounter = 0;
    }

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
    if (isKnocked) return; // ❗ กัน movement กลบแรงกระเด็น

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

    public void TakeDamage(float dmg, Vector2 sourcePos)
{
    if (isInvincible) return;

    // ลดเลือด

    Vector2 dir = (transform.position - (Vector3)sourcePos).normalized;
    dir.y = 0.5f; // 🔥 ดันขึ้นนิดนึง
    dir = dir.normalized;
    StartCoroutine(Knockback(dir));
    StartCoroutine(Invincible());
}

IEnumerator Invincible()
{
    isInvincible = true;
    yield return new WaitForSeconds(invincibleTime);
    isInvincible = false;
}

IEnumerator Knockback(Vector2 dir)
{
    isKnocked = true;

    rb.linearVelocity = Vector2.zero;
    rb.AddForce(dir * knockbackForce, ForceMode2D.Impulse);

    yield return new WaitForSeconds(knockbackDuration);

    isKnocked = false;
}
public Health health;

public void Respawn()
{
    float x = PlayerPrefs.GetFloat("cp_x", transform.position.x);
    float y = PlayerPrefs.GetFloat("cp_y", transform.position.y);
    float z = PlayerPrefs.GetFloat("cp_z", transform.position.z);

    transform.position = new Vector3(x, y, z);

    // รีเซ็ต HP
    if (health != null)
    {
        health.currentHealth = health.maxHealth;
        health.onHealthChanged?.Invoke(1f);
    }

    // reset state
    isKnocked = false;
    isInvincible = false;
    rb.linearVelocity = Vector2.zero;

    // ✅ เรียกแค่ครั้งเดียว
    if (GameManager.instance != null)
    {
        GameManager.instance.ResetAllBosses();
    }
}

public void RespawnInternal()
{
    float x = PlayerPrefs.GetFloat("cp_x", transform.position.x);
    float y = PlayerPrefs.GetFloat("cp_y", transform.position.y);
    float z = PlayerPrefs.GetFloat("cp_z", transform.position.z);

    transform.position = new Vector3(x, y, z);

    if (health != null)
    {
        health.currentHealth = health.maxHealth;
        health.onHealthChanged?.Invoke(1f);
    }

    isKnocked = false;
    isInvincible = false;
    rb.linearVelocity = Vector2.zero;
}
}