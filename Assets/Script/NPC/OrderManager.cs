using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.Analytics;

public class OrderManager : MonoBehaviour
{
    public static OrderManager instance;
    [SerializeField] NpcManager npcManager;

    Stack<Recipe> menuStack = new Stack<Recipe>();
    List<Recipe> failMenu = new List<Recipe>();

    public Coroutine restaurantCoroutine;
    public Coroutine restaurantStopCoroutine;
    private float startInterval = 0f;
    private const float minSpwanInterval = 5f, maxSpwanInterval = 10f; //NPC 생성시간 최소

    public bool isMenuSoldOut;

    private void Awake()
    {
        instance = this;
    }

    public void MakeMenuList()
    {
        menuStack = CreateRandomMenu(DailyMenuManager.dailyMenuList);
    }

    public void OpenRestaurant()
    {
        if (restaurantCoroutine == null)
        {
            restaurantCoroutine = StartCoroutine(StartRestaurant());
        }
    }


    public void CloseRestaurant()
    {
        if (restaurantCoroutine != null)
        {
            StopCoroutine(restaurantCoroutine);
        }
        if (restaurantStopCoroutine == null)
            {
                restaurantStopCoroutine = StartCoroutine(StopRestaurant());
                DailyMenuManager.instance.DailyMenuReset();
            }
        }

    IEnumerator StartRestaurant()
    {
        MakeMenuList();
        yield return new WaitForSeconds(startInterval);
        WaitForSeconds npcDelayTime = new WaitForSeconds(5f);
        while (true)
        {
            if (!CheckSpawnNPC()) break;
            
            if (npcManager.IsCanFindSeat() && menuStack.Count > 0)
            {
                Recipe recipe = GetRecipe();
                if (recipe != null)
                {
                    Debug.Log(recipe.menuName);
                    npcManager.SpwanNPCs(recipe);
                    yield return new WaitForSeconds(UnityEngine.Random.Range(minSpwanInterval, maxSpwanInterval));
                }
                else
                {
                    Debug.Log("Empty Menu");
                    yield return npcDelayTime;
                }
            }
            else
            {
                yield return npcDelayTime;
            }
            
        }
        restaurantCoroutine = null;
    }

    IEnumerator StopRestaurant()
    {
        foreach (GameObject npc in npcManager.npcList)
        {
            NPCBehavior npcBehavior = npc.GetComponent<NPCBehavior>();
            npcBehavior.npcState = NPCBehavior.NPCState.Exiting;
            StartCoroutine(npcBehavior.ForeceExit());
        }

        while (true)
        {
            if (npcManager.npcList.Count > 0) break;
            yield return null;
        }
        restaurantCoroutine = null;
        restaurantStopCoroutine = null;
    }

    bool CheckSpawnNPC()
    {
        if(menuStack.Count <= 0 && NpcManager.instance.npcList.Count <= 0) {
            return false;
        }
        return true;
    }

    public void MakeMenu(Recipe recipe)
    {
        failMenu.Add(recipe);
    }
    public void FailMenu(Recipe recipe)
    {
        failMenu.Add(recipe);
        //DailyMenuManager.instance.DailyMenuRemove(recipe);

    }

    public Recipe GetRecipe()
    {
        while (menuStack.Count > 0)
        {
            if (menuStack.TryPop(out Recipe recipe))
            {
                Recipe makeMenuCheck = failMenu.FirstOrDefault(r => r == recipe);
                if (makeMenuCheck == null)
                {
                    return recipe;
                }
                else failMenu.Remove(recipe);
            }
        }
        return null;
    }

    public void RetuenMenu(Recipe recipe)
    {
        menuStack.Push(recipe);
        return;
        Recipe failMenuCheck = failMenu.FirstOrDefault(r => r == recipe);
        if (failMenuCheck != null)
        {
            failMenu.Remove(failMenuCheck);
        }
        else
        {
            
        }

    }

    public Stack<Recipe> CreateRandomMenu(Dictionary<Recipe, int> dailyMenuList)
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
