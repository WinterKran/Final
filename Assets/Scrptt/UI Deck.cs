using UnityEngine;
using TMPro;

public class DeckUI : MonoBehaviour
{
    public Deck deck;
    public TextMeshProUGUI deckText;

    void Update()
    {
        if (deck == null || deckText == null) return;

        deckText.text = deck.deck.Count.ToString();
    }
}