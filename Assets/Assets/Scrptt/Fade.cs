using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    public Image fadeImage;

    public void FadeOut(float duration)
    {
        StartCoroutine(Fade(0, 1, duration));
    }

    IEnumerator Fade(float start, float end, float duration)
    {
        float t = 0;
        Color c = fadeImage.color;

        while (t < duration)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(start, end, t / duration);
            fadeImage.color = new Color(c.r, c.g, c.b, a);
            yield return null;
        }
    }
}