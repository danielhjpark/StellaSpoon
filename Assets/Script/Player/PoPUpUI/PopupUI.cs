using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopupUI : MonoBehaviour
{
    public TextMeshProUGUI messageText;
    public CanvasGroup canvasGroup;

    private float duration = 2f;
    private float fadeDuration = 0.5f;

    public void Setup(string message, float duration)
    {
        this.duration = duration;
        messageText.text = message;
        StartCoroutine(FadeOutRoutine());
    }

    private IEnumerator FadeOutRoutine()
    {
        yield return new WaitForSeconds(duration);

        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            yield return null;
        }

        Destroy(gameObject);
    }
}
