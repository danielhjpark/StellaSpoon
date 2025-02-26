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
    private NavMeshAgent nav;
    private Transform targetSeat; // 목표 좌석

    public float moveSpeed = 2f; // NPC 이동 속도
    public int baseFoodPrice = 100; // 음식의 기본 가격

    private bool isSeated = false; // 좌석 착석 여부
    private bool hasOrdered = false; // 주문 완료 여부
    private bool hasReceivedMenu = false; // 메뉴 수령 여부
    private float orderTime; // 주문한 시간

    private Recipe currentMenu;
    private Transform mainCam;
    private int currentSeatIndex;
    private Vector3 npcSpawnPoint = new Vector3(10f, 1f, 0f);

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

    IEnumerator MoveToSeat()
    {
        while (Vector3.Distance(transform.position, targetSeat.position) > 0.1f)
        {
            Vector3 direction = (targetSeat.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
            yield return null;
        }

        // 좌석을 사용 불가능 상태로 설정
        targetSeat.gameObject.SetActive(false);
        isSeated = true;
    }

    public bool CheckOrderMenu(Recipe menu) {
        return  currentMenu == menu ? true : false;
    }

    public IEnumerator DifferentMenu() {
        menuImage.sprite = emotionSprites[1];
        yield return new WaitForSeconds(2f);
        menuImage.sprite = currentMenu.menuImage;
    }

    public IEnumerator SameMenu() {
        menuImage.sprite = emotionSprites[0];
        yield return new WaitForSeconds(2f);
        menuImage.enabled = false;
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

        while(true) {
            if(waitTime < stayDuration) //해당 시간동안 음식을 받는지 체크함.
            {
                waitTime += Time.deltaTime;
            }
            else if (!hasReceivedMenu) //음식을 받지 못하였을 때
            {
                OrderManager.instance.ReturnMenu(currentMenu); //메뉴 회수
                yield return Exit(this.gameObject, npcSpawnPoint, nav, currentSeatIndex);
                yield break;
            }
            else {
                yield return Exit(this.gameObject, npcSpawnPoint, nav, currentSeatIndex);
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


    public void ReceiveNpc()
    {
        StartCoroutine(SameMenu());
        if (!hasReceivedMenu)
        {
            Debug.Log("NPC Recive Menu.");
            hasReceivedMenu = true;

            float timeToServe = Time.time - orderTime;
            int price = baseFoodPrice;

            // 음식 제공 시간에 따른 가격 결정
            if (timeToServe <= 20f)
            {
                price += Mathf.RoundToInt(baseFoodPrice * 0.3f);
            }
            else if (timeToServe > 40f)
            {
                Debug.Log("Too late.");
                return;
            }

            Debug.Log($"음식 가격: {price} 골드");

            // 음식 섭취 시작
            StartCoroutine(EatAndReturn());
        }
    }

    IEnumerator EatAndReturn()
    {
        Debug.Log("NPC Eat Food");
        float eatingTime = Random.Range(40f, 60f);
        yield return new WaitForSeconds(eatingTime);

    }


}

