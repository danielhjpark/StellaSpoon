using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class TimerSystem : MonoBehaviour
{
    [SerializeField] Image timerGague;
    [SerializeField] bool onBillboard;
    [NonSerialized] public bool antiClockwise;
    bool isTimerEnd = false;
    Coroutine timerCoroutine;
    void Start() {
        antiClockwise = false;
    }

    void Update()
    {
        TimerBillboard();
    }
    
    private void TimerBillboard() {
        if (onBillboard && Camera.main != null)
        {
            Vector3 targetPosition = Camera.main.transform.position; // 바라볼 대상
            Vector3 direction = targetPosition - transform.position;
            direction.y = 0;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation;

        }
    }

    public IEnumerator TimerStart(float second) {
        timerCoroutine = StartCoroutine(TimerOperate(second));
        yield return timerCoroutine;
    }

    public IEnumerator TimerOperate(float second)
    {
        float secondValue;
        float targetValue;
        isTimerEnd = false;

        if(antiClockwise) {
            timerGague.fillAmount = 0;
            targetValue = 1;
            secondValue = 1 /(second * 20) * -1;
        }
        else {
            timerGague.fillAmount = 1;
            targetValue = 0;
            secondValue = 1 /(second * 20);
        }

        while(true)
        {
            if(Mathf.Abs(timerGague.fillAmount - targetValue) <= 0.01f) {
                break;
            }
            timerGague.fillAmount -= secondValue;
            yield return new WaitForSeconds(0.05f);
        }

        isTimerEnd = true;
    }

    public void TimerStop()
    {
        StopCoroutine(timerCoroutine);
        timerGague.fillAmount = 1;
    }

    public bool TimerEnd()
    {
        return isTimerEnd;
    }
}
