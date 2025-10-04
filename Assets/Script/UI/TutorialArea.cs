using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialArea : MonoBehaviour
{
    [SerializeField] int tutorialTypeNum;

    private void OnTriggerEnter(Collider other)
    {
        TutorialManger.instance.OpenTutorialUI();
        TutorialManger.instance.SelectTutorial((TutorialManger.TutorialType)tutorialTypeNum);
        Destroy(this.gameObject);
    }
}
