using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectMenuUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //---------------Swap Object--------------------//
    [SerializeField] GameObject addMenu;
    [SerializeField] GameObject selectMenu;

    //------------SelectMenu Setup -----------------//
    [SerializeField] Image selectMenuImage;
    [SerializeField] TextMeshProUGUI selectMenuName;
    [SerializeField] TextMeshProUGUI selectMenuCount;

    //----------- Cancle DailyMenu-----------------//
    [SerializeField] Image cancleGague;

    private bool isEnter;
    private bool isCanAddMenu;
    [SerializeField] DailyMenuSystem dailyMenuSystem;
    Recipe currentMenu;

    void Start()
    {
        isEnter = false;
        isCanAddMenu = true;
    }

    public void Update() {
        CancleMenuCheck();  
        if(currentMenu != null) {
            selectMenuCount.text = dailyMenuSystem.dailyMenuList[currentMenu].ToString();
        }
    }

    public void AddMenu(Recipe recipe) {
        addMenu.SetActive(false);
        selectMenu.SetActive(true);

        selectMenuImage.sprite = recipe.menuImage;
        selectMenuName.text = recipe.menuName;
        selectMenuCount.text = dailyMenuSystem.dailyMenuList[recipe].ToString();

        isCanAddMenu = false;
        currentMenu = recipe;
    }

    void CancleMenu() {
        addMenu.SetActive(true);
        selectMenu.SetActive(false);
        
        for(int i = 0; i <dailyMenuSystem.dailyMenuList[currentMenu]; i++) 
            RecipeManager.instance.RecallIngredientFromRecipe(currentMenu);
        dailyMenuSystem.dailyMenuList.Remove(currentMenu);
        currentMenu = null;
        isCanAddMenu = true;
    }
    
    // Update is called once per frame

    void CancleMenuCheck() {
        if(Input.GetMouseButton(0)) {
            if(isEnter && !isCanAddMenu && cancleGague.fillAmount <= 0.99f) {
                cancleGague.fillAmount += Time.deltaTime;
            }
            else if(isEnter&& !isCanAddMenu) {
                CancleMenu();
                isEnter = false;
                cancleGague.fillAmount = 0;
            }
        }
        else if(Input.GetMouseButton(1)) {
            
        }
        else cancleGague.fillAmount = 0;

    }


    public bool IsCanAddMenu() {
        return isCanAddMenu;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isEnter = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isEnter = false;
    }
}
