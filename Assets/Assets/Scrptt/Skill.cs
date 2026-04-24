using UnityEngine;

public class Skill : MonoBehaviour
{
    private float damage;
    private Vector2 direction;
    private SkillType type;

    public float speed = 10f;

    private Rigidbody2D rb;

    private float lifeTime;

    public void Init(Card card, Vector2 dir)
    {
        damage = card.damage;
        direction = dir.normalized;
        type = card.skillType;

        rb = GetComponent<Rigidbody2D>();

        transform.rotation = Quaternion.Euler(0, 0,
            Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);

        switch (type)
        {
            case SkillType.Projectile:
                rb.linearVelocity = direction * speed;
                break;

            case SkillType.AreaHit:
                lifeTime = 1f;
                break;

            case SkillType.Trap:
                lifeTime = 10f;
                break;

            case SkillType.Dash:
                // ไม่ใช้ใน skill object
                break;
        }
    }

    void Update()
    {
        switch (type)
        {
            case SkillType.AreaHit:
            case SkillType.Trap:
                lifeTime -= Time.deltaTime;
                if (lifeTime <= 0)
                    Destroy(gameObject);
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("Enemy"))
    {
        other.GetComponent<Enemy>().TakeDamage(damage);
    }

    if (other.CompareTag("Boss"))
    {
        other.GetComponent<Boss>().TakeDamage(damage);
    }

    if (type == SkillType.Trap && other.CompareTag("Player"))
    {
        Debug.Log("Player stepped on trap");
    }
}
}