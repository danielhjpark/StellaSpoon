using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public class NpcManager : MonoBehaviour
{
    public static NpcManager instance;

    public int totalGold;
    TextMeshProUGUI totalGoldText;

    //--------------- Seat variable ---------------------//
    public Transform[] sitpositions = new Transform[4]; //���� ��ǥ
    public Transform[] sitpoint = new Transform[4]; //���� �� ��ǥ
    private bool[] sitOccupied = new bool[24]; //���� ���� ǥ��

    //-------------- SpawnNpc Setting --------------------//
    [SerializeField] Transform spawnPoint;
    [SerializeField] Transform doorPoint;
    [SerializeField] Transform centerPoint;
    [SerializeField] GameObject[] npcPrefab;

    private void Awake() {
        totalGoldText = GameObject.Find("GoldUI").GetComponent<TextMeshProUGUI>();
        instance = this;
    }
    
    private void Update() {
        totalGoldText.text = totalGold.ToString();
    }

    public void SpwanNPCs(Recipe recipe)
    {
        int seatIndex = FindRandomSeat();
        if (seatIndex == -1) // ��� �¼��� �� ���� ��
        {
           return;
        }
        GameObject npc = Instantiate(npcPrefab[Random.Range(0, npcPrefab.Length)], spawnPoint.position, Quaternion.identity);// ��ġ npc������ġ�� �ٲܰ�
        StartCoroutine(ManageNPC(npc, recipe));
    }
    
    IEnumerator MoveNPC(GameObject npc, Transform targetPosition) {
        NavMeshAgent nav = npc.GetComponent<NavMeshAgent>();
        nav.enabled = true;
        nav.SetDestination(targetPosition.position);
        // NPC�� ��ǥ ��ġ�� ������ ������ ���
        while (nav.pathPending || nav.remainingDistance > nav.stoppingDistance)
        {
            // NPC�� ��ǥ ������ �����ߴ��� Ȯ��
            if (Mathf.Abs(npc.transform.position.x - targetPosition.position.x) <= 0.3 &&
                Mathf.Abs(npc.transform.position.z - targetPosition.position.z) <= 0.3)
            {
                //npc.transform.position = targetPosition.position; // NPC�� ��ǥ ��ġ�� �����̵�
                //NpcRotation(npc, seatIndex); //npc å����⿡���� ȸ��
                break;
            }

            yield return null;
        }
 
    }

    IEnumerator ManageNPC(GameObject npc, Recipe menu)
    {
        int seatIndex = FindRandomSeat();
        if (seatIndex == -1) // ��� �¼��� �� ���� ��
        {
            Destroy(npc);
            yield break;
        }
        else {
            sitOccupied[seatIndex] = true;
        }
        // Set npc sitting
       // Transform sitPoint = sitpositions[seatIndex];
        Transform sitPoint = sitpoint[seatIndex];
        npc.GetComponent<NPCBehavior>().Initialize(menu, seatIndex);

        // Move enterance
        yield return StartCoroutine(MoveNPC(npc, doorPoint.transform));
        yield return StartCoroutine(MoveNPC(npc, centerPoint.transform));
        yield return StartCoroutine(MoveNPC(npc, sitPoint));
        //yield return StartCoroutine(MoveNPC(npc, sitPoint2));
        //NpcRotation(npc, seatIndex);
        StartCoroutine(npc.GetComponent<NPCBehavior>().OrderMenu(menu));
    }


    private void NpcRotation(GameObject npc, int seatIndex)
    {
        switch (seatIndex)
        {
            case 0:
            case 1:
            case 6:
            case 7:
            case 12:
            case 13:
            case 18:
            case 19:
                // ������ �ٶ󺸰� (Y�� �������� 90�� ȸ��)
                npc.transform.rotation = Quaternion.Euler(0, 90, 0);
                break;
            case 2:
            case 3:
            case 4:
            case 5:
            case 14:
            case 15:
            case 16:
            case 17:
                // ���� �ٶ󺸰� (Y�� �������� -90�� ȸ��)
                npc.transform.rotation = Quaternion.Euler(0, -90, 0);
                break;
            case 8:
            case 9:
                // ���� �ٶ󺸰� (Y�� �������� 180�� ȸ��)
                npc.transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case 10:
            case 11:
                // �Ʒ��� �ٶ󺸰� (Y�� �������� 0�� ȸ��)
                npc.transform.rotation = Quaternion.Euler(0, 180, 0);
                break;
            case 20:
            case 21:
                // ���� ���� �ٶ󺸰� (Y�� �������� -135�� ȸ��)
                npc.transform.rotation = Quaternion.Euler(0, -45, 0);
                break;
            case 22:
            case 23:
                // ������ �Ʒ��� �ٶ󺸰� (Y�� �������� 45�� ȸ��)
                npc.transform.rotation = Quaternion.Euler(0, 135, 0);
                break;
        }
    }
    

    private Vector3 GetTablePositionInFront(Vector3 npcPosition, Quaternion npcRotation)
    {
        // NPC�� ȸ������ ���� ���� ��ġ ���
        Vector3 forwardDirection = npcRotation * Vector3.forward; // NPC�� �ٶ󺸴� ����
        float distanceInFront = 2.0f; // NPC ���� �Ÿ� (���� ����)
                                      // NPC ���� ��ġ�� ���
        Vector3 tablePosition = npcPosition + forwardDirection * distanceInFront;

        // Y �� ���� (��: Y ���� -0.5�� ����)
        tablePosition.y = npcPosition.y - 0.55f; // ���ϴ� Y ������ ����

        return tablePosition;
    }


    public bool IsCanFindSeat() {
        for (int i = 0; i < sitOccupied.Length; i++)
        {
            if (!sitOccupied[i])
            {
                return true;
            }
        }
        return false;
    }

    int FindRandomSeat() //�¼� ��ġ
    {
        int[] shuffledIndices = ShuffleArray(Enumerable.Range(0, sitpositions.Length).ToArray());
        for (int i = 0; i < shuffledIndices.Length; i++)
        {
            if (!sitOccupied[shuffledIndices[i]])
            {
                return shuffledIndices[i];
            }
        }
        return -1;
    }

    public void SeatEmpty(int seatIndex) {
        sitOccupied[seatIndex] = false;
    }

    int[] ShuffleArray(int[] array) //������ �ڸ� ����
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            int temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
        return array;
    }
}
