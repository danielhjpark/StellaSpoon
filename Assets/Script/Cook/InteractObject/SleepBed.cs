using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SleepBed : MonoBehaviour
{
    [SerializeField] GameObject fadePanel;
    [SerializeField] GameTimeManager gameTimeManager;

    bool isPlayerNearby;
    Coroutine sleepCoroutine;
    int sleepTime = 5;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerNearby && !InteractUIManger.isUseInteractObject && Input.GetKeyDown(KeyCode.F) && sleepCoroutine == null) //UI�� �����ְ� �ֺ� �÷��̾ �ְ� FŰ ������ ��
        {
            sleepCoroutine = StartCoroutine(UseSleepBed());
        }
    }

    IEnumerator UseSleepBed()
    {
        //Player lock moving or actions;
        gameTimeManager.AddTime(300);
        InteractUIManger.isUseInteractObject = true;

        fadePanel.SetActive(true);
        fadePanel.GetComponent<CanvasRenderer>().SetAlpha(0f);

        yield return StartCoroutine(FadeOut());
        yield return StartCoroutine(FadeIn());
        InteractUIManger.instance.UsingText(InteractUIManger.TextType.Sleep);
        InteractUIManger.isUseInteractObject = false;
        fadePanel.SetActive(false);
        sleepCoroutine = null;
    }

    IEnumerator FadeOut()
    {
        float elapsedTime = 0f; // ���� ��� �ð�
        float fadedTime = 2f; // �� �ҿ� �ð�

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
