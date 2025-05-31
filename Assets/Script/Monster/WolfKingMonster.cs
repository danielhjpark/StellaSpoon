using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WolfKingMonster : MonsterBase
{
    private int nextPattern = 0;

    private static readonly int THROW = 0; //던지기
    private static readonly int PILLAR = 1; //기둥세우기
    private static readonly int SPAWN = 2; //스폰

    private Coroutine currentPatternCoroutine = null;

    public static bool isThrowWarning = false; //던지기 경고 여부

    [Header("Throw")]
    [SerializeField]
    private int waitThrowTime; //던지기 기다리는 시간
    [SerializeField]
    private Transform[] throwPosition; //투척물 위치
    [SerializeField]
    private GameObject throwObjectPrefab; //투척물 오브젝트 프리팹
    private int throwcount = 0; //투척물 횟수
    [SerializeField]
    private GameObject warningPrefab; // 빨간 경고 프리팹

    [Header("Pillar")]
    [SerializeField]
    private float pillarRange = 5.0f; //기둥 세우기 범위
    [SerializeField]
    private GameObject pillarPrefab; //기둥 프리팹
    private int pillarCount = 0; //기둥 세우기 횟수

    [Header("Spawn")]
    [SerializeField]
    private GameObject silverSpawnPrefab; //실버울프 스폰 프리팹
    [SerializeField]
    private GameObject novaSpawnPrefab; //오바울프 스폰 프리팹
    [SerializeField]
    private float spawnRange = 5f; //스폰 범위
    private int silverSpawnCount = 2; //실버울프 스폰 횟수
    private int novaSpawnCount = 1; //노바울프 스폰 횟수

    [Header("Health UI")]
    [SerializeField]
    private GameObject bossHealthUI;
    [SerializeField]
    private Slider bossHealthSlider;

    private bool isPlayerInRange = false;

    protected override void Start()
    {
        base.Start();

        if (bossHealthUI != null)
        {
            bossHealthUI.SetActive(false);
        }

        if (bossHealthSlider != null)
        {
            bossHealthSlider.maxValue = maxHealth;
            bossHealthSlider.value = currentHealth;
        }
    }

    private void Update()
    {
        base.Update();

        if (distanceToPlayer <= playerDetectionRange && !isPlayerInRange)
        {
            isPlayerInRange = true;
            if (bossHealthUI != null)
            {
                bossHealthUI.SetActive(true);
            }
        }
        else if (distanceToPlayer > playerDetectionRange && isPlayerInRange)
        {
            isPlayerInRange = false;
            if (bossHealthUI != null)
            {
                bossHealthUI.SetActive(false);
            }
        }
    }

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
        if(isDead) yield break; //죽었으면 코루틴 종료
        animator.SetBool("Walk", false);
        if (!inAttackRange) yield break; //공격 범위 안에 플레이어가 없으면 공격하지 않음
        //animator.SetTrigger("Attack8");

        foreach (Transform pos in throwPosition)
        {
            //경고 효과 표시
            StartCoroutine(ShowThrowGroundEffect(pos));

        }
        throwcount++;
        if (throwcount < 2)
        {
            yield return new WaitForSeconds(6.0f); //3초 대기
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

    private IEnumerator ShowThrowGroundEffect(Transform pos)
    {
        if (isDead) yield break; //죽었으면 코루틴 종료
        // 준비 시 플레이어의 현재 위치 저장
        Vector3 targetPosition = player.transform.position;
        //플레이어와 중간지점 계산
        Vector3 middlePosition = transform.position + ((targetPosition - pos.position) / 2);
        yield return new WaitForSeconds(0.7f); // 0.5초 대기
        isThrowWarning = true;
        // 1) 경고 오브젝트 생성
        GameObject warning = Instantiate(warningPrefab, new Vector3(pos.position.x, transform.position.y + 0.01f, pos.position.z), pos.rotation);
        // 2) X축 회전값을 90도로 조정
        Vector3 warnRot = warning.transform.rotation.eulerAngles;
        warnRot.x = 90;
        warning.transform.rotation = Quaternion.Euler(warnRot);
        // 3) 길이 5, 폭 1로 스케일 조정
        warning.transform.localScale = new Vector3(1.5f, 10, 1);

        yield return new WaitForSeconds(waitThrowTime);

        ThrowObjectSpawn(pos); // 투척물 생성
        // 4) 3초 후 삭제
        Destroy(warning);

        StartCoroutine(StartAttack01());
    }

    private IEnumerator StartAttack01()
    {
        if (isDead) yield break; //죽었으면 코루틴 종료
        animator.SetTrigger("Attack1");
        // 애니메이션이 끝날 때까지 대기
        while (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
            yield return null;
    }

    private void ThrowObjectSpawn(Transform pos)
    {
        if (isDead) return; //죽었으면 코루틴 종료
        // 기존 투척물 생성
        GameObject throwObject = Instantiate(throwObjectPrefab, pos.position, pos.rotation);
        throwObject.GetComponent<LunaWolfKingBullet>().thirdPersonController = thirdPersonController;
        throwObject.GetComponent<LunaWolfKingBullet>().damage = damage;

        StartCoroutine(WaitThrowDelay(1f));
    }

    private IEnumerator WaitThrowDelay(float delayTime)
    {
        if (isDead) yield break; //죽었으면 코루틴 종료
        yield return new WaitForSeconds(delayTime);
        isThrowWarning = false;
    }

    private IEnumerator Pillar()
    {
        if (isDead) yield break; //죽었으면 코루틴 종료
        if (!inAttackRange) yield break; //공격 범위 안에 플레이어가 없으면 공격하지 않음
        pillarCount = Random.Range(4, 6); //기둥 세우기 횟수

        yield return new WaitForSeconds(2.0f); //2초 대기
        //애니메이션 재생

        for (int i = 0; i < pillarCount; i++)
        {
            Debug.Log(pillarCount);
            Vector3 randomPos = new Vector3(Random.Range(-pillarRange, pillarRange), 0, Random.Range(-pillarRange, pillarRange));
            GameObject pillar = Instantiate(pillarPrefab, transform.position + randomPos, Quaternion.identity);
            Destroy(pillar, 5.0f); //5초 후 기둥 삭제
        }
        yield return new WaitForSeconds(5.0f); //5초 대기
        nextPattern = SPAWN;
        nextPatternPlay();
    }

    private IEnumerator Spawn()
    {
        if (isDead) yield break; //죽었으면 코루틴 종료
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
        nextPattern = THROW;
        nextPatternPlay();
    }
    protected void nextPatternPlay()
    {
        if (currentPatternCoroutine != null)
            return; // 이미 실행 중이면 중복 실행 방지

        switch (nextPattern)
        {
            case 0:
                StartCoroutine(Throw());
                Debug.Log("던지기 패턴 실행");
                break;
            case 1:
                StartCoroutine(Pillar());
                Debug.Log("기둥 세우기 패턴 실행");
                break;
            case 2:
                StartCoroutine(Spawn());
                Debug.Log("스폰 패턴 실행");
                break;
        }
    }

    protected override void OnDrawGizmosSelected()//선택시 보이게 //항상 보이게 OnDrawGizmos
    {
        base.OnDrawGizmosSelected();

        Gizmos.color = Color.white; //기둥 범위
        Gizmos.DrawWireSphere(transform.position, pillarRange);
    }
}
