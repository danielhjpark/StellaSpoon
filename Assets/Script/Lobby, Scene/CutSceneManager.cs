using System.Collections;
using UnityEngine;
using TMPro;

public class CutSceneManager : MonoBehaviour
{
    [Header("UI References")]
    public CanvasGroup cutSceneCanvas;      // CutSceneScreen�� CanvasGroup ���̱�
    public TextMeshProUGUI[] storyTexts;    // StoryText �迭

    [Header("Timings")]
    public float cutSceneDuration = 5f;     // �ƽ� ��ü �ð�
    public float fadeDuration = 1.5f;       // ���̵� �ð�

    private bool isSkipping = false;
    public static bool isStory = false;
    /// <summary>
    /// SceneLoader���� ȣ��
    /// </summary>
    public void StartCutScene()
    {
        gameObject.SetActive(true);
        isSkipping = false;
        isStory = true;
        StartCoroutine(PlayCutScene());
    }

    private void Update()
    {
        // ESC�θ� ��ŵ
        if (Input.GetKeyDown(KeyCode.Escape) && !isSkipping)
        {
            isSkipping = true;
            StopAllCoroutines();
            StartCoroutine(FadeOutAndEndCutScene());
        }
    }

    private IEnumerator PlayCutScene()
    {
        // �ʱ� ����: ���� ��� + �ؽ�Ʈ ǥ��
        cutSceneCanvas.alpha = 1;

        foreach (var text in storyTexts)
            text.gameObject.SetActive(true);

        // �ƽ� ��ü ����
        yield return new WaitForSeconds(cutSceneDuration);

        // ���� �̺�Ʈ ����
        yield return StartCoroutine(FadeOutAndEndCutScene());
    }

    private IEnumerator FadeOutAndEndCutScene()
    {
        // �ؽ�Ʈ ����
        foreach (var text in storyTexts)
            text.gameObject.SetActive(false);

        // ���̵� �ƿ�
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            cutSceneCanvas.alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            yield return null;
        }
        isStory = false;
        // ��� ���� ȭ�� ����
        yield return new WaitForSeconds(0.5f);

        // �ƽ� ���� �� UI ��Ȱ��ȭ
        gameObject.SetActive(false);
    }
}