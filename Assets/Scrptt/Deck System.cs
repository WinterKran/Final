using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    [Header("Main Deck")]
    public List<Card> deck = new List<Card>();

    [Header("Discard Pile")]
    public List<Card> discard = new List<Card>();

    [Header("Card Pool")]
    public List<Card> cardPool = new List<Card>();

    // 🎴 จั่วการ์ด
    public Card Draw()
    {
        if (deck.Count == 0)
        {
            Reshuffle();
        }

        if (deck.Count == 0) return null;

        Card c = deck[0];
        deck.RemoveAt(0);

        return c;
    }

    // ➕ เพิ่มเข้ากอง
    public void AddCardToDeck(Card card)
    {
        deck.Add(card);
    }

    // ♻️ ย้าย discard กลับ deck
    public void Reshuffle()
    {
        deck.AddRange(discard);
        discard.Clear();

        Shuffle(deck);
    }

    // 🎲 สุ่ม list
    void Shuffle(List<Card> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Card temp = list[i];
            int rand = Random.Range(i, list.Count);
            list[i] = list[rand];
            list[rand] = temp;
        }
    }

    // 💥 ได้การ์ดใหม่
    public void GainRandomCard()
    {
        if (cardPool.Count == 0) return;

        Card c = cardPool[Random.Range(0, cardPool.Count)];

        deck.Add(c);

        Debug.Log("Gained card: " + c.cardName);
    }

    // 🗑 ใช้แล้วทิ้ง
    public void Discard(Card card)
    {
        discard.Add(card);
    }

    public Card GetRandomCard()
{
    if (cardPool == null || cardPool.Count == 0) return null;

    return cardPool[Random.Range(0, cardPool.Count)];
}
}