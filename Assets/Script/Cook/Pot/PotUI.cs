using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotUI : MonoBehaviour
{
    [SerializeField] GameObject potTimer;
    GameObject mainTimer;
    TimerSystem mainTimerSystem;
    TimerSystem potTimerSystem;

    PotViewportSystem potViewportSystem;

    void Start()
    {
        mainTimer = GameObject.Find("PotTimer");
        potTimerSystem = potTimer.GetComponent<TimerSystem>();
        mainTimerSystem = mainTimer.GetComponent<TimerSystem>();
        potViewportSystem = GetComponent<PotViewportSystem>();
    }

    public IEnumerator LinkTimerStart() {
        potTimer.SetActive(true);
        mainTimer.SetActive(true);
        mainTimer.GetComponent<CanvasGroup>().alpha = 1.0f;
        StartCoroutine(potTimerSystem.TimerStart());
        yield return StartCoroutine(mainTimerSystem.TimerStart());

        mainTimer.GetComponent<CanvasGroup>().alpha = 0f;
        potTimer.SetActive(false);
        mainTimer.SetActive(false);

    }

    public IEnumerator TimerStart()
    {
        potTimer.SetActive(true);
        yield return StartCoroutine(potTimerSystem.TimerStart());
        potTimer.SetActive(false);
    }

    public void TimerReset()
    {
        potTimerSystem.TimerReset();
    }

    public bool TimerEnd()
    {
        return potTimerSystem.TimerEnd();
    }

    //---------------------ViewPort-----------------------------//

    public void SetActiveBottomButton() {
        potViewportSystem.bottomButton.SetActive(true);
    }

    public void SetActiveFrontButton() {
        potViewportSystem.frontButton.SetActive(true);
    }

}
