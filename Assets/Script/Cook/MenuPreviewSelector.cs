using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPreviewSelector : MonoBehaviour
{
    [SerializeField] GameObject DescriptionParent;
    [SerializeField] GameObject ObjectParent;

    Dictionary<string, int> RecipeList = new Dictionary<string, int>();

    public int selectNum;
    private int previousNum;

    void Awake()
    {
        SetupRecipeList();
    }

    void Start()
    {
        selectNum = 0;
        previousNum = 0;
        
    }

    void SetupRecipeList()
    {
        for (int i = 0; i < DescriptionParent.transform.childCount; i++)
        {
            string recipeName = DescriptionParent.transform.GetChild(i).name;
            RecipeList.Add(recipeName, i);
        }
            
    }

    public void CloseAllMenu()
    {
        for (int i = 0; i < DescriptionParent.transform.childCount; i++)
        {
            DescriptionParent.transform.GetChild(i).gameObject.SetActive(false);
            ObjectParent.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void SelectMenu(string selectName)
    {
        int selectNum = RecipeList[selectName];
        if (selectNum >= DescriptionParent.transform.childCount) return;
        else CloseAllMenu();

        DescriptionParent.transform.GetChild(selectNum).gameObject.SetActive(true);
        ObjectParent.transform.GetChild(selectNum).gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            selectNum++;
             if (selectNum >= DescriptionParent.transform.childCount) selectNum = 0;
        }
        //SelectMenu();
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
