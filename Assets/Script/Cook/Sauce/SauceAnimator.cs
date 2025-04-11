using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LiquidVolumeFX;
using UnityEngine.Playables;

public class SauceAnimator : MonoBehaviour
{   
    [Header("Object")]
    [SerializeField] GameObject sauceContainer;
    [SerializeField] SauceSystem flowSauceSystem;
    [SerializeField] SauceSystem bottomSauceSystem;
    
    LiquidVolume flowSauce;
    Vector3 startRotation = new Vector3(0, 0, 0);
    Vector3 endRotation = new Vector3(0, 0, 105f);
    
    void Start() {
        flowSauce = flowSauceSystem.liquidVolume;
        flowSauce.level = 0;
    }

    public void StartAnimation() {
        StartCoroutine(TiltSauceContainer());
    }

    public IEnumerator TiltSauceContainer() {
        float t = 0;
        while(true) {
            t += Time.deltaTime * CookManager.instance.tiltSauceContainerAcceleration;
            if(Vector3.Distance(sauceContainer.transform.localEulerAngles, endRotation) < 2) break;
            sauceContainer.transform.localEulerAngles = Vector3.Lerp(startRotation, endRotation, t);
            yield return null;
        }
        yield return StartCoroutine(AddSauce());
    }

    IEnumerator AddSauce() {
        float increaseLevelValue = 0.005f * CookManager.instance.SauceAcceleration;
        
        while(true) {
            if(flowSauce.level >= 0.95f) break;
            flowSauce.level += increaseLevelValue;
            yield return null;
        }
        yield return StartCoroutine(bottomSauceSystem.StartLiquidLevel());
        
        while(true) {
            if(flowSauce.level <= 0.01f) {
                flowSauce.level = 0;
                break;
            } 
            flowSauce.level -= increaseLevelValue;
            yield return null;
        }

        float t = 0;
        while(true) {
            if(Vector3.Distance(sauceContainer.transform.localEulerAngles, startRotation) < 2) {
                
                break;    
            }
            t += Time.deltaTime;
            sauceContainer.transform.localEulerAngles = Vector3.Lerp(endRotation , startRotation, t);
            yield return null;
        }
        
        bottomSauceSystem.IsLiquidFilled(true);
    }

}
