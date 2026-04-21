using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Transform attackPoint;
    public GameObject slashPrefab;
    public GameObject fireballPrefab;
    private bool hasAttacked;

    public Vector2 lastDirection = Vector2.right;
    public float dashForce = 10f;

    private Rigidbody2D rb;
    private Animator animator;

    private Card currentCard;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Vector2 input = Vector2.zero;

        if (Input.GetKey(KeyCode.W)) input.y = 1;
        if (Input.GetKey(KeyCode.S)) input.y = -1;
        if (Input.GetKey(KeyCode.A)) input.x = -1;
        if (Input.GetKey(KeyCode.D)) input.x = 1;

        if (input != Vector2.zero)
        {
            lastDirection = input.normalized;
        }
    }

   public void UseCard(Card card)
{
    currentCard = card;

    if (attackPoint == null)
    {
        Debug.LogError("AttackPoint not assigned!");
        return;
    }

    if (card == null)
    {
        Debug.LogError("Card is NULL");
        return;
    }

    if (card.skillPrefab == null)
    {
        Debug.LogError("SkillPrefab missing in card: " + card.cardName);
        return;
    }

    if (card.skillType == SkillType.Dash)
{
    animator.SetTrigger("Dash");
    rb.linearVelocity = lastDirection * dashForce;
    return;
}

    GameObject skill = Instantiate(
        card.skillPrefab,
        attackPoint.position,
        Quaternion.identity
    );

    Skill s = skill.GetComponent<Skill>();

    if (s == null)
    {
        Debug.LogError("Skill script missing on prefab: " + card.cardName);
        return;
    }

    s.Init(card, lastDirection);
}

void Dash(Card card)
{
    rb.linearVelocity = lastDirection * dashForce;
}
}