using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

public class OrderManager : MonoBehaviour
{
    public static OrderManager instance;
    [SerializeField] NpcManager npcManager;

    Stack<Recipe> menuStack = new Stack<Recipe>();
    private Coroutine restaurantCoroutine;
    private float startInterval = 0f;
    private float minSpwanInterval = 5f, maxSpwanInterval = 10f; //NPC 생성시간 최소

    public bool isMenuSoldOut;
    public bool isStoppedOrder;

    private void Awake()
    {
        instance = this;
    }

    public void UpdateMenu()
    {
        menuStack = CreateRandomStack(DailyMenuManager.dailyMenuList);
    }

    public void OpenRestaurant()
    {
        isStoppedOrder = false;

        if (restaurantCoroutine == null)
        {
            restaurantCoroutine = StartCoroutine(StartRestaurant());
        }

    }

    public void CloseRestaurant()
    {
        isStoppedOrder = true;
        StopCoroutine(restaurantCoroutine);
        //NPC Return
    }

    IEnumerator StartRestaurant()
    {
        UpdateMenu();
        yield return new WaitForSeconds(startInterval);
        WaitForSeconds npcDelayTime = new WaitForSeconds(5f);
        while (true)
        {
            if (isStoppedOrder) break;
            if (!CheckSpawnNpc()) break;
            
            if (npcManager.IsCanFindSeat() && menuStack.Count > 0)
            {
                if (menuStack.TryPop(out Recipe recipe))
                {
                    Debug.Log(recipe.menuName);
                    npcManager.SpwanNPCs(recipe);
                    yield return new WaitForSeconds(UnityEngine.Random.Range(minSpwanInterval, maxSpwanInterval));
                }
                else
                {
                    Debug.Log("Empty Menu");
                    UpdateMenu();
                    yield return npcDelayTime;
                }
            }
            else
            {
                UpdateMenu();
                yield return npcDelayTime;
            }
            
        }
        restaurantCoroutine = null;
        isStoppedOrder = false;
    }

    bool CheckSpawnNpc()
    {
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

}
