using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class OrderManager : MonoBehaviour
{
    public static OrderManager instance;
    [SerializeField] NpcManager npcManager;

    Stack<Recipe> menuStack = new Stack<Recipe>();
    
    public bool isTimeOver;
    public bool isMenuSoldOut;
    private float startInterval = 0f;
    private float minSpwanInterval = 5f; //NPC 생성시간 최소
    private float maxSpwanInterval = 10f; //NPC 생성시간 최소

    private void Awake() {
        instance = this;
    }

    public void OpenRestaurant() {
        menuStack = CreateRandomStack(DailyMenuManager.dailyMenuList);
        StartCoroutine(StartRestaurant());
    }

    public void CloseRestaurant() {
        
    }

    IEnumerator StartRestaurant() {
        yield return new WaitForSeconds(startInterval);
        while(true) {
            if(npcManager.IsCanFindSeat() && menuStack.Count > 0)  {
                if(menuStack.TryPop(out Recipe recipe)) {
                    Debug.Log(recipe.menuName);
                    npcManager.SpwanNPCs(recipe);
                    yield return new WaitForSeconds(UnityEngine.Random.Range(minSpwanInterval, maxSpwanInterval));
                }
                else {
                    Debug.Log("Empty Menu");
                    yield return new WaitForSeconds(5f);
                }    
            }
            else yield return new WaitForSeconds(5f);
            if(!CheckSpawnNpc()) break;
        }
    }

    bool CheckSpawnNpc() {
        return true;
    }
        
    public Stack<Recipe> CreateRandomStack(Dictionary<Recipe, int> dailyMenuList)
    {
        List<Recipe> tempList = new List<Recipe>();

        // Recipe를 개수만큼 tempList에 추가
        foreach (var pair in dailyMenuList)
        {
            for (int i = 0; i < pair.Value; i++)
            {
                tempList.Add(pair.Key);
            }
        }

        System.Random rand = new System.Random();
        int n = tempList.Count;
        while (n > 1)
        {
            n--;
            int k = rand.Next(n + 1);
            (tempList[n], tempList[k]) = (tempList[k], tempList[n]);
        }

        // 섞인 리스트를 Stack에 Push
        Stack<Recipe> tempMenuStack = new Stack<Recipe>(tempList);
        return tempMenuStack;
    }

    public void ReturnMenu(Recipe getMenu) {
        menuStack.Push(getMenu);
    }
}
