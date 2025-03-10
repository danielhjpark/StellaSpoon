using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class NPCBehavior : MonoBehaviour
{
    [SerializeField] private GameObject menuObject;
    [SerializeField] private Image menuImage; // NPC 머리 위에 표시될 메뉴 스프라이트
    [SerializeField] private Sprite[] emotionSprites;// 0 - yes , 1 - no
    private WaitForSeconds emotionWaitTime = new WaitForSeconds(2f);
    private NavMeshAgent nav;
    private Transform mainCam;

    private bool hasOrdered = false; // 주문 완료 여부
    private bool hasReceivedMenu = false; // 메뉴 수령 여부
    private float orderTime; // 주문한 시간

    private Recipe currentMenu;
    private int currentSeatIndex;
    private Vector3 npcSpawnPoint = new Vector3(10f, 1f, 0f);
    private int payPrice;

    private void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        mainCam = Camera.main.transform;
        menuImage.enabled = false;
    }

    private void LateUpdate()
    {
        menuObject.transform.LookAt(menuObject.transform.position + mainCam.rotation * Vector3.forward,
            mainCam.rotation * Vector3.up);
    }
    
    public void Initialize(Recipe menu, int seatIndex) {
        currentMenu = menu;
        currentSeatIndex = seatIndex;
    }

    public bool CheckOrderMenu(Recipe menu) {
        return  currentMenu == menu ? true : false;
    }

    public void OrderMenu(Recipe menu)
    {
        if (!hasOrdered)
        {
            Debug.Log("NPC OrderMenu.");
            hasOrdered = true;
            menuImage.enabled = true;
            menuImage.sprite = menu.menuImage;
            // 주문 시간 기록
            orderTime = Time.time;
        }
        StartCoroutine(ReceiveMenu());
    }

    IEnumerator ReceiveMenu() {
        float waitTime = 0f;
        float stayDuration = 60f;
        // float orderDelay = 10f;
        // float npcWaitTime = 40f;

        while(true) {
            if(waitTime < stayDuration) //해당 시간동안 음식을 받는지 체크함.
            {
                waitTime += Time.deltaTime;
            }
            else if (!hasReceivedMenu) //음식을 받지 못하였을 때
            {
                OrderManager.instance.UpdateMenu(); //메뉴 회수
                yield return Exit(this.gameObject, npcSpawnPoint, nav, currentSeatIndex);
                yield break;
            }
            else {
                //yield return Exit(this.gameObject, npcSpawnPoint, nav, currentSeatIndex);
                break;
            }
            yield return null;
        }
    }
    
    private IEnumerator Exit(GameObject npc, Vector3 npcSpawnPoint, NavMeshAgent nav, int seatIndex)
    {
        // 퇴장 절차
        nav.enabled = true; // 이동 재개
        menuImage.enabled = false;
        nav.SetDestination(npcSpawnPoint);
        Debug.Log("Exit");
        while (nav.pathPending || nav.remainingDistance > 1)
        {
            yield return null;
        }
        NpcManager.instance.SeatEmpty(seatIndex);
        Destroy(npc); // NPC 제거
    }

    //------------------ Serve to player ---------------------//
    public void ReceiveNPC(GameObject serveObject)
    {
        bool isSameMenu = CheckOrderMenu(serveObject.GetComponent<MenuData>().menu);

        if(!isSameMenu) {
            StartCoroutine(DifferentMenu());
            return;
        }
        else {
            StartCoroutine(SameMenu());
            payPrice = serveObject.GetComponent<MenuData>().menu.menuPrice;
            Destroy(serveObject);
        }

        if (!hasReceivedMenu) {
            Debug.Log("NPC Recive Menu.");
            hasReceivedMenu = true;

            float timeToServe = Time.time - orderTime;

            // Fast Serve Menu Add price
            if (timeToServe <= 20f)
            {
                payPrice += Mathf.RoundToInt(payPrice * 0.3f);
            }

            Debug.Log($"음식 가격: {payPrice} 골드");
            // 음식 섭취 시작
            StartCoroutine(EatAndExit());
        }
    }

    IEnumerator EatAndExit()
    {
        Debug.Log("NPC Eat Food");
        float eatingTime = Random.Range(40f, 60f);
        yield return new WaitForSeconds(eatingTime);
        NpcManager.instance.totalGold += payPrice;
        yield return Exit(this.gameObject, npcSpawnPoint, nav, currentSeatIndex);
    }


    //-------------- Emotion Sprite Swap----------------------//

    IEnumerator SameMenu() {
        menuImage.sprite = emotionSprites[0];
        yield return emotionWaitTime;
        menuImage.enabled = false;
    }

    IEnumerator DifferentMenu() {
        menuImage.sprite = emotionSprites[1];
        yield return emotionWaitTime;
        menuImage.sprite = currentMenu.menuImage;
    }

    IEnumerator PayGold() {
        menuImage.sprite = emotionSprites[2]; // Gold Sprite 추가필요
        yield return emotionWaitTime;
        menuImage.sprite = currentMenu.menuImage;
    }




}

