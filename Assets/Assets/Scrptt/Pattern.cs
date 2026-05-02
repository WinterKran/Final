using UnityEngine;
using System.Collections;

public class Boss : Health
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float dashSpeed = 10f;
    public GameObject victoryPanel;

    public GameObject arenaWall;

    Transform player;
    Coroutine bossRoutine;
    bool isDead = false;

    bool isDashing = false;

    Rigidbody2D rb;

    bool hitPlayer = false;
    

    bool phase2 = false;
    bool transitioning = false;
    public float phase2Health = 200f; // ปรับตามเกมคุณ


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        bossRoutine = StartCoroutine(BossLoop());
        rb = GetComponent<Rigidbody2D>();

        if (victoryPanel != null)
            victoryPanel.SetActive(false);
    }

    IEnumerator BossLoop()
{
    while (!isDead)
    {
        yield return Attack1();
        yield return Dash();
        yield return Attack2();

        yield return new WaitForSeconds(0.5f);
    }
}

    IEnumerator Attack1()
    {
        for (int i = 0; i < 3 && !isDead; i++)
        {
            ShootAtPlayer();
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator Attack2()
    {
        int bulletCount = 5;

        for (int i = 0; i < bulletCount && !isDead; i++)
        {
            float angle = i * 360f / bulletCount;
            Vector2 dir = Quaternion.Euler(0, 0, angle) * Vector2.right;
            Shoot(dir);
        }

        yield return new WaitForSeconds(2f);
    }

    IEnumerator Dash()
{
    isDashing = true;
    hitPlayer = false;

    Vector2 dir = (player.position - transform.position).normalized;

    rb.linearVelocity = dir * dashSpeed;

    yield return new WaitForSeconds(0.5f);

    rb.linearVelocity = dir * dashSpeed;


    isDashing = false;

}

    void ShootAtPlayer()
    {
        Vector2 dir = (player.position - firePoint.position).normalized;
        Shoot(dir);
    }

    void Shoot(Vector2 dir)
    {
        GameObject b = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        b.GetComponent<Bullet>().SetDirection(dir);
    }

    public override void TakeDamage(float dmg, Vector2 sourcePos)
{
    if (isDead) return;

    base.TakeDamage(dmg, sourcePos);

    if (!phase2 && currentHealth <= phase2Health)
    {
        StartCoroutine(EnterPhase2());
        return;
    }

    if (currentHealth <= 0)
        Die();
}

    void Die()
    {
        if (isDead) return;

        isDead = true;

        GameObject wall = GameObject.FindGameObjectWithTag("Wall");
        if (wall != null)
            Destroy(wall);

        // ✅ แสดง Victory Panel
        if (victoryPanel != null)
            victoryPanel.SetActive(true);

        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D col)
{
    if (!isDashing || hitPlayer) return;

    if (col.gameObject.CompareTag("Player"))
    {
        PlayerMovement player = col.gameObject.GetComponent<PlayerMovement>();

        if (player != null)
        {
            player.TakeDamage(20f, transform.position);
            hitPlayer = true; // ❗ กันตีซ้ำ
        }
    }
}

IEnumerator EnterPhase2()
{
    transitioning = true;

    // หยุดการโจมตีปกติ
    if (bossRoutine != null)
        StopCoroutine(bossRoutine);

    rb.linearVelocity = Vector2.zero;

    // 🔥 Attack2 10 ครั้งก่อนเปลี่ยนเฟส
    for (int i = 0; i < 3; i++)
    {
        yield return Attack2();
        yield return new WaitForSeconds(0.2f);
    }

    // รีเซ็ตเลือด
    currentHealth = phase2Health;
    
    onHealthChanged.Invoke(currentHealth / maxHealth);

    // เปิด Phase 2 settings
    phase2 = true;

    // Collider เป็น Trigger
    GetComponent<Collider2D>().isTrigger = true;

    // ปิดแรงโน้มถ่วง
    rb.gravityScale = 0;

    transitioning = false;

    // เริ่ม Phase 2 loop (คุณจะปรับเองได้)
    bossRoutine = StartCoroutine(BossPhase2Loop());
}
IEnumerator BossPhase2Loop()
{
    while (!isDead)
    {
        if (transitioning)
        {
            yield return null;
            continue;
        }

        int skill = Random.Range(0, 3); // 0-2

        switch (skill)
        {
            case 0:
                yield return Attack1();
                break;

            case 1:
                yield return Attack2();
                break;

            case 2:
                yield return Dash();
                break;
        }

        yield return new WaitForSeconds(Random.Range(0.3f, 1f));
    }
}
public void ResetBoss(bool countAsKill)
{
    StopAllCoroutines();

    isDead = false;
    phase2 = false;
    transitioning = false;

    currentHealth = maxHealth;

    rb.linearVelocity = Vector2.zero;

    gameObject.SetActive(false);
}

public void RespawnBoss()
{
    gameObject.SetActive(true);
    StartCoroutine(BossLoop());
}

}