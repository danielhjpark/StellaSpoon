using System.Collections;
using System.Collections.Generic;
using Unity.Android.Types;
using UnityEngine;
using UnityEngine.UI;

public class FadeEffect : MonoBehaviour
{
    public static FadeEffect Instance;

    [SerializeField]
    [Range(0.01f, 10f)]
    private float fadeTime;
    private Image image;

    private void Awake()
    {
        Instance = this; // ΩÃ±€≈Ê √ ±‚»≠
        image = GetComponent<Image>();
    }

    public IEnumerator Fade(float start, float end)
    {
        float currentTime = 0.0f;
        float percent = 0.0f;

        while (percent < 1)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / fadeTime;

            Color color = image.color;
            color.a = Mathf.Lerp(start, end, percent);
            image.color = color;

            yield return null;
        }
    }
}
