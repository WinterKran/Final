using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public List<Card> cards = new List<Card>();
    public int maxHand = 3;

    public CardUI[] cardSlots;

    public PlayerCombat player;
    public Deck deck; // 👈 ต้องมีอันนี้

    private CardType lastUsedType;
    private int comboCount = 0;

    private int selectedIndex = 0;

    public float selectedOffsetY = 20f;

    public TMPro.TextMeshProUGUI comboText;
    public int maxCombo = 10;

    public Card fireSpecial;
    public Card crystalSpecial;
    public Card ShadowSpecial;

    private Dictionary<CardType, int> combo = new Dictionary<CardType, int>();

public Card specialCardReward;

    public void AddCard(Card card)
    {
        if (cards.Count >= maxHand) return;

        cards.Add(card);
        UpdateUI();
    }
public void UseCard(int index)
{
    if (index >= cards.Count) return;

    Card c = cards[index];
    cards.RemoveAt(index);

    player.UseCard(c);

    if (deck != null)
    {
        deck.Discard(c);
    }

    UpdateCombo(c); // 👈 เพิ่มตรงนี้

    UpdateUI();
}
    void UpdateUI()
{
    for (int i = 0; i < cardSlots.Length; i++)
    {
        if (i < cards.Count)
        {
            cardSlots[i].SetCard(cards[i]);

            // 💥 เพิ่มอันนี้
            cardSlots[i].SetSelected(i == selectedIndex);
        }
        else
        {
            cardSlots[i].SetCard(null);
            cardSlots[i].SetSelected(false);
        }
    }
}

    void UpdateCombo(Card usedCard)
{
    CardType type = usedCard.type;

    if (!combo.ContainsKey(type))
        combo[type] = 0;

    combo[type]++;

    UpdateComboUI();

    Debug.Log(type + " combo: " + combo[type]);

    if (combo[type] >= 5)
    {
        combo[type] = 0;
        GiveSpecialCard(type);
    }
}

void GiveSpecialCard(CardType type)
{
    Card reward = null;

    switch (type)
    {
        case CardType.Fire:
            reward = fireSpecial;
            break;

        case CardType.crystal:
            reward = crystalSpecial;
            break;

        case CardType.Shadow:
            reward = ShadowSpecial;
            break;
    }

    if (reward == null) return;

    Debug.Log("🔥 Special Card: " + type);

    AddCard(reward);
}

    void Update()
{
    if (Input.GetKeyDown(KeyCode.Alpha1)) SelectCard(0);
    if (Input.GetKeyDown(KeyCode.Alpha2)) SelectCard(1);
    if (Input.GetKeyDown(KeyCode.Alpha3)) SelectCard(2);

    if (Input.GetKeyDown(KeyCode.Q)) MoveSelect(-1);
    if (Input.GetKeyDown(KeyCode.E)) MoveSelect(1);

    if (Input.GetKeyDown(KeyCode.J)) UseCard(selectedIndex);
}
void MoveSelect(int dir)
{
    if (cards.Count == 0) return;

    selectedIndex += dir;

    if (selectedIndex < 0)
        selectedIndex = cards.Count - 1;

    if (selectedIndex >= cards.Count)
        selectedIndex = 0;

    UpdateUI();
}

void SelectCard(int index)
{
    if (index >= cards.Count) return;

    selectedIndex = index;
    UpdateUI();
}
string GetComboBar(CardType type)
{
    int value = combo.ContainsKey(type) ? combo[type] : 0;

    string bar = "";

    for (int i = 0; i < maxCombo; i++)
        bar += (i < value) ? "█" : "░";

    return bar + $" ({value}/{maxCombo})";
}

void UpdateComboUI()
{
    if (comboText == null) return;

    comboText.text =
        "Fire: " + GetComboBar(CardType.Fire) + "\n" +
        "crystal : " + GetComboBar(CardType.crystal) + "\n" +
        "Shadow: " + GetComboBar(CardType.Shadow);
}
}