using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfKingMonster : MonsterBase
{
    private int nextPattern = 0;

    private static readonly int THROW = 0; //던지기
    private static readonly int PILLAR = 1; //기둥세우기
    private static readonly int SPAWN = 2; //스폰

    [Header("Throrw")]
    [SerializeField]
    private Transform[] throwPosition; //던지기 위치
    [SerializeField]
    private GameObject throwObjectPrefab; //던지기 오브젝트 프리팹
    private int throwcount = 0; //던지기 횟수

    [Header("Pillar")]
    [SerializeField]
    private float pillarRange = 5.0f; //기둥 세우기 범위
    [SerializeField]
    private GameObject pillarPrefab; //기둥 프리팹
    private int pillarCount = 0; //기둥 세우기 횟수
    [SerializeField]
    private Transform[] pillarPosition; //기둥 세우기 위치

    [Header("Spawn")]
    [SerializeField]
    private GameObject silverSpawnPrefab; //실버울프 스폰 프리팹
    [SerializeField]
    private GameObject novaSpawnPrefab; //오바울프 스폰 프리팹
    [SerializeField]
    private float spawnRange = 5f; //스폰 범위
    private int silverSpawnCount = 2; //실버울프 스폰 횟수
    private int novaSpawnCount = 1; //노바울프 스폰 횟수


    protected override void HandleAttack()
    {
        animator.SetBool("Walk", false);
        if (!isAttack)
        {
            isAttack = true;
            StartCoroutine(Throw());
        }
    }
    private IEnumerator Throw()
    {
        if (!inAttackRange) yield break; //공격 범위 안에 플레이어가 없으면 공격하지 않음
        //animator.SetTrigger("Attack8");
        foreach (Transform pos in throwPosition) //투척물 생성
        {
            GameObject throwObject = Instantiate(throwObjectPrefab, pos.position, Quaternion.identity);
        }
        throwcount++;
        if (throwcount < 2)
        {
            yield return new WaitForSeconds(3.0f); //3초 대기
            nextPattern = THROW;
            nextPatternPlay();
        }
        else
        {
            throwcount = 0;
            yield return new WaitForSeconds(4.0f); //4초 대기
            nextPattern = PILLAR;
            nextPatternPlay();
        }
    }

    private IEnumerator Pillar()
    {
        if (!inAttackRange) yield break; //공격 범위 안에 플레이어가 없으면 공격하지 않음
        pillarCount = Random.Range(4, 6); //기둥 세우기 횟수

        yield return new WaitForSeconds(2.0f); //2초 대기
        //애니메이션 재생

        for (int i = 0; i < pillarCount; i++)
        {
            Vector3 randomPos = new Vector3(Random.Range(-pillarRange, pillarRange), 0, Random.Range(-pillarRange, pillarRange));
            GameObject pillar = Instantiate(pillarPrefab, transform.position + randomPos, Quaternion.identity);
            Destroy(pillar, 5.0f); //5초 후 기둥 삭제
            yield return new WaitForSeconds(1.0f);
        }
        yield return new WaitForSeconds(5.0f); //5초 대기
        nextPattern = SPAWN;
        nextPatternPlay();
    }

    private IEnumerator Spawn()
    {
        if (!inAttackRange) yield break; //공격 범위 안에 플레이어가 없으면 공격하지 않음
        yield return new WaitForSeconds(2.0f); //2초 대기
        //애니메이션 재생
        for (int i = 0; i < silverSpawnCount; i++)
        {
            Vector3 randomPos = new Vector3(Random.Range(-spawnRange, spawnRange), 0, Random.Range(-spawnRange, spawnRange));
            GameObject silverWolf = Instantiate(silverSpawnPrefab, transform.position + randomPos, Quaternion.identity);
        }
        for (int i = 0; i < novaSpawnCount; i++)
        {
            Vector3 randomPos = new Vector3(Random.Range(-spawnRange, spawnRange), 0, Random.Range(-spawnRange, spawnRange));
            GameObject novaWolf = Instantiate(novaSpawnPrefab, transform.position + randomPos, Quaternion.identity);
        }
        yield return new WaitForSeconds(3.0f); //3초 대기
    }
    protected void nextPatternPlay()
    {
        switch (nextPattern)
        {
            case 0:
                StartCoroutine(Throw());
                break;
            case 1:
                StartCoroutine(Pillar());
                break;
            case 2:
                StartCoroutine(Spawn());
                break;
        }
    }
}
