using System.Collections;
using System.Collections.Generic;
using cakeslice;
using UnityEngine;

public class PotUI : MonoBehaviour
{
    [SerializeField] cakeslice.OutlineEffect outlineEffect;
    [SerializeField] cakeslice.OutlineAnimation outlineAnimation;
    [SerializeField] GameObject lidButton;
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

        lidButton.SetActive(false);
    }

    public IEnumerator LinkTimerStart(float second) {
        //Enable
        potTimer.SetActive(true);
        mainTimer.SetActive(true);
        mainTimer.GetComponent<CanvasGroup>().alpha = 1.0f;
        
        //anti
        potTimerSystem.antiClockwise = true;
        mainTimerSystem.antiClockwise = true;

        //Timer Start
        StartCoroutine(potTimerSystem.TimerStart(second));
        yield return StartCoroutine(mainTimerSystem.TimerStart(second));

        //Disable
        mainTimer.GetComponent<CanvasGroup>().alpha = 0f;
        potTimer.SetActive(false);
        mainTimer.SetActive(false);
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

    public void VisibleLidButton() {
        lidButton.SetActive(true);
    }

    public void HideLidButton() {
        lidButton.SetActive(false);
        Destroy(outlineEffect);
        Destroy(outlineAnimation);
    }
}
