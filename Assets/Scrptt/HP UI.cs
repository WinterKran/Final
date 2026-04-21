using UnityEngine;
using UnityEngine.UI;

public class HPUI : MonoBehaviour
{
    public Health targetHealth;
    public Image hpBar;

    void OnEnable()
    {
        if (targetHealth != null)
        {
            targetHealth.onHealthChanged.AddListener(UpdateBar);
            UpdateBar(targetHealth.currentHealth / targetHealth.maxHealth);
        }
    }

    void UpdateBar(float percent)
    {
        if (hpBar == null) return;

        hpBar.fillAmount = percent;
    }
}