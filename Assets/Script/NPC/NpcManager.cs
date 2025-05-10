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
    public Transform[] sitpositions = new Transform[4]; //의자 좌표
    public Transform[] sitpoint = new Transform[4]; //의자 옆 좌표
    private bool[] sitOccupied = new bool[24]; //의자 상태 표시

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
        if (seatIndex == -1) // 모든 좌석이 차 있을 때
        {
           return;
        }
        GameObject npc = Instantiate(npcPrefab[Random.Range(0, npcPrefab.Length)], spawnPoint.position, Quaternion.identity);// 위치 npc생성위치로 바꿀것
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
                // 오른쪽 바라보게 (Y축 기준으로 90도 회전)
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
                // 왼쪽 바라보게 (Y축 기준으로 -90도 회전)
                npc.transform.rotation = Quaternion.Euler(0, -90, 0);
                break;
            case 8:
            case 9:
                // 위쪽 바라보게 (Y축 기준으로 180도 회전)
                npc.transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case 10:
            case 11:
                // 아래쪽 바라보게 (Y축 기준으로 0도 회전)
                npc.transform.rotation = Quaternion.Euler(0, 180, 0);
                break;
            case 20:
            case 21:
                // 왼쪽 위를 바라보게 (Y축 기준으로 -135도 회전)
                npc.transform.rotation = Quaternion.Euler(0, -45, 0);
                break;
            case 22:
            case 23:
                // 오른쪽 아래를 바라보게 (Y축 기준으로 45도 회전)
                npc.transform.rotation = Quaternion.Euler(0, 135, 0);
                break;
        }
    }
    

    private Vector3 GetTablePositionInFront(Vector3 npcPosition, Quaternion npcRotation)
    {
        // NPC의 회전값에 따른 앞쪽 위치 계산
        Vector3 forwardDirection = npcRotation * Vector3.forward; // NPC가 바라보는 방향
        float distanceInFront = 2.0f; // NPC 앞의 거리 (조절 가능)
                                      // NPC 앞쪽 위치를 계산
        Vector3 tablePosition = npcPosition + forwardDirection * distanceInFront;

        // Y 값 조정 (예: Y 값을 -0.5로 설정)
        tablePosition.y = npcPosition.y - 0.55f; // 원하는 Y 값으로 조정

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
