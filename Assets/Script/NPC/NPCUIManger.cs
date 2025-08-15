using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPCUIManger : MonoBehaviour
{
    [Header("Main UI")]
    [SerializeField] GameObject menuPanel;
    [Header("Sub UI")]
    [SerializeField] GameObject[] menuList;

    private Image[] menuImage;
    private TextMeshProUGUI[] menuText;

    private List<NPCBehavior> NPCBehaviors;
    private bool useCookScene;
    private bool[] useMenu = new bool[4];

    void Start()
    {
        menuImage = new Image[menuList.Length];
        menuText = new TextMeshProUGUI[menuList.Length];

        for (int i = 0; i < menuList.Length; i++)
        {
            menuImage[i] = menuList[i].GetComponentsInChildren<Image>().FirstOrDefault(img => img.gameObject != menuList[i]);
            menuText[i] = menuList[i].GetComponentInChildren<TextMeshProUGUI>();
        }
    }

    void Update()
    {
        CheckCookScene();
        UpdateNPCList();
        if (!useCookScene)
        {
            HideUI();
        }
        else if (useCookScene)
        {
            menuPanel.SetActive(true);
            if (NPCBehaviors.Count > 0) UpdateMenuList();
            else
            {
                VisibleUI();
            }
        }
    }

    void CheckCookScene()
    {
        if (CookSceneManager.instance.isSceneLoaded)
        {
            useCookScene = true;
        }
        else
        {
            useCookScene = false;
        }
    }

    public void UpdateNPCList()
    {
        NPCBehaviors = NpcManager.instance.npcList.Select(npc => npc.GetComponent<NPCBehavior>()).Where(b => b != null).ToList();
    }

    void UpdateMenuList()
    {
        Array.Fill(useMenu, false);
        foreach (var NPCBehavior in NPCBehaviors)
        {

            if (NPCBehavior.GetState() == NPCBehavior.NPCState.Sitting)
            {
                Recipe currentRecipe = NPCBehavior.GetRecipe();
                int seatIndex = NPCBehavior.GetSeatIndex();
                menuImage[seatIndex].sprite = currentRecipe.menuImage;
                menuText[seatIndex].text = currentRecipe.menuName;
                useMenu[seatIndex] = true;
            }
        }

        for (int i = 0; i < useMenu.Length; i++)
        {
            menuList[i].SetActive(useMenu[i]);
            if (useMenu[i])
            {
                menuList[1].transform.SetAsLastSibling();
            }
        }
        

    }

    void VisibleUI()
    {
        menuPanel.SetActive(true);
        for (int i = 0; i < useMenu.Length; i++)
        {
            menuList[i].SetActive(false);
        }
    }

    void HideUI()
    {
        //Cook scene is not active, hide menuPanel UI
        /*if()*/
        menuPanel.SetActive(false);
    }
}
