using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPreviewSelector : MonoBehaviour
{
    [SerializeField] GameObject DescriptionParent;
    [SerializeField] GameObject ObjectParent;

    public int selectNum;
    private int previousNum;
    
    void Start()
    {
        selectNum = 0;
        previousNum = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            selectNum++;
             if (selectNum >= DescriptionParent.transform.childCount) selectNum = 0;
        }
        SelectMenu();
    }

    void SelectMenu()
    {
        if (selectNum >= DescriptionParent.transform.childCount) return;
        
        if (selectNum != previousNum)
            {
                DescriptionParent.transform.GetChild(previousNum).gameObject.SetActive(false);
                ObjectParent.transform.GetChild(previousNum).gameObject.SetActive(false);
                previousNum = selectNum;
            }

        DescriptionParent.transform.GetChild(selectNum).gameObject.SetActive(true);
        ObjectParent.transform.GetChild(selectNum).gameObject.SetActive(true);
    }
}
