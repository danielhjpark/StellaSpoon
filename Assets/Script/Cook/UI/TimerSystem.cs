using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class TimerSystem : MonoBehaviour
{
    [SerializeField] Image timerGague;
    [SerializeField] bool onBillboard;
    bool isTimerEnd = false;

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

    public IEnumerator TimerStart(float second)
    {
        float secondValue = 1 /(second * 20);
        isTimerEnd = false;
        timerGague.fillAmount = 1;
        while (timerGague.fillAmount >= 0.1f)
        {
            timerGague.fillAmount -= secondValue;
            yield return new WaitForSeconds(0.05f);
        }
        timerGague.fillAmount = 1;
        isTimerEnd = true;
        //StartCoroutine(TimerReset());
    }

    public void TimerReset()
    {
        timerGague.fillAmount = 1;
    }

    public bool TimerEnd()
    {
        return isTimerEnd;
    }
}
