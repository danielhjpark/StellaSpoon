using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BearKingScenes : MonoBehaviour
{
    [SerializeField]
    private BoxCollider[] bearKingScenesCollider;

    [SerializeField]
    private Transform cinemaStartPoint;
    [SerializeField]
    private CinemachineVirtualCamera virtualCamera;
    [SerializeField]
    private Camera BossCamara;

    [SerializeField]
    private GameObject fadeCanvas;
    [SerializeField]
    private Image fadeImage;
    [SerializeField]
    private GameObject NameText;

    private bool isTriggered = false;

    private GameObject PlayerGroup;

    [SerializeField]
    private GameObject BossCameraPosition;
    private void Awake()
    {
        fadeCanvas = GameObject.Find("BossCanvas");
        fadeCanvas.SetActive(false);
        PlayerGroup = GameObject.FindGameObjectWithTag("PlayerGroup");
        // 모든 콜라이더를 트리거로 설정
        foreach (var col in bearKingScenesCollider)
        {
            col.isTrigger = true;
            // 각 콜라이더에 TriggerProxy 추가
            if (col.GetComponent<BearKingTriggerProxy>() == null)
            {
                var proxy = col.gameObject.AddComponent<BearKingTriggerProxy>();
                proxy.parent = this;
            }
        }
    }

    // Proxy에서 호출
    public void OnPlayerTriggered(GameObject player)
    {
        if (isTriggered) return;
        isTriggered = true;
        Time.timeScale = 0f;
        fadeCanvas.SetActive(true);
        StartCoroutine(PlayCinemachineScene());
    }

    private IEnumerator PlayCinemachineScene()
    {
        Debug.Log("Cinemachine Scene Start");

        // 1. 4초간 점점 까매짐 (페이드 아웃)
        if (fadeImage != null)
        {
            fadeImage.enabled = true;
            yield return StartCoroutine(Fade(0f, 1f, 4f));
        }
        BossCamara.enabled = true;
        PlayerGroup.SetActive(false);
        NameText.SetActive(true);

        // 2. virtualCamera Priority를 20으로
        virtualCamera.Priority = 20;

        // 3. 4초간 점점 밝아짐 (페이드 인)
        if (fadeImage != null)
        {
            yield return StartCoroutine(Fade(1f, 0f, 4f));
            fadeImage.enabled = false;
        }

        // Z축으로 15만큼 이동할 목표 위치를 설정합니다.
        Vector3 targetPosition = BossCameraPosition.transform.position;

        // 카메라의 이동을 위한 코루틴을 시작합니다.
        yield return StartCoroutine(MoveCameraTo(virtualCamera.transform, targetPosition, 2f)); // 2초간 이동

        // 5. 5초 대기
        yield return new WaitForSecondsRealtime(5f);

        fadeCanvas.SetActive(false);
        PlayerGroup.SetActive(true);
        NameText.SetActive(false);
        // 6. Priority를 0으로
        virtualCamera.Priority = 0;
        BossCamara.enabled = false;
        Time.timeScale = 1f;
    }
    private IEnumerator MoveCameraTo(Transform cameraTransform, Vector3 targetPosition, float duration)
    {
        float elapsedTime = 0f;
        Vector3 startPosition = cameraTransform.position;

        while (elapsedTime < duration)
        {
            cameraTransform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.unscaledDeltaTime; // Time.timeScale이 0일 때도 작동
            yield return null;
        }

        cameraTransform.position = targetPosition; // 정확한 위치로 설정
    }

    private IEnumerator Fade(float from, float to, float duration)
    {
        float elapsed = 0f;
        Color color = fadeImage.color;
        while (elapsed < duration)
        {
            float alpha = Mathf.Lerp(from, to, elapsed / duration);
            color.a = alpha;
            fadeImage.color = color;
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        color.a = to;
        fadeImage.color = color;
    }
}
