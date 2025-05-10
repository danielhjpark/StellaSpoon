using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class NPCBehavior : MonoBehaviour
{
    private Animator npcAnimator;
    private NavMeshAgent npcNav;

    [SerializeField] private GameObject menuObject;
    [SerializeField] private Image menuImage; // NPC �Ӹ� ���� ǥ�õ� �޴� ��������Ʈ
    [SerializeField] private Sprite[] emotionSprites;// 0 - yes , 1 - no

    private WaitForSeconds emotionWaitTime = new WaitForSeconds(2f);

    private bool hasOrdered = false; // �ֹ� �Ϸ� ����
    private bool hasReceivedMenu = false; // �޴� ���� ����
    private float orderTime; // �ֹ��� �ð�

    private Recipe currentMenu;
    private Vector3 npcSpawnPoint = new Vector3(10f, 1f, 0f);
    private int currentSeatIndex;
    private int payPrice;

    private void Start()
    {
        npcAnimator = GetComponent<Animator>();
        npcNav = GetComponent<NavMeshAgent>();
        menuImage.enabled = false;
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

    public bool CheckOrderMenu(Recipe menu) {
        return  currentMenu == menu ? true : false;
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
        float stayDuration = 120f;
        // float orderDelay = 10f;
        // float npcWaitTime = 40f;

        while(true) {
            if(waitTime < stayDuration) //�ش� �ð����� ������ �޴��� üũ��.
            {
                waitTime += 1f;
            }
            else if (!hasReceivedMenu) //������ ���� ���Ͽ��� ��
            {
                //OrderManager.instance.UpdateMenu(); //�޴� ȸ��
                yield return Exit(this.gameObject, npcSpawnPoint, npcNav, currentSeatIndex);
                yield break;
            }
            else {
                //yield return Exit(this.gameObject, npcSpawnPoint, nav, currentSeatIndex);
                break;
            }
            yield return new WaitForSeconds(1f);
        }
    }
    
    private IEnumerator Exit(GameObject npc, Vector3 npcSpawnPoint, NavMeshAgent nav, int seatIndex)
    {
        // ���� ����
        npcAnimator.SetBool("isSitting", false);
        nav.enabled = true; // �̵� �簳
        menuImage.enabled = false;
        nav.SetDestination(npcSpawnPoint);
        Debug.Log("Exit");
        while (nav.pathPending || nav.remainingDistance > 1)
        {
            yield return null;
        }
        NpcManager.instance.SeatEmpty(seatIndex);
        Destroy(npc); // NPC ����
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
        NpcManager.instance.totalGold += payPrice;
        yield return Exit(this.gameObject, npcSpawnPoint, npcNav, currentSeatIndex);
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

