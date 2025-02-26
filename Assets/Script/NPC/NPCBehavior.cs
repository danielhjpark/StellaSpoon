using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class NPCBehavior : MonoBehaviour
{
    [SerializeField] private GameObject menuObject;
    [SerializeField] private Image menuImage; // NPC �Ӹ� ���� ǥ�õ� �޴� ��������Ʈ
    [SerializeField] private Sprite[] emotionSprites;// 0 - yes , 1 - no
    private NavMeshAgent nav;
    private Transform targetSeat; // ��ǥ �¼�

    public float moveSpeed = 2f; // NPC �̵� �ӵ�
    public int baseFoodPrice = 100; // ������ �⺻ ����

    private bool isSeated = false; // �¼� ���� ����
    private bool hasOrdered = false; // �ֹ� �Ϸ� ����
    private bool hasReceivedMenu = false; // �޴� ���� ����
    private float orderTime; // �ֹ��� �ð�

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

        // �¼��� ��� �Ұ��� ���·� ����
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
            // �ֹ� �ð� ���
            orderTime = Time.time;
        }
        StartCoroutine(ReceiveMenu());
    }

    IEnumerator ReceiveMenu() {
        float waitTime = 0f;
        float stayDuration = 60f;

        while(true) {
            if(waitTime < stayDuration) //�ش� �ð����� ������ �޴��� üũ��.
            {
                waitTime += Time.deltaTime;
            }
            else if (!hasReceivedMenu) //������ ���� ���Ͽ��� ��
            {
                OrderManager.instance.ReturnMenu(currentMenu); //�޴� ȸ��
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
        // ���� ����
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


    public void ReceiveNpc()
    {
        StartCoroutine(SameMenu());
        if (!hasReceivedMenu)
        {
            Debug.Log("NPC Recive Menu.");
            hasReceivedMenu = true;

            float timeToServe = Time.time - orderTime;
            int price = baseFoodPrice;

            // ���� ���� �ð��� ���� ���� ����
            if (timeToServe <= 20f)
            {
                price += Mathf.RoundToInt(baseFoodPrice * 0.3f);
            }
            else if (timeToServe > 40f)
            {
                Debug.Log("Too late.");
                return;
            }

            Debug.Log($"���� ����: {price} ���");

            // ���� ���� ����
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

