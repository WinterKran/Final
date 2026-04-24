using UnityEngine;
using UnityEngine.UI;

public class EnemyHPUI : MonoBehaviour
{
    public Slider hpBar;
    public Enemy enemy;

    void Start()
    {
        hpBar.value = 1f;
    }

    public void UpdateHP(float current, float max)
    {
        hpBar.value = current / max;
    }
}