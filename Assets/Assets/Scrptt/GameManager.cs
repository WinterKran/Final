using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Fade")]
    public CanvasGroup fadeCanvas;
    public float fadeSpeed = 2f;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void RespawnPlayer(PlayerMovement player)
    {
        StartCoroutine(RespawnRoutine(player));
    }

    IEnumerator RespawnRoutine(PlayerMovement player)
    {
        // 1. Fade out
        yield return StartCoroutine(Fade(1));

        // 2. Reset player
        player.RespawnInternal();

        // 3. Reset bosses
        ResetAllBosses();

        // 4. Fade in
        yield return StartCoroutine(Fade(0));
    }

    public void ResetAllBosses()
{
    Boss[] bosses = FindObjectsOfType<Boss>(true);

    foreach (Boss b in bosses)
    {
        b.ResetBoss(false);
        b.RespawnBoss();
    }
}

    IEnumerator Fade(float target)
    {
        float start = fadeCanvas.alpha;

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * fadeSpeed;
            fadeCanvas.alpha = Mathf.Lerp(start, target, t);
            yield return null;
        }
    }
}