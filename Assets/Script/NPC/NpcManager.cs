using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;
using System;

public class NpcManager : MonoBehaviour
{
    public static NpcManager instance;
    public static int totalNpcCount = 0;
    //-------------- SpawnNpc Setting --------------------//
    [Header("NPC Move Path")]
    [SerializeField] public Transform spawnPoint;
    [SerializeField] public Transform doorPoint;
    [SerializeField] public Transform centerPoint;
    [SerializeField] GameObject[] npcPrefab;

    [Header("Seat Point")]
    public Transform[] sitpositions = new Transform[4]; //���� ��ǥ
    public Transform[] sitpoint = new Transform[4]; //���� �� ��ǥ
    private bool[] sitOccupied = new bool[4]; //���� ���� ǥ��
    public Transform[] foodpoint = new Transform[4];//���� ���̺� ����Ʈ

    //OrderManger use this list
    [NonSerialized] public List<GameObject> npcList = new List<GameObject>();

    private void Awake()
    {
        instance = this;
    }


    public void SpwanNPCs(Recipe recipe)
    {
        int seatIndex = FindRandomSeat();
        if (seatIndex == -1) return; // ��� �¼��� �� ���� ��

        GameObject npc = Instantiate(npcPrefab[UnityEngine.Random.Range(0, npcPrefab.Length)], spawnPoint.position, Quaternion.identity);// ��ġ npc������ġ�� �ٲܰ�
        npcList.Add(npc);
        totalNpcCount++;
        StartCoroutine(ManageNPC(npc, recipe));
    }

    IEnumerator MoveNPC(GameObject npc, Transform targetPosition)
    {
        NavMeshAgent nav = npc.GetComponent<NavMeshAgent>();
        NPCBehavior npcBehavior = nav.GetComponent<NPCBehavior>();
        nav.enabled = true;
        nav.SetDestination(targetPosition.position);
        // NPC�� ��ǥ ��ġ�� ������ ������ ���
        while (nav.pathPending || nav.remainingDistance > nav.stoppingDistance)
        {
            if (npcBehavior.npcState == NPCBehavior.NPCState.Exiting)
            {
                nav.ResetPath();
                break;
            }
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
        else
        {
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
        if (npc.GetComponent<NPCBehavior>().npcState == NPCBehavior.NPCState.Exiting) yield break;

        StartCoroutine(npc.GetComponent<NPCBehavior>().OrderMenu(menu));
    }

    public bool IsCanFindSeat()
    {
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

    public void SeatEmpty(int seatIndex)
    {
        sitOccupied[seatIndex] = false;
    }

    int[] ShuffleArray(int[] array) //������ �ڸ� ����
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int randomIndex = UnityEngine.Random.Range(0, i + 1);
            int temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
        return array;
    }
}
