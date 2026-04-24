using UnityEngine;

public class BossHitbox : MonoBehaviour
{
    public float damage = 10f;
    public float speed = 8f;
    public float lifeTime = 2f;

    private Vector2 direction;

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var hp = other.GetComponent<Health>();

        if (hp != null && other.CompareTag("Player"))
        {
            hp.TakeDamage(damage, transform.position);
        }
    }
}