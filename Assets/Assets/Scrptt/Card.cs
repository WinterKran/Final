using UnityEngine;

public enum CardType
{
    Slash,
    Dash,
    Fireball,
    Fire
}

public enum SkillType
{
    Projectile,
    AreaHit,
    Dash,
    Trap
}

[CreateAssetMenu(fileName = "NewCard", menuName = "Card")]
public class Card : ScriptableObject
{
    public string cardName;

    public CardType type;

    public SkillType skillType;   // 👈 ตัวควบคุมหลัก

    public float damage;
    public float cooldown;

    public Sprite icon;

    public GameObject skillPrefab;

    public Vector2 hitBoxSize;
}