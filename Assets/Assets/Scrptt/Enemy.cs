using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 20;

    public Deck playerDeck;

    [Header("AI")]
    public Transform player;
    public float moveSpeed = 3f;
    public float attackRange = 1.5f;
    public float attackCooldown = 1f;
    public float damage = 10f;

    private float attackTimer;

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        // 🎯 เดินเข้าหา
        if (distance > attackRange)
        {
            MoveToPlayer();
        }
        else
        {
            Attack();
        }
    }

    void MoveToPlayer()
    {
        Vector2 dir = (player.position - transform.position).normalized;

        transform.position += (Vector3)dir * moveSpeed * Time.deltaTime;

        // 🔄 หันหน้า
        if (dir.x != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(dir.x), 1, 1);
        }
    }

    void Attack()
    {
        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0)
        {
            attackTimer = attackCooldown;

            Health hp = player.GetComponent<Health>();
            if (hp != null)
            {
                hp.TakeDamage(damage);
                Debug.Log("Enemy hit player!");
            }
        }
    }

    // 💥 โดนตี
    public void TakeDamage(float dmg)
    {
        health -= Mathf.RoundToInt(dmg);

        if (health <= 0)
        {
            DropCards();
            Destroy(gameObject);
        }
    }

    void DropCards()
    {
        if (playerDeck == null)
        {
            Debug.LogWarning("Player Deck not assigned!");
            return;
        }

        for (int i = 0; i < 3; i++)
        {
            Card c = playerDeck.GetRandomCard(); // 👈 แก้แล้ว

            if (c != null)
            {
                playerDeck.AddCardToDeck(c);
                Debug.Log("Player got card: " + c.cardName);
            }
        }
    }
}