using UnityEngine;
using UnityEngine.UI;

public class BossUI : MonoBehaviour
{
    public Slider hpBar;
    public Health bossHealth;

    void Start()
    {
        hpBar.value = 1f;
        bossHealth.onHealthChanged.AddListener(UpdateHP);
    }

    void UpdateHP(float percent)
    {
        hpBar.value = percent;
    }
}