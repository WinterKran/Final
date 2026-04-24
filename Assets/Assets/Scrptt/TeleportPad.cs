using System.Collections;
using UnityEngine;

public class TeleportPad : MonoBehaviour
{
    public Transform exitPoint;   // จุด Block Out
    public float cooldown = 0.5f;

    private bool canTeleport = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!canTeleport) return;

        if (other.CompareTag("Player"))
        {
            StartCoroutine(Teleport(other.transform));
        }
    }

    IEnumerator Teleport(Transform player)
    {
        canTeleport = false;

        // ย้ายตำแหน่ง
        player.position = exitPoint.position;

        // กันวาร์ปเด้งกลับทันที
        yield return new WaitForSeconds(cooldown);

        canTeleport = true;
    }
}