using UnityEngine;
using System.Collections;

public class Boss2 : Health
{
    public Transform player;

    [Header("Underground")]
    public Transform[] holes; // จุดโผล่ 3 จุด
    public GameObject emergeHitboxPrefab;
    public float undergroundDelay = 1f;

    [Header("Surface Attack")]
    public GameObject sideHitboxPrefab;
    public Transform attackPoint;
    public float attackDelay = 3f;

    [Header("Dash")]
    public float dashSpeed = 12f;
    public float dashTime = 0.5f;

    Rigidbody2D rb;

    bool isDead = false;
    bool isDashing = false;
    bool canBeHit = false;

    public Sprite undergroundSprite;
    public Sprite surfaceSprite;
    public Sprite dashSprite;

    SpriteRenderer sr;

    Vector2 dashDir;

    public GameObject arenaWall;

    void Start()
{
    rb = GetComponent<Rigidbody2D>();
    sr = GetComponent<SpriteRenderer>(); // 🔥 ต้องเพิ่มบรรทัดนี้

    StartCoroutine(FindPlayer());
    StartCoroutine(BossLoop());
}

IEnumerator FindPlayer()
{
    while (player == null)
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
            player = p.transform;

        yield return null;
    }
}

    IEnumerator BossLoop()
    {
        while (!isDead)
        {
            yield return UndergroundAttack();
            yield return SurfaceAttack();
            yield return DashAttack();

            yield return new WaitForSeconds(1f);
        }
    }

    // =========================
    // 🕳️ 1. มุดดิน
    // =========================
   IEnumerator UndergroundAttack()
{
    canBeHit = false;
    sr.sprite = undergroundSprite;

    for (int i = 0; i < 3; i++)
    {
        int rand = Random.Range(0, holes.Length);
        Transform hole = holes[rand];

        transform.position = hole.position;

        GameObject hit = Instantiate(emergeHitboxPrefab, hole.position, Quaternion.identity);

        // 🔥 ใส่ damage ให้ hitbox
        BossHitbox hb = hit.GetComponent<BossHitbox>();
        if (hb != null)
        {
            hb.damage = 15f;
        }

        yield return new WaitForSeconds(undergroundDelay);
    }

    int finalHole = Random.Range(0, holes.Length);
    transform.position = holes[finalHole].position;

    sr.sprite = surfaceSprite;

    canBeHit = true;

    yield return new WaitForSeconds(1f);
}

    // =========================
    // ⚔️ 2. โผล่มาตี
    // =========================
   IEnumerator SurfaceAttack()
{
    sr.sprite = surfaceSprite;

    yield return new WaitForSeconds(attackDelay);

    SpawnWave(Vector2.left);
    SpawnWave(Vector2.right);

    yield return new WaitForSeconds(0.5f);
}
void SpawnWave(Vector2 dir)
{
    GameObject wave = Instantiate(sideHitboxPrefab, attackPoint.position, Quaternion.identity);

    BossHitbox hb = wave.GetComponent<BossHitbox>();
    if (hb != null)
    {
        hb.damage = 20f;
        hb.SetDirection(dir);
    }

    Destroy(wave, 3f);
}

    // =========================
    // 💨 3. Dash
    // =========================
    IEnumerator DashAttack()
{
    isDashing = true;
    sr.sprite = dashSprite;

    if (player == null)
        yield break;

    float timer = 0f;

    while (timer < dashTime)
    {
        dashDir = (player.position - transform.position).normalized;

        rb.linearVelocity = dashDir * dashSpeed;

        if (dashDir.x != 0)
        {
            sr.flipX = dashDir.x < 0;
        }

        timer += Time.deltaTime;
        yield return null;
    }

    rb.linearVelocity = Vector2.zero;

    sr.sprite = surfaceSprite;
    isDashing = false;

    sr.color = Color.red;
    yield return new WaitForSeconds(0.3f);
    sr.color = Color.white;
}

    // =========================
    // 💥 ชน player ตอน dash
    // =========================
    void OnCollisionEnter2D(Collision2D col)
{
    if (!isDashing) return;

    if (col.gameObject.CompareTag("Player"))
    {
        var player = col.gameObject.GetComponent<Health>();

        if (player != null)
        {
            player.TakeDamage(20f, transform.position);
        }
    }
}

    // =========================
    // ❤️ โดนตีได้เฉพาะตอนโผล่
    // =========================
    

    public override void TakeDamage(float dmg, Vector2 sourcePos)
{
    if (!canBeHit || isDead) return;

    base.TakeDamage(dmg, sourcePos);

    if (currentHealth <= 0)
    {
        Die();
    }
}

void Die()
{
    isDead = true;
    StopAllCoroutines();
    Destroy(gameObject);

    GameObject wall = GameObject.FindGameObjectWithTag("Wall");
    if (wall != null)
        Destroy(wall);

    Destroy(gameObject);
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