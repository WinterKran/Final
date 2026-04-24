using UnityEngine;
using System.Collections;

public class Boss3 : Health
{
    public float dashSpeed = 15f;
    public float dashDuration = 0.4f;
    public float dashCooldown = 0.5f;
    public float dashDamage = 25f;

    Transform player;
    Rigidbody2D rb;

    bool isDead = false;
    bool isDashing = false;
    bool hitPlayer = false;

    public GameObject bulletPrefab;
    public Transform firePoint;

    public GameObject arenaWall;
int dashCount = 0;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        StartCoroutine(BossLoop());
    }

    IEnumerator BossLoop()
    {
        while (!isDead)
        {
            yield return Dash();
            yield return new WaitForSeconds(dashCooldown);
        }
    }

    IEnumerator Dash()
    {
        Collider2D col = GetComponent<Collider2D>();

        isDashing = true;
        hitPlayer = false;

        // 👇 ทะลุ player
        col.isTrigger = true;

        Vector2 dir = (player.position - transform.position).normalized;
        rb.linearVelocity = dir * dashSpeed;

        yield return new WaitForSeconds(dashDuration);

       rb.linearVelocity = Vector2.zero;

col.isTrigger = false;
isDashing = false;

// ✅ นับ dash
dashCount++;

if (dashCount >= 3)
{
    ShootCircle(); // ยิงกระสุน
    dashCount = 0;
}
    }

    public override void TakeDamage(float dmg, Vector2 sourcePos)
    {
        if (isDead) return;

        base.TakeDamage(dmg, sourcePos);

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;
        Destroy(gameObject);

        GameObject wall = GameObject.FindGameObjectWithTag("Wall");
    if (wall != null)
        Destroy(wall);

    Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
{
    if (!isDashing || hitPlayer) return;

    if (col.CompareTag("Player"))
    {
        Health hp = col.GetComponent<Health>();

        if (hp != null)
        {
            hp.TakeDamage(dashDamage, transform.position);
            hitPlayer = true;
        }
    }
}

void ShootCircle()
{
    int bulletCount = 8;

    for (int i = 0; i < bulletCount; i++)
    {
        float angle = i * 360f / bulletCount;
        Vector2 dir = Quaternion.Euler(0, 0, angle) * Vector2.right;

        GameObject b = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        b.GetComponent<Bullet>().SetDirection(dir);
    }
}
public void ResetBoss(bool countAsKill)
{
    StopAllCoroutines();

    if (!countAsKill)
    {
        // ❌ ไม่ถือว่าตาย
        isDead = false;
    }

    gameObject.SetActive(false);
}
}