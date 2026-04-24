using UnityEngine;

public class Attack : MonoBehaviour
{
    public int damage = 5;
    public float lifeTime = 0.2f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D col)
{
    Debug.Log("Hit: " + col.name);

    Enemy enemy = col.GetComponentInParent<Enemy>();

    if (enemy != null)
    {
        enemy.TakeDamage(damage);
    }
}
}