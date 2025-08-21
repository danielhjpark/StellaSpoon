using System.Collections;
using UnityEngine;
using TMPro;

public class CutSceneManager : MonoBehaviour
{
    [Header("UI References")]
    public CanvasGroup cutSceneCanvas;      // CutSceneScreen에 CanvasGroup 붙이기
    public TextMeshProUGUI[] storyTexts;    // StoryText 배열

    [Header("Timings")]
    public float cutSceneDuration = 5f;     // 컷신 전체 시간
    public float fadeDuration = 1.5f;       // 페이드 시간

    private bool isSkipping = false;
    public static bool isStory = false;
    /// <summary>
    /// SceneLoader에서 호출
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
        // ESC로만 스킵
        if (Input.GetKeyDown(KeyCode.Escape) && !isSkipping)
        {
            isSkipping = true;
            StopAllCoroutines();
            StartCoroutine(FadeOutAndEndCutScene());
        }
    }

    private IEnumerator PlayCutScene()
    {
        // 초기 상태: 검은 배경 + 텍스트 표시
        cutSceneCanvas.alpha = 1;

        foreach (var text in storyTexts)
            text.gameObject.SetActive(true);

        // 컷신 전체 유지
        yield return new WaitForSeconds(cutSceneDuration);

        // 종료 이벤트 실행
        yield return StartCoroutine(FadeOutAndEndCutScene());
    }

    private IEnumerator FadeOutAndEndCutScene()
    {
        // 텍스트 끄기
        foreach (var text in storyTexts)
            text.gameObject.SetActive(false);

        // 페이드 아웃
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            cutSceneCanvas.alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            yield return null;
        }
        isStory = false;
        // 잠시 검은 화면 유지
        yield return new WaitForSeconds(0.5f);

        // 컷신 종료 → UI 비활성화
        gameObject.SetActive(false);
    }
}