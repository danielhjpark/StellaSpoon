using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SleepBed : InteractObject
{
    [SerializeField] GameObject fadePanel;
    [SerializeField] GameTimeManager gameTimeManager;

    bool isPlayerNearby;
    Coroutine sleepCoroutine;
    int sleepTime = 5;
    float delayTime = 1800;

    // Update is called once per frame
    void Update()
    {
        delayTime += Time.deltaTime * 60f;
        if (gameTimeManager == null) gameTimeManager = FindObjectOfType<GameTimeManager>();
        if (isPlayerNearby && !InteractUIManger.isUseInteractObject && Input.GetKeyDown(KeyCode.F)
            && sleepCoroutine == null && DeviceManager.isDeactived) //UI가 닫혀있고 주변 플레이어가 있고 F키 눌렀을 때
        {
            if (RestaurantOpenSystem.isRestaurantOpened)
            {
                InteractUIManger.instance.UsingText("식당이 오픈 중입니다.", true);
                return;
            }
            else if (delayTime <= 1800)
            {
                InteractUIManger.instance.UsingText("아직 졸리지 않습니다.", true);
                return;
            }
            sleepCoroutine = StartCoroutine(UseSleepBed());
        }
    }

    IEnumerator UseSleepBed()
    {
        gameTimeManager.AddTime(240);
        InteractUIManger.isUseInteractObject = true;
        fadePanel.SetActive(true);
        fadePanel.GetComponent<CanvasRenderer>().SetAlpha(0f);
        PlayAudio();

        yield return StartCoroutine(FadeOut());
        yield return StartCoroutine(FadeIn());

        InteractUIManger.instance.UsingText(InteractUIManger.TextType.Sleep);
        InteractUIManger.isUseInteractObject = false;
        fadePanel.SetActive(false);
        sleepCoroutine = null;
        StopAudio();
        delayTime = 0;
    }

    IEnumerator FadeOut()
    {
        float elapsedTime = 0f; // 누적 경과 시간
        float fadedTime = 3f; // 총 소요 시간

        while (elapsedTime <= fadedTime)
        {
            fadePanel.GetComponent<CanvasRenderer>().SetAlpha(Mathf.Lerp(0f, 1f, elapsedTime / fadedTime));

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        yield break;
    }

    IEnumerator FadeIn()
    {
        float elapsedTime = 0f; // 누적 경과 시간
        float fadedTime = 0.5f; // 총 소요 시간

        while (elapsedTime <= fadedTime)
        {
            fadePanel.GetComponent<CanvasRenderer>().SetAlpha(Mathf.Lerp(1f, 0f, elapsedTime / fadedTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        yield break;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InteractUIManger.isPlayerNearby = true;
            InteractUIManger.currentInteractObject = this.gameObject;
            isPlayerNearby = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // if (InteractUIManger.isPlayerNearby && Input.GetKeyDown(KeyCode.F)) //UI가 닫혀있고 주변 플레이어가 있고 F키 눌렀을 때
        // {
        //     StartCoroutine(UseSleepBed());
        // }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InteractUIManger.isPlayerNearby = false;
            isPlayerNearby = false;
        }
    }
}
