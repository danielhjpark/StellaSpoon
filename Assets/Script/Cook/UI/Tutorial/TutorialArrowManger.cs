using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TutorialArrowManger : MonoBehaviour
{
    [SerializeField] GameObject[] ArrowObject;
    TutorialArrow[] tutorialArrows;

    void Awake()
    {
        CutSceneManager.OnCutSceneStart += ResetAllTutorialArrows;
    }

    public void MakeTutorialArrow(int ArrowNum)
    {
        string key = $"Arrow_{ArrowNum}";
        PlayerPrefs.SetInt(key, 1);
        PlayerPrefs.Save();
    }

    public void ResetTutorialArrow(int ArrowNum)
    {
        string key = $"Arrow_{ArrowNum}";
        PlayerPrefs.DeleteKey(key);
    }

    public void ResetAllTutorialArrows()
    {
        for (int i = 0; i < ArrowObject.Length; i++)
        {
            ResetTutorialArrow(i);
        }   
    }

    public bool ShouldShowTutorialArrow(int ArrowNum)
    {
        string key = $"Arrow_{ArrowNum}";
        return PlayerPrefs.GetInt(key, 0) == 0;
    }

    void Start()
    {
        tutorialArrows = ArrowObject
            .Select(obj => obj.GetComponent<TutorialArrow>())
            .Where(comp => comp != null) 
            .ToArray();
        foreach (var tutorialArrow in tutorialArrows) tutorialArrow.OnArrowDisable += ArrowCheck;


        for (int i = 0; i < ArrowObject.Length; i++)
        {
            if (ShouldShowTutorialArrow(i))
            {
                ArrowObject[i].SetActive(true);
            }
            else ArrowObject[i].SetActive(false);
        }
    }

    void ArrowCheck()
    {
        for (int i = 0; i < ArrowObject.Length; i++)
        {
            if (!ArrowObject[i].activeSelf)
            {
                MakeTutorialArrow(i);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
