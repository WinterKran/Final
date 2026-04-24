using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    public UnityEvent<float> onHealthChanged;
    public UnityEvent onDeath;

    void Start()
    {
        currentHealth = maxHealth;
        onHealthChanged?.Invoke(currentHealth / maxHealth);
    }

    public virtual void TakeDamage(float dmg, Vector2 sourcePos)
    {
        ApplyDamage(dmg);
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        onHealthChanged?.Invoke(currentHealth / maxHealth);
    }

    protected void ApplyDamage(float dmg)
    {
        currentHealth -= dmg;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        onHealthChanged?.Invoke(currentHealth / maxHealth);

        if (currentHealth <= 0)
            Die();
    }

    protected virtual void Die()
    {
        onDeath?.Invoke();

        if (CompareTag("Player"))
        {
            GetComponent<PlayerMovement>().Respawn();
            currentHealth = maxHealth;
            return;
        }

        Destroy(gameObject);
    }
}