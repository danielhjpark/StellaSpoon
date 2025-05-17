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
    //-------------- SpawnNpc Setting --------------------//
    [Header("NPC Move Path")]
    [SerializeField] public Transform spawnPoint;
    [SerializeField] public Transform doorPoint;
    [SerializeField] public Transform centerPoint;
    [SerializeField] GameObject[] npcPrefab;

    [Header("Seat Point")]
    public Transform[] sitpositions = new Transform[4]; //의자 좌표
    public Transform[] sitpoint = new Transform[4]; //의자 옆 좌표
    private bool[] sitOccupied = new bool[4]; //의자 상태 표시

    private TextMeshProUGUI totalGoldText;

    //OrderManger use this list
    public List<GameObject> npcList = new List<GameObject>();

    private void Awake() {
        totalGoldText = GameObject.Find("GoldUI").GetComponent<TextMeshProUGUI>();
        instance = this;
    }
    
    private void Update() {
        totalGoldText.text = Manager.gold.ToString();
    }

    public void SpwanNPCs(Recipe recipe)
    {
        int seatIndex = FindRandomSeat();
        if (seatIndex == -1) return; // 모든 좌석이 차 있을 때

        GameObject npc = Instantiate(npcPrefab[Random.Range(0, npcPrefab.Length)], spawnPoint.position, Quaternion.identity);// 위치 npc생성위치로 바꿀것
        npcList.Add(npc);
        StartCoroutine(ManageNPC(npc, recipe));
    }
    
    IEnumerator MoveNPC(GameObject npc, Transform targetPosition) {
        NavMeshAgent nav = npc.GetComponent<NavMeshAgent>();
        nav.enabled = true;
        nav.SetDestination(targetPosition.position);
        // NPC가 목표 위치에 도달할 때까지 대기
        while (nav.pathPending || nav.remainingDistance > nav.stoppingDistance)
        {
            // NPC가 목표 지점에 근접했는지 확인
            if (Mathf.Abs(npc.transform.position.x - targetPosition.position.x) <= 0.3 &&
                Mathf.Abs(npc.transform.position.z - targetPosition.position.z) <= 0.3)
            {
                //npc.transform.position = targetPosition.position; // NPC를 목표 위치로 순간이동
                //NpcRotation(npc, seatIndex); //npc 책상방향에따라 회전
                break;
            }

            yield return null;
        }
 
    }

    IEnumerator ManageNPC(GameObject npc, Recipe menu)
    {
        int seatIndex = FindRandomSeat();
        if (seatIndex == -1) // 모든 좌석이 차 있을 때
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
        StartCoroutine(npc.GetComponent<NPCBehavior>().OrderMenu(menu));
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

    int FindRandomSeat() //좌석 서치
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

    int[] ShuffleArray(int[] array) //랜덤한 자리 선정
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
