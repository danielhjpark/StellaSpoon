using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum MonsterStates //몬스터 상태
{
    Idle, //일반 상태
    Attack, //공격 상태
    Chasing, //쫒기 상태
    RandomMove, //랜덤 이동 상태
    Death //죽음 상태
}
public abstract class MonsterBase1 : MonoBehaviour
{
    [Header("Basic information")]
    public int maxHealth; //최대 체력
    public int currentHealth; //현재 체력
    public int damage; //공격력
    protected Vector3 initialPosition; //최초 위치
    protected bool isDead; //죽음 체크 변수
    protected bool isMove; //움직임 체크 변수
    protected float wanderTimer;
    protected float idleMoveInterval; //랜덤 이동 대기시간
    protected float damageDelayTime; //공격 딜레이 시간
    protected float lastAttackTime; //마지막 공격 시간
    protected bool isAttack = false; //공격중 체크 변수

    [Header("Range")]
    public float attackRange; //공격 범위
    public float playerDetectionRange; //인지 범위
    public float randomMoveRange; //랜덤 이동 범위
    public float damageRange; //플레이어 공격 인지 범위

    [Header("Drop item")]
    public GameObject[] dropItems; //드랍 아이템 리스트
    public float[] dropProbability; //아이템 별 드랍 확률
    public int[] maxDropItems; //아이템 별 최대 드랍 갯수

    protected MonsterStates currentState;
    protected NavMeshAgent nav;
    protected Animator animator;
    protected GameObject player;
    protected Collider coll;
    protected ThirdPersonController thirdPersonController;

    [SerializeField]
    private bool canDamage; //플레이어 공격 인지 범위 내에 있는지 체크 변수
    private bool isDamage; //데미지를 입었는지 체크 변수
    private bool RandomPositionDecide = false; //랜덤 경로가 정해졌는지 체크 변수

    [SerializeField]
    private float distanceToPlayer; //플레이어와의 거리

    protected void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        coll = GetComponent<Collider>();
        thirdPersonController = player.GetComponent<ThirdPersonController>();

        initialPosition = transform.position; //최초 위치 선언
        currentHealth = maxHealth;

        currentState = MonsterStates.Idle; //시작시 Idle상태

        lastAttackTime = -damageDelayTime; //공격 딜레이 초기화
    }

    protected void Update()
    {
        if (!isDead)
        {
            if (currentHealth <= 0)
            {
                currentState = MonsterStates.Death;
            }
            else
            {
                distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
                if (!isAttack) //공격중이 아닐때
                {

                    //데미지 인지 범위와 인지범위 사이일 때만 
                    if (distanceToPlayer <= damageRange && distanceToPlayer > playerDetectionRange)
                    {
                        canDamage = true;
                    }
                    else
                    {
                        canDamage = false;
                    }

                    if (distanceToPlayer <= attackRange)
                    {
                        currentState = MonsterStates.Attack;
                    }
                    else if (distanceToPlayer > attackRange && distanceToPlayer <= playerDetectionRange)
                    {
                        currentState = MonsterStates.Chasing;
                    }
                    HandleState();
                }
                LookPlayer();
            }
        }
    }
    protected void HandleState()
    {
        switch (currentState)
        {
            case MonsterStates.Idle:
                HandleIdle();
                Debug.Log("일반 상태");
                break;

            case MonsterStates.Attack:
                Debug.Log("공격 상태");
                HandleAttack();
                break;

            case MonsterStates.Chasing:
                Debug.Log("쫒기 상태");
                HandleChasing();
                break;

            case MonsterStates.RandomMove:
                Debug.Log("랜덤 이동 상태");
                HandleRandomMove();
                break;
            case MonsterStates.Death:
                Debug.Log("죽음 상태");
                HandleDeath();
                break;

        }
    }

    protected virtual void HandleIdle()
    {
        animator.SetBool("Walk", false);
        if (CanSeePlayer())
        {
            currentState = MonsterStates.Chasing;
        }
        //아무 범위에도 들어오지 않았다면 랜덤이동상태 전환
        else
        {
            currentState = MonsterStates.RandomMove;
        }
    }
    protected virtual void HandleAttack()
    {
        if (!isAttack || Time.time - lastAttackTime >= damageDelayTime)
        {
            lastAttackTime = Time.time;
            isAttack = true;
            animator.SetBool("Walk", false);
            animator.SetBool("Attack", true); //Attack 애니메이션 실행
            thirdPersonController.TakeDamage(damage, transform.position); //플레이어 데미지
            //플레이어에게 데미지 주기
        }

    }
    protected virtual void HandleChasing()
    {
        animator.SetBool("Walk", true);
        nav.SetDestination((player.transform.position) - (player.transform.position - transform.position).normalized * 0.8f);
        if (canDamage && isDamage)
        {
            //5초 뒤에 idle 상태로 변환
            StartCoroutine(DamageChasingDelay());
        }
        else
        {
            if (distanceToPlayer > playerDetectionRange)
            {
                currentState = MonsterStates.Idle;
            }
        }
        //플레이어의 위치 받아와서 경로 생성
        //Walk 애니메이션 실행
    }
    protected virtual void HandleRandomMove()
    {
        wanderTimer += Time.deltaTime; //시간 체크
        if (wanderTimer >= idleMoveInterval)
        {
            if (!RandomPositionDecide) // 랜덤 경로가 설정되지 않았을 때
            {
                Debug.Log("랜덤 위치 생성");
                Vector3 newDestination = GetRandomPoint(initialPosition, randomMoveRange);
                nav.SetDestination(newDestination);
                RandomPositionDecide = true;
                animator.SetBool("Walk", true);
                wanderTimer = 0; // 타이머 초기화
            }
            else
            {
                // 생성한 위치로 이동
                if (!nav.pathPending && nav.remainingDistance <= 0.5f) //도착지에서 0.3 내에 있는지
                {
                    animator.SetBool("Walk", false);
                    currentState = MonsterStates.Idle;
                    RandomPositionDecide = false; // 랜덤 경로 설정 해제
                }
            }
            //랜덤 위치 생성
        }
        //randomMoveRange 범위 내에서 랜덤한 위치 생성 //함수가 실행될 때 한번만 생성
        //랜덤한 위치로 이동

    }
    protected virtual void HandleDeath()
    {
        animator.SetBool("Walk", false);

        Debug.Log($"{gameObject.name}이(가) 사망했습니다.");
        currentHealth = 0;
        //애니메이션 죽음 실행
        animator.SetTrigger("Die");
        //충돌 제거
        coll.enabled = false;
        //네비매쉬끄기
        nav.ResetPath();
        //5초뒤 오브젝트 삭제
        Destroy(gameObject, 5f);
        //아이템 드랍
        DropItems();
    }

    //
    private Vector3 GetRandomPoint(Vector3 center, float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += center;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            return hit.position;
        }
        return center;
    }

    public void Damage(int damage)
    {
        currentHealth -= damage;
        Debug.Log(damage + " 데미지 입음! " + currentHealth + " 체력 남음");
        if (currentHealth <= 0)
        {
            isDead = true;
            currentState = MonsterStates.Death;
            HandleState();
        }
        if (canDamage)
        {
            isDamage = true;
            //플레이어 쫒기
            currentState = MonsterStates.Chasing;
        }
    }

    protected bool IsHpZero() //체력이 0 이하인지 검사
    {
        if (currentHealth <= 0)
        {
            return true;
        }
        return false;
    }

    protected bool CanAttack()
    {
        if (distanceToPlayer <= attackRange)
        {
            Debug.Log("공격범위 들어옴");
            return true;
        }
        return false;
    }
    protected bool CanSeePlayer()
    {
        if (distanceToPlayer <= playerDetectionRange)
        {
            Debug.Log("인지 범위 들어옴");
            return true;
        }
        return false;
    }
    protected bool CanDamage()
    {
        if (distanceToPlayer <= damageRange)
        {
            Debug.Log("데미지 범위 들어옴");
            return true;
        }
        return false;
    }

    IEnumerator DamageChasingDelay()
    {
        yield return new WaitForSeconds(5f); //todo 추후 변수로 변경 필요
        //현재 플레이어가 인지 범위 내에 있는지 확인
        if (distanceToPlayer < playerDetectionRange)
        {
            isDamage = false;
            currentState = MonsterStates.Chasing;
            //쫒기 상태 전환
        }
        else
        {
            isDamage = false;
            currentState = MonsterStates.Idle;
            //일반 상태 전환
        }

    }
    protected virtual void DropItems()
    {
        List<GameObject> droppedItems = new List<GameObject>();
        int k = 0; //아이템 종류 카운트
        for (int i = 0; i < dropItems.Length; i++) //드랍되는 아이템 종류만큼
        {
            for (int j = 0; j < maxDropItems[k]; j++) //아이템별 최대 드랍 갯수만큼
            {
                float itemPercent = Random.Range(0f, 100f); //아이템이 떨어지는 랜덤값 생성
                GameObject itemToDrop = null;
                if (itemPercent <= dropProbability[k]) //현재 아이템 퍼센트내에 충족되면 
                {
                    itemToDrop = dropItems[i];
                }
                if (itemToDrop != null)
                {
                    Vector3 dropPosition = transform.position + new Vector3(0f, 1f, 0f);
                    GameObject droppedItem = Instantiate(itemToDrop, dropPosition, Quaternion.identity);
                    droppedItems.Add(droppedItem);
                    // 현재 드랍된 아이템과 이전에 드랍된 아이템들 사이의 충돌 무시
                    for (int m = 0; m < droppedItems.Count - 1; m++)
                    {
                        Physics.IgnoreCollision(droppedItem.GetComponent<Collider>(), droppedItems[m].GetComponent<Collider>());
                    }
                }
            }
            k++;
        }
    }



    public void TurnOffAttack()
    {
        animator.SetBool("Attack", false);
        StartCoroutine(EndAttack());
    }

    IEnumerator EndAttack()
    {
        yield return new WaitForSeconds(damageDelayTime);
        Debug.Log("공격 대기 끝남");
        isAttack = false;
        currentState = MonsterStates.Idle;
    }

    private void LookPlayer()
    {
        if (distanceToPlayer <= playerDetectionRange)
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
        else
        {
            // 회전을 멈추도록 현재 회전을 유지합니다.
            transform.rotation = transform.rotation;
        }
    }

    // 감지 및 공격 범위 시각화
    private void OnDrawGizmos()//항상 보이게 //선택시 보이게 OnDrawGizmosSelected
    {
        Gizmos.color = Color.red; //감지 범위
        Gizmos.DrawWireSphere(transform.position, playerDetectionRange);

        Gizmos.color = Color.blue; // 공격 범위
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.green; //움직임 범위
        Gizmos.DrawWireSphere(initialPosition, randomMoveRange);

        Gizmos.color = Color.magenta; //플레이어 공격 인지 범위 
        Gizmos.DrawWireSphere(transform.position, damageRange);
    }
}