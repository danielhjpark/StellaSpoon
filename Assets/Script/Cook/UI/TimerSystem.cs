using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class TimerSystem : MonoBehaviour
{
    [SerializeField] Image timerGague;
    [SerializeField] RectTransform clickHand;
    [SerializeField] Image clickHandImage;
    [SerializeField] bool onBillboard;
    Color32 DangerColor = new Color32(255, 0, 0, 255);
    Color32 CautionColor = new Color32(255, 255, 0, 255);
    Color32 SafeColor = new Color32(0, 255, 0, 255);

    bool isTimerEnd = false;
    void Start()
    {
        //StartCoroutine(TimerStart());
    }
    void Update()
    {
        if (onBillboard && Camera.main != null)
        {
            Vector3 targetPosition = Camera.main.transform.position; // 바라볼 대상
            Vector3 direction = targetPosition - transform.position;
            direction.y = 0;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation;

            //this.transform.LookAt(this.transform.position + mainCam.rotation * Vector3.forward, mainCam.rotation * Vector3.up);
        }
        // if(timerGague.fillAmount <= 0.2f) {
        //     clickHandImage.color = DangerColor;
        //     timerGague.color = DangerColor;
        // }
        // else if(timerGague.fillAmount <= 0.4f) {
        //     clickHandImage.color = CautionColor;
        //     timerGague.color = CautionColor;
        // }
        // else {
        //     clickHandImage.color = SafeColor;
        //     timerGague.color = SafeColor;
        // }
        // ClickHandUpdate();
    }

    void ClickHandUpdate()
    {
        float rotationValue = (1 - timerGague.fillAmount) * 360;

        clickHand.localRotation = Quaternion.Euler(0, 0, rotationValue);
    }

    public IEnumerator TimerStart()
    {
        isTimerEnd = false;
        timerGague.fillAmount = 0;
        while (timerGague.fillAmount <= 0.99f)
        {
            timerGague.fillAmount += Time.deltaTime / 10;
            yield return null;
        }
        timerGague.fillAmount = 1;
        isTimerEnd = true;
        //StartCoroutine(TimerReset());
    }

    public void TimerReset()
    {
        timerGague.fillAmount = 0;
    }

    public bool TimerEnd()
    {
        return isTimerEnd;
    }
}
