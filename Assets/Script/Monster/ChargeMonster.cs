using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChargeMonster : MonsterBase
{
    [Header("Basic Information")]
    [SerializeField]
    private float attackDamage = 10f;//���� ������
    [SerializeField]
    private float attackRange = 5f;//���� ����
    [SerializeField]
    private float chargeSpeed = 10f; //���ݼӵ�
    [SerializeField]
    private float chargeDuration = 2f;

    [SerializeField]
    private float wanderRadius = 15f; //�������� �����̴� ����
    [SerializeField]
    private float wanderTime = 3f; //���� �̵� �ֱ�

    private float wanderTimer;

    private bool isPlayerDetected = false; //�÷��̾� ���� ����

    private Vector3 initialPosition; //�ʱ� ��ġ ����


    private bool isCharging = false;

    private NavMeshAgent agent;      //������ NavMeshAgent
    private Collider ChargeMonsterCollider;

    private new void Start()
    {
        base.Start(); //�θ� Ŭ���� �ʱ�ȭ
        agent = GetComponent<NavMeshAgent>();
        ChargeMonsterCollider = GetComponent<Collider>();
        wanderTimer = wanderTime;

        initialPosition = transform.position; //�ʱ� ��ġ ����

        agent.avoidancePriority = Random.Range(30, 60); // ȸ�� �켱������ �������� ����
    }

    private void Update()
    {
        if (player.transform == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        //���� ���� �ȿ� �ִ� ��� �÷��̾ ����
        if (distanceToPlayer <= detectionRange && distanceToPlayer > attackRange)
        {
            FollowPlayer();
            isPlayerDetected = true;
        }
        //���� ���� �ȿ� �ִ� ��� ����
        else if (distanceToPlayer <= attackRange)
        {
            StopMoving();
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                Attack();
                lastAttackTime = Time.time;
            }
        }
        //���� ������ ��� ��� ���� ��ġ�� �̵�
        else
        {
            if (isPlayerDetected)
            {
                isPlayerDetected = false;
                if (agent.hasPath)
                {
                    agent.ResetPath(); // ���� ��� �ʱ�ȭ
                }
            }
            HandleRandomMovement();
        }
    }

    private void HandleRandomMovement()
    {
        wanderTimer += Time.deltaTime;

        if (wanderTimer >= wanderTime)
        {
            Vector3 newDestination = getRandomPoint(initialPosition, wanderRadius);
            agent.SetDestination(newDestination);
            wanderTimer = 0f;
            animator.SetBool("Walk", true);
        }

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            animator.SetBool("Walk", false);
        }
    }
    private Vector3 getRandomPoint(Vector3 center, float radius)
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

    private void FollowPlayer()
    {
        if (agent != null && player.transform != null)
        {
            animator.SetBool("Walk", true);
            agent.isStopped = false;
            agent.SetDestination(player.transform.position);
            wanderTimer = 0f;
        }
    }

    private void StopMoving()
    {
        if (agent != null)
        {
            animator.SetBool("Walk", false);
            agent.isStopped = true;
        }
    }

    protected override void Attack()
    {
        animator.SetTrigger("Attack");
        if (!isCharging)
        {
            Debug.Log("���� ����!");
            StartCoroutine(Charge());
        }
        //���⿡ �÷��̾�� �������� ������ ���� �߰�

        /*animator.SetBool("Attack", false);*/
    }

    private IEnumerator Charge()
    {
        isCharging = true;
        Vector3 targetPosition = player.transform.position - (player.transform.position - transform.position).normalized * 0.4f; //��ǥ���� 0.4 �տ� ���߰�

        // ���� ���� �� NavMeshAgent ��Ȱ��ȭ (���� �̵��� ����)
        agent.isStopped = true;

        float startTime = Time.time;

        while (Time.time < startTime + chargeDuration)
        {
            // ��ǥ ��ġ������ �Ÿ� ���
            float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

            // ��ǥ�� �����ϸ� ���� ����
            if (distanceToTarget <= 1f)
            {
                break;
            }

            // ��ǥ �������� �̵�
            Vector3 direction = (targetPosition - transform.position).normalized;
            transform.position += direction * chargeSpeed * Time.deltaTime;

            yield return null;
        }

        Debug.Log("���� ����!");
        isCharging = false;

        // NavMeshAgent �ٽ� Ȱ��ȭ
        agent.isStopped = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && isCharging)
        {
            Debug.Log("�÷��̾�� �浹! ���� ����.");
            StopCharging();
            Vector3 attackerPosition = transform.position; // �÷��̾ �����ϴ� ����
            thirdPersonController.TakeDamage(attackDamage, attackerPosition);
        }
    }

    private void StopCharging()
    {
        isCharging = false;
        agent.isStopped = false; //NavMeshAgent ��Ȱ��ȭ
    }

    // ���� �� ���� ���� �ð�ȭ
    private void OnDrawGizmos() //�׻� ���̰� //���ý� ���̰� OnDrawGizmosSelected
    {
        Gizmos.color = Color.red; //���� ����
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.blue; // ���� ����
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.green; //������ ����
        Gizmos.DrawWireSphere(initialPosition, wanderRadius);
    }

    public override void Damage(int bulletDamage)
    {
        base.Damage(bulletDamage);
        if (animator != null)
        {
            animator.SetTrigger("Hit");
        }
    }

    protected override void Die()
    {
        agent.isStopped = true; //�̵� ���߱�
        ChargeMonsterCollider.enabled = false; //�浹 ����
        animator.SetTrigger("Die");
        StartCoroutine(DieDelays());
    }

    IEnumerator DieDelays()
    {
        yield return new WaitForSeconds(dieDelay);

        Destroy(gameObject);
    }
}
