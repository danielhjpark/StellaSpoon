using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using Unity.Profiling;

public class NPCBehavior : MonoBehaviour
{
    //Npc want menu image
    [SerializeField] private GameObject menuImageObject;
    [SerializeField] private Image menuImage; // NPC 머리 위에 표시될 메뉴 스프라이트
    [SerializeField] private Sprite[] emotionSprites;// 0 - yes , 1 - no
    [SerializeField] private AudioClip npcSittingAudio;

    private Animator npcAnimator;
    private NavMeshAgent npcNav;
    private WaitForSeconds emotionWaitTime = new WaitForSeconds(2f);
    private GameObject serveObject;

    private bool hasOrdered = false; // 주문 완료 여부
    private bool hasReceivedMenu = false; // 메뉴 수령 여부
    private bool isCanReceivedMenu = true;

    private float orderTime; // 주문한 시간
    private int currentSeatIndex;
    private int payPrice;
    public enum NPCState { Idle, Entering, Sitting, Exiting }
    public NPCState npcState = NPCState.Idle;

    //NPC want menu
    public Recipe currentMenu;
    //Exit Point
    private Transform spawnPoint;
    private Transform doorPoint;
    private Transform centerPoint;

    private Coroutine eatCoroutine;

    private void Start()
    {
        npcAnimator = GetComponent<Animator>();
        npcNav = GetComponent<NavMeshAgent>();
        menuImage.enabled = false;

        spawnPoint = NpcManager.instance.spawnPoint;
        doorPoint = NpcManager.instance.doorPoint;
        centerPoint = NpcManager.instance.centerPoint;
    }

    private void LateUpdate()
    {
        Transform mainCam = Camera.main.transform;
        menuImageObject.transform.LookAt(menuImageObject.transform.position + mainCam.rotation * Vector3.forward,
            mainCam.rotation * Vector3.up);
    }

    public void Initialize(Recipe menu, int seatIndex)
    {
        currentMenu = menu;
        currentSeatIndex = seatIndex;
    }


    public IEnumerator OrderMenu(Recipe menu)
    {
        npcAnimator.SetBool("isSitting", true);
        npcState = NPCState.Sitting;

        AudioSource.PlayClipAtPoint(npcSittingAudio, this.transform.position);
        npcNav.enabled = false;
        this.transform.rotation = Quaternion.Euler(0, -180, 0);

        Vector3 seatPosition = NpcManager.instance.sitpositions[currentSeatIndex].position;
        Vector3 initPositon = this.transform.position;
        Vector3 targetPosition = new Vector3(seatPosition.x, this.transform.position.y, seatPosition.z);
        float t = 0;
        while (true)
        {
            t += 0.1f;
            this.transform.position = Vector3.Lerp(initPositon, targetPosition, t);
            if (t >= 1) { break; }
            yield return null;
        }

        yield return new WaitForSeconds(2f);// 기다린 후 주문

        if (npcState == NPCState.Exiting) yield break;
        else if (!hasOrdered)
        {
            Debug.Log("NPC OrderMenu.");
            hasOrdered = true;
            menuImage.enabled = true;
            menuImage.sprite = menu.menuImage;
            // 주문 시간 기록
            orderTime = Time.time;
            StartCoroutine(ReceiveMenu());
        }

    }

    IEnumerator ReceiveMenu()
    {
        float waitTime = 0f;
        float stayDuration = 50f;
        // float npcWaitTime = 40f;

        while (true)
        {
            if (npcState == NPCState.Exiting)
            {
                break;
            }
            else if (waitTime < stayDuration) //해당 시간동안 음식을 받는지 체크함.
            {
                waitTime += Time.deltaTime;
            }
            else if (!hasReceivedMenu) //음식을 받지 못하였을 때
            {
                isCanReceivedMenu = false; //메뉴 회수
                yield return Exit();
                yield break;
            }
            else
            {
                //yield return Exit(this.gameObject, npcSpawnPoint, npcNav, currentSeatIndex);
                break;
            }
            yield return null;
        }
    }

    public IEnumerator Exit()
    {
        // 퇴장 절차
        OrderManager.instance.RetuenMenu(currentMenu);
        npcState = NPCState.Exiting;
        npcAnimator.SetBool("isSitting", false);
        npcNav.enabled = true; // 이동 재개
        menuImage.enabled = false;

        yield return StartCoroutine(MoveNPC(centerPoint));
        yield return StartCoroutine(MoveNPC(doorPoint));
        yield return StartCoroutine(MoveNPC(spawnPoint));

        NpcManager.instance.SeatEmpty(currentSeatIndex);
        NpcManager.instance.npcList.Remove(this.gameObject);
        Destroy(this.gameObject); // NPC 제거
    }


    public IEnumerator ForeceExit()
    {
        if (eatCoroutine != null)
        {
            yield break;
        }
        npcState = NPCState.Exiting;
        npcAnimator.SetBool("isSitting", false);
        npcNav.enabled = true; // 이동 재개
        menuImage.enabled = false;

        Vector3 npcPos = this.transform.position;
        float centerDistance = Vector3.Distance(npcPos, centerPoint.position);
        float doorDistance = Vector3.Distance(npcPos, doorPoint.position);
        float spawnDistance = Vector3.Distance(npcPos, spawnPoint.position);
        if (centerDistance < doorDistance && centerDistance < spawnDistance)
        {
            yield return StartCoroutine(MoveNPC(centerPoint));
            yield return StartCoroutine(MoveNPC(doorPoint));
            yield return StartCoroutine(MoveNPC(spawnPoint));
        }
        else if (doorDistance < centerDistance && doorDistance < spawnDistance)
        {
            yield return StartCoroutine(MoveNPC(doorPoint));
            yield return StartCoroutine(MoveNPC(spawnPoint));
        }
        else
        {
            yield return StartCoroutine(MoveNPC(spawnPoint));
        }
        NpcManager.instance.DestroyNPC(currentSeatIndex, this.gameObject);
        NpcManager.instance.SeatEmpty(currentSeatIndex);
        NpcManager.instance.npcList.Remove(this.gameObject);
        Destroy(this.gameObject); // NPC 제거
    }

    IEnumerator MoveNPC(Transform targetPosition)
    {
        npcNav.enabled = true;
        npcNav.SetDestination(targetPosition.position);
        // NPC가 목표 위치에 도달할 때까지 대기
        while (npcNav.pathPending || npcNav.remainingDistance > npcNav.stoppingDistance)
        {
            // NPC가 목표 지점에 근접했는지 확인
            if (Mathf.Abs(this.transform.position.x - targetPosition.position.x) <= 0.3 &&
                Mathf.Abs(this.transform.position.z - targetPosition.position.z) <= 0.3)
            {
                //npc.transform.position = targetPosition.position; // NPC를 목표 위치로 순간이동
                //NpcRotation(npc, seatIndex); //npc 책상방향에따라 회전
                break;
            }

            yield return null;
        }

    }

    //------------------ Serve to player ---------------------//
    public bool ReceiveNPC(GameObject serveObject)
    {
        this.serveObject = serveObject;
        bool isSameMenu = currentMenu == serveObject.GetComponent<MenuData>().menu ? true : false;

        //Emotion
        if (!isSameMenu)
        {
            StartCoroutine(DifferentMenu());
            return false;
        }
        else
        {
            StartCoroutine(SameMenu());
            payPrice = serveObject.GetComponent<MenuData>().menu.menuPrice;
            //DailyMenuManager.instance.DailyMenuRemove(currentMenu);
            serveObject.transform.SetParent(NpcManager.instance.foodpoint[currentSeatIndex]);
            serveObject.transform.localPosition = Vector3.zero;
        }
        //Receive Menu Behavior
        if (!hasReceivedMenu)
        {
            npcState = NPCState.Idle;
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
            eatCoroutine = StartCoroutine(EatAndExit());
        }
        return true;
    }

    IEnumerator EatAndExit()
    {
        Debug.Log("NPC Eat Food");
        //float eatingTime = Random.Range(40f, 60f);
        float eatingTime = Random.Range(5f, 10f);
        yield return new WaitForSeconds(eatingTime);
        Destroy(serveObject);
        yield return StartCoroutine(PayGold());
        yield return Exit();
    }


    //-------------- Emotion Sprite Swap----------------------//

    IEnumerator SameMenu()
    {
        menuImage.sprite = emotionSprites[0];
        yield return emotionWaitTime;
        menuImage.enabled = false;
    }

    IEnumerator DifferentMenu()
    {
        menuImage.sprite = emotionSprites[1];
        yield return emotionWaitTime;
        menuImage.sprite = currentMenu.menuImage;
    }

    IEnumerator PayGold()
    {
        menuImage.enabled = true;
        menuImage.sprite = emotionSprites[2]; // Gold Sprite 추가필요
        Manager.gold += payPrice;
        yield return new WaitForSeconds(1f);
        menuImage.sprite = currentMenu.menuImage;
        menuImage.enabled = false;
    }


    public bool IsCanReceivedMenu() { return isCanReceivedMenu; }
    public NPCState GetState() { return npcState; }
    public Recipe GetRecipe() { return currentMenu; }
    public int GetSeatIndex() { return currentSeatIndex; }
}

