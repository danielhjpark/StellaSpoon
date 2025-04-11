using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DailyMenuUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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
    public Recipe currentMenu;
    int currentAmount;

    void Start()
    {
        currentAmount = 0;
        isEnter = false;
        isCanAddMenu = true;
    }

    public void Update() {
        if(currentMenu != null) {
            selectMenuCount.text = DailyMenuManager.dailyMenuList[currentMenu].ToString();
            currentAmount = DailyMenuManager.dailyMenuList[currentMenu];
        }
        if(currentAmount <= 0 && currentMenu != null) RemoveMenu();
        CancleMenuCheck();  

    }

    public void AddMenu(Recipe recipe, int amount) {
        addMenu.SetActive(false);
        selectMenu.SetActive(true);

        selectMenuImage.sprite = recipe.menuImage;
        selectMenuName.text = recipe.menuName;
        selectMenuCount.text = DailyMenuManager.dailyMenuList[recipe].ToString();

        isCanAddMenu = false;
        currentMenu = recipe;
        currentAmount += amount;
    }

    public void RemoveMenu() {
        addMenu.SetActive(true);
        selectMenu.SetActive(false);
        DailyMenuManager.dailyMenuList.Remove(currentMenu);
        currentMenu = null;
        isCanAddMenu = true;
        currentAmount = 0;
    }

    void CancleMenu() {
        addMenu.SetActive(true);
        selectMenu.SetActive(false);
        
        RecipeManager.instance.RecallIngredientFromRecipe(currentMenu, currentAmount);
        RefrigeratorManager.instance.RecallIngredientToInventory(currentMenu, currentAmount);
        DailyMenuManager.dailyMenuList.Remove(currentMenu);
        currentMenu = null;
        isCanAddMenu = true;
        currentAmount = 0;
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
