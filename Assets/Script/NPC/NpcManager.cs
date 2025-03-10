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
    [SerializeField] TextMeshProUGUI totalGoldText;

    //----------------Seat variable ---------------------//
    public Transform[] sitpositions = new Transform[4]; //���� ��ǥ
    private bool[] sitOccupied = new bool[24]; //���� ���� ǥ��

    //---------------SpawnNpc Setting----------------------//
    public GameObject npcPrefab;
    private Vector3 npcSpawnpoint = new Vector3(10f, 1f, 0f);


    private void Awake() {
        instance = this;
    }
    
    private void Update() {
        totalGoldText.text = totalGold.ToString();
    }

    public void SpwanNPCs(Recipe recipe)
    {
        GameObject npc = Instantiate(npcPrefab, npcSpawnpoint, Quaternion.identity);// ��ġ npc������ġ�� �ٲܰ�
        StartCoroutine(ManageNPC(npc, recipe));
    }
    
    IEnumerator ManageNPC(GameObject npc, Recipe menu)
    {
        NavMeshAgent nav = npc.GetComponent<NavMeshAgent>();
        Vector3 nowPosition = npc.transform.position;
        // �¼��� ã��
        int seatIndex = FindRandomSeat();
        if (seatIndex == -1) // ��� �¼��� �� ���� ��
        {
            Destroy(npc);
            yield break;
        }

        Transform targetPosition = sitpositions[seatIndex];
        sitOccupied[seatIndex] = true; // �¼��� ���� ���·� ����
        npc.GetComponent<NPCBehavior>().Initialize(menu, seatIndex);
        // ��ǥ ��ġ�� �̵�
        nav.SetDestination(targetPosition.position);

        // NPC�� ��ǥ ��ġ�� ������ ������ ���
        while (nav.pathPending || nav.remainingDistance > nav.stoppingDistance)
        {
            // NPC�� ��ǥ ������ �����ߴ��� Ȯ��
            if (Mathf.Abs(npc.transform.position.x - targetPosition.position.x) <= 2 &&
                Mathf.Abs(npc.transform.position.y - targetPosition.position.y) <= 2)
            {
                nowPosition = npc.transform.position;
                nav.enabled = false;
                npc.transform.position = targetPosition.position; // NPC�� ��ǥ ��ġ�� �����̵�
                NpcRotation(npc, seatIndex); //npc å����⿡���� ȸ��
                break;
            }

            yield return null;
        }

        yield return new WaitForSeconds(2f);// ��ٸ� �� �ֹ�

        npc.GetComponent<NPCBehavior>().OrderMenu(menu);

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
