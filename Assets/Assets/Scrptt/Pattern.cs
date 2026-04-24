using UnityEngine;
using System.Collections;

public class Boss : Health
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float dashSpeed = 10f;

    public GameObject arenaWall;

    Transform player;
    Coroutine bossRoutine;
    bool isDead = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        bossRoutine = StartCoroutine(BossLoop());
    }

    IEnumerator BossLoop()
    {
        while (!isDead)
        {
            yield return Attack1();
            yield return Attack2();
            yield return Dash();
            yield return new WaitForSeconds(1f);
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
        Vector2 dir = (player.position - transform.position).normalized;
        float t = 0;

        while (t < 0.5f && !isDead)
        {
            transform.Translate(dir * dashSpeed * Time.deltaTime);
            t += Time.deltaTime;
            yield return null;
        }
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

    public override void TakeDamage(float dmg)
    {
        if (isDead) return;

        base.TakeDamage(dmg);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
{
    if (isDead) return;

    isDead = true;

    GameObject wall = GameObject.FindGameObjectWithTag("Wall");
    if (wall != null)
        Destroy(wall);

    Destroy(gameObject);
}
}