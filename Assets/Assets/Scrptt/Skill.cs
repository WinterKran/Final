using UnityEngine;

public class Skill : MonoBehaviour
{
    private float damage;
    private Vector2 direction;
    private SkillType type;

    public float speed = 10f;

    private Rigidbody2D rb;

    private float lifeTime;

    private AudioSource audioSource;
    private AudioClip sound;
    private AudioClip hitSound;

    public void Init(Card card, Vector2 dir)
    {
        damage = card.damage;
        direction = dir.normalized;
        type = card.skillType;
        sound = card.sound;
        hitSound = card.hitSound;// 🔊 รับเสียงจากการ์ด

        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        transform.rotation = Quaternion.Euler(0, 0,
            Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);

        // 🔊 เล่นเสียงตอนสร้างสกิล
        if (sound != null && audioSource != null)
        {
            audioSource.PlayOneShot(sound);
        }

        switch (type)
        {
            case SkillType.Projectile:
                rb.linearVelocity = direction * speed;
                break;

            case SkillType.AreaHit:
                lifeTime = 3f;
                break;

            case SkillType.Trap:
                lifeTime = 10f;
                break;

            case SkillType.Dash:
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

    void PlayHitSound()
    {
        if (hitSound != null)
        {
            AudioSource.PlayClipAtPoint(hitSound, transform.position);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
{
    Vector2 hitPos = transform.position;

    if (other.CompareTag("Enemy"))
    {
        var hp = other.GetComponent<Enemy>();
        if (hp != null) hp.TakeDamage(damage);

            PlayHitSound();
       }

    if (other.CompareTag("Boss"))
    {
        var hp = other.GetComponent<Boss>();
        if (hp != null) hp.TakeDamage(damage, hitPos);

            PlayHitSound();
        }

    if (other.CompareTag("Boss2"))
    {
        var hp = other.GetComponent<Boss2>();
        if (hp != null) hp.TakeDamage(damage, hitPos);

            PlayHitSound();
        }

    if (other.CompareTag("Boss3"))
    {
        var hp = other.GetComponent<Boss3>();
        if (hp != null) hp.TakeDamage(damage, hitPos);
            PlayHitSound();
        }

    if (type == SkillType.Trap && other.CompareTag("Player"))
    {
        Debug.Log("Player stepped on trap");
    }
}
}