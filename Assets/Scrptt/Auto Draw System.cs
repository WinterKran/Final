using UnityEngine;

public class AutoDraw : MonoBehaviour
{
    public Deck deck;
    public Hand hand;

    public float drawInterval = 2f;
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= drawInterval)
        {
            timer = 0;

            if (hand.cards.Count < hand.maxHand)
            {
                hand.AddCard(deck.Draw());
            }
        }
    }
}