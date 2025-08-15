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
            && sleepCoroutine == null && DeviceManager.isDeactived) //UI�� �����ְ� �ֺ� �÷��̾ �ְ� FŰ ������ ��
        {
            if (RestaurantOpenSystem.isRestaurantOpened)
            {
                InteractUIManger.instance.UsingText("�Ĵ��� ���� ���Դϴ�.", true);
                return;
            }
            else if (delayTime <= 1800)
            {
                InteractUIManger.instance.UsingText("���� ������ �ʽ��ϴ�.", true);
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
        float elapsedTime = 0f; // ���� ��� �ð�
        float fadedTime = 3f; // �� �ҿ� �ð�

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
        float elapsedTime = 0f; // ���� ��� �ð�
        float fadedTime = 0.5f; // �� �ҿ� �ð�

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
        // if (InteractUIManger.isPlayerNearby && Input.GetKeyDown(KeyCode.F)) //UI�� �����ְ� �ֺ� �÷��̾ �ְ� FŰ ������ ��
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
