using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class NPCBehavior : MonoBehaviour
{
    private Animator npcAnimator;
    private NavMeshAgent npcNav;
    private WaitForSeconds emotionWaitTime = new WaitForSeconds(2f);

    [SerializeField] private GameObject menuObject;
    [SerializeField] private Image menuImage; // NPC �Ӹ� ���� ǥ�õ� �޴� ��������Ʈ
    [SerializeField] private Sprite[] emotionSprites;// 0 - yes , 1 - no

    
    private bool hasOrdered = false; // �ֹ� �Ϸ� ����
    private bool hasReceivedMenu = false; // �޴� ���� ����
    private float orderTime; // �ֹ��� �ð�
    private int currentSeatIndex;
    private int payPrice;

    //NPC want menu
    public Recipe currentMenu;
    //private Vector3 npcSpawnPoint = new Vector3(10f, 1f, 0f);
    
    //Exit Point
    private Transform spawnPoint;
    private Transform doorPoint;
    private Transform centerPoint;

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
        menuObject.transform.LookAt(menuObject.transform.position + mainCam.rotation * Vector3.forward,
            mainCam.rotation * Vector3.up);
    }
    
    public void Initialize(Recipe menu, int seatIndex) {
        currentMenu = menu;
        currentSeatIndex = seatIndex;
    }


    public IEnumerator OrderMenu(Recipe menu)
    {
        npcAnimator.SetBool("isSitting", true);
        npcNav.enabled = false;
        this.transform.rotation = Quaternion.Euler(0, -180, 0);

        Vector3 seatPosition = NpcManager.instance.sitpositions[currentSeatIndex].position;
        Vector3 initPositon = this.transform.position;
        Vector3 targetPosition = new Vector3(seatPosition.x, this.transform.position.y, seatPosition.z);
        float t = 0;
        while(true) {
            t += 0.1f;
            this.transform.position = Vector3.Lerp(initPositon, targetPosition, t);
            if(t >= 1) {break;}
            yield return null;
        }
        
        yield return new WaitForSeconds(2f);// ��ٸ� �� �ֹ�

        if (!hasOrdered)
        {
            Debug.Log("NPC OrderMenu.");
            hasOrdered = true;
            menuImage.enabled = true;
            menuImage.sprite = menu.menuImage;
            // �ֹ� �ð� ���
            orderTime = Time.time;
        }
        StartCoroutine(ReceiveMenu());
    }

    IEnumerator ReceiveMenu() {
        float waitTime = 0f;
        float stayDuration = 30f;
        // float orderDelay = 10f;
        // float npcWaitTime = 40f;

        while(true) {
            if(waitTime < stayDuration) //�ش� �ð����� ������ �޴��� üũ��.
            {
                waitTime += 1f;
            }
            else if (!hasReceivedMenu) //������ ���� ���Ͽ��� ��
            {
                OrderManager.instance.RetuenMenu(currentMenu); //�޴� ȸ��
                yield return Exit();
                yield break;
            }
            else {
                //yield return Exit(this.gameObject, npcSpawnPoint, npcNav, currentSeatIndex);
                break;
            }
            yield return new WaitForSeconds(1f);
        }
    }
    
    private IEnumerator Exit()
    {
        // ���� ����
        npcAnimator.SetBool("isSitting", false);
        npcNav.enabled = true; // �̵� �簳
        menuImage.enabled = false;

        yield return StartCoroutine(MoveNPC(centerPoint));
        yield return StartCoroutine(MoveNPC(doorPoint));
        yield return StartCoroutine(MoveNPC(spawnPoint));

        NpcManager.instance.SeatEmpty(currentSeatIndex);
        NpcManager.instance.npcList.Remove(this.gameObject);
        Destroy(this.gameObject); // NPC ����
    }

    IEnumerator MoveNPC(Transform targetPosition) {
      
        npcNav.enabled = true;
        npcNav.SetDestination(targetPosition.position);
        // NPC�� ��ǥ ��ġ�� ������ ������ ���
        while (npcNav.pathPending || npcNav.remainingDistance > npcNav.stoppingDistance)
        {
            // NPC�� ��ǥ ������ �����ߴ��� Ȯ��
            if (Mathf.Abs(this.transform.position.x - targetPosition.position.x) <= 0.3 &&
                Mathf.Abs(this.transform.position.z - targetPosition.position.z) <= 0.3)
            {
                //npc.transform.position = targetPosition.position; // NPC�� ��ǥ ��ġ�� �����̵�
                //NpcRotation(npc, seatIndex); //npc å����⿡���� ȸ��
                break;
            }

            yield return null;
        }
 
    }

    //------------------ Serve to player ---------------------//
    public void ReceiveNPC(GameObject serveObject)
    {
        bool isSameMenu = currentMenu == serveObject.GetComponent<MenuData>().menu ? true : false;

        //Emotion
        if(!isSameMenu) {
            StartCoroutine(DifferentMenu());
            return;
        }
        else {
            StartCoroutine(SameMenu());
            payPrice = serveObject.GetComponent<MenuData>().menu.menuPrice;
            DailyMenuManager.instance.DailyMenuRemove(currentMenu);
            Destroy(serveObject);
        }
        //Receive Menu Behavior
        if (!hasReceivedMenu) {
            Debug.Log("NPC Recive Menu.");
            hasReceivedMenu = true;

            float timeToServe = Time.time - orderTime;

            // Fast Serve Menu Add price
            if (timeToServe <= 20f)
            {
                payPrice += Mathf.RoundToInt(payPrice * 0.3f);
            }

            Debug.Log($"���� ����: {payPrice} ���");
            // ���� ���� ����
            StartCoroutine(EatAndExit());
        }
    }
    
    IEnumerator EatAndExit()
    {
        Debug.Log("NPC Eat Food");
        //float eatingTime = Random.Range(40f, 60f);
        float eatingTime = Random.Range(5f, 10f);
        yield return new WaitForSeconds(eatingTime);
        //NpcManager.instance.totalGold += payPrice;
        Manager.gold += payPrice;
        yield return Exit();
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
        menuImage.sprite = emotionSprites[2]; // Gold Sprite �߰��ʿ�
        yield return emotionWaitTime;
        menuImage.sprite = currentMenu.menuImage;
    }




}

