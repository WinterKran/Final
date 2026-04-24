using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 10;

    private Vector2 direction;
    private Rigidbody2D rb;

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
        Destroy(gameObject, 5f);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Health hp = other.GetComponent<Health>();

            if (hp != null)
            {
                hp.TakeDamage(damage, transform.position);
            }

            Destroy(gameObject);
        }
    }
}