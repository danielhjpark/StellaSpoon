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
    Color32 DangerColor = new Color32(255, 0, 0, 255);
    Color32 CautionColor = new Color32(255, 255, 0, 255);
    Color32 SafeColor = new Color32(0, 255, 0, 255);

    void Start()
    {
        StartCoroutine(TimerStart());
    }

    // Update is called once per frame
    void Update()
    {
        if(timerGague.fillAmount <= 0.2f) {
            clickHandImage.color = DangerColor;
            timerGague.color = DangerColor;
        }
        else if(timerGague.fillAmount <= 0.4f) {
            clickHandImage.color = CautionColor;
            timerGague.color = CautionColor;
        }
        else {
            clickHandImage.color = SafeColor;
            timerGague.color = SafeColor;
        }
        ClickHandUpdate();
    }

    void ClickHandUpdate() {
        float rotationValue = (1 - timerGague.fillAmount) * 360;

        clickHand.localRotation = Quaternion.Euler(0, 0, rotationValue);
    }

    IEnumerator TimerStart() {
        timerGague.fillAmount = 1;
        while(timerGague.fillAmount >= 0.01f) {
            timerGague.fillAmount -= Time.deltaTime / 10;
            yield return null;
        }
        StartCoroutine(TimerReset());
    }

    IEnumerator TimerReset() {
        while(timerGague.fillAmount <= 0.99f) {
            timerGague.fillAmount += Time.deltaTime / 2;
            yield return null;
        }
        StartCoroutine(TimerStart());
    }
}
