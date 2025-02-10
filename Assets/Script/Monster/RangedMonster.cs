using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedMonster : MonsterBase
{
    [Header("Basic Information")]
    [SerializeField]
    protected float attackDamage = 10f;  //���� ������
    [SerializeField]
    private float attackRange = 7f;   //���� ����

    [SerializeField]
    private float wanderRadius = 15f; //�������� �����̴� ����
    [SerializeField]
    private float wanderTime = 3f; //���� �̵� �ֱ�

    private float wanderTimer;

    private bool isPlayerDetected = false; //�÷��̾� ���� ����

    private Vector3 initialPosition; //�ʱ� ��ġ ����

    [SerializeField]
    private GameObject projectilePrefab; //��ô�� ������
    [SerializeField]
    private Transform firePoint; //��ô�� ������ġ

    private NavMeshAgent agent;      //������ NavMeshAgent

    [Header("Item")]
    public GameObject[] RangedItems;
    private int maxItem = 3;
    private float APercent = 99f;
    private float BPercent = 30f;
    private float CPercent = 10f;

    private new void Start()
    {
        base.Start(); //�θ� Ŭ���� �ʱ�ȭ
        agent = GetComponent<NavMeshAgent>();
        wanderTimer = wanderTime;

        initialPosition = transform.position; //�ʱ� ��ġ ����

        agent.avoidancePriority = Random.Range(30, 60); // ȸ�� �켱������ �������� ����
    }

    private void Update()
    {
        if (player == null) return;

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
            Vector3 targetDis = player.transform.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(targetDis);

            // �ε巴�� ȸ���ϵ��� Slerp ���
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                //�÷��̾� ������ �ٶ󺸰� ����
                //�÷��̾ �ٶ���� �� ����
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

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if(!collDamage)
            {
                Debug.Log("������");
                Vector3 attackerPosition = transform.position; // �÷��̾ �����ϴ� ����
                                                               //���⿡ �÷��̾�� �������� ������ ���� �߰�
                thirdPersonController.TakeDamage(attackDamage, attackerPosition);
                StartCoroutine(AttackDelay());
            }
        }
    }

    protected override void Attack()
    {
        animator.SetTrigger("Attack");
        Debug.Log("���Ÿ� ����!");
        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        //���⿡ �÷��̾�� �������� ������ ���� �߰�

        /*animator.SetBool("Attack", false);*/
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
        base.Die();
    }

    protected override void DropItem()
    {
        for (int i = 0; i < maxItem; i++)
        {
            float itemPercent = Random.Range(0f, 100f);
            GameObject itemToDrop = null;
            if (i == 0 && itemPercent <= APercent)
            {
                itemToDrop = RangedItems[0];
            }
            else if (i == 1 && itemPercent <= BPercent)
            {
                itemToDrop = RangedItems[1];
            }
            else if (i == 2 && itemPercent <= CPercent)
            {
                itemToDrop = RangedItems[2];
            }
            if (itemToDrop != null)
            {
                Vector3 dropPosition = transform.position + new Vector3(0f, 1f, 1f);
                Instantiate(itemToDrop, dropPosition, Quaternion.identity);
            }
        }
    }
}
