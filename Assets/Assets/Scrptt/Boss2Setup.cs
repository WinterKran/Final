using UnityEngine;

public class Boss2Setup : MonoBehaviour
{
    public Boss2 boss;
    public Transform[] holes;
    public Transform attackPoint;

    void Start()
    {
        boss.holes = holes;
        boss.attackPoint = attackPoint;
    }
}