using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LiquidVolumeFX;
using UnityEngine.Playables;

public class AddSauceAnimation : MonoBehaviour
{   
    [Header("TimeLine")]
    [SerializeField] PlayableDirector sauceContainerTimeLine;

    [Header("Liquid")]
    [SerializeField] LiquidVolume containerLiquid;
    [SerializeField] LiquidVolume flowLiquid;

    bool isStartTimeLine = false;
    public void StartAnimation() {
        StartCoroutine(TiltSauceContainer());
    }

    IEnumerator TiltSauceContainer() {
        while(true) {
            if(!isStartTimeLine) {
                isStartTimeLine = true;
                sauceContainerTimeLine.time = 0;
                sauceContainerTimeLine.Play();
            }

            if(sauceContainerTimeLine.time >= sauceContainerTimeLine.duration) {
                 break;
            }
            yield return null;
        }
        StartCoroutine(AddSauce());
    }

    IEnumerator AddSauce() {
        float increaseLevelValue = 0.05f;
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.1f);

        while(true) {
            flowLiquid.level += increaseLevelValue;
            yield return waitForSeconds;
        }
        
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
