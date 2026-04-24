using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardUI : MonoBehaviour
{
    public Image cardImage;
    public TextMeshProUGUI cardName;

    private RectTransform rect;
    private Vector2 defaultPos;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        defaultPos = rect.anchoredPosition;
    }

    public void SetCard(Card card)
    {
        if (card == null)
        {
            cardName.text = "Empty";

            if (cardImage != null)
                cardImage.enabled = false;

            return;
        }

        cardName.text = card.cardName;

        if (cardImage != null)
        {
            cardImage.enabled = true;
            cardImage.sprite = card.icon;
        }

        // reset position ทุกครั้ง
        rect.anchoredPosition = defaultPos;
    }

    // 💥 เรียกตอน "เลือกการ์ด"
    public void SetSelected(bool isSelected)
    {
        if (rect == null) return;

        if (isSelected)
        {
            rect.anchoredPosition = defaultPos + new Vector2(0, 20f);
        }
        else
        {
            rect.anchoredPosition = defaultPos;
        }
    }
}