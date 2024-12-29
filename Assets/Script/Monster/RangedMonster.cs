using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedMonster : MonsterBase
{
    [Header("Basic Information")]
    [SerializeField]
    private float attackDamage = 10f;  //���� ������
    [SerializeField]
    private float attackRange = 7f;   //���� ����

    [SerializeField]
    private GameObject projectilePrefab; //��ô�� ������
    [SerializeField]
    private Transform firePoint; //��ô�� ������ġ

    private NavMeshAgent agent;      //������ NavMeshAgent
    private Animator animator;
    [SerializeField]
    private Collider collider;

    void Start()
    {
        base.Start(); //�θ� Ŭ���� �ʱ�ȭ
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider>();
    }

    void Update()
    {
        if (playerTf == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, playerTf.position);

        //���� ���� �ȿ� �ִ� ��� �÷��̾ ����
        if (distanceToPlayer <= detectionRange && distanceToPlayer > attackRange)
        {
            FollowPlayer();
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
        //���� ������ ��� ��� ����
        else
        {
            StopMoving();
        }
    }

    private void FollowPlayer()
    {
        if (agent != null && playerTf != null)
        {
            animator.SetBool("Walk", true);
            agent.isStopped = false;
            agent.SetDestination(playerTf.position);
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
        Debug.Log("���Ÿ� ����!");
        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        //���⿡ �÷��̾�� �������� ������ ���� �߰�

        /*animator.SetBool("Attack", false);*/
    }

    // ���� �� ���� ���� �ð�ȭ
    void OnDrawGizmos() //�׻� ���̰� //���ý� ���̰� OnDrawGizmosSelected
    {
        Gizmos.color = Color.red; //���� ����
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.blue; // ���� ����
        Gizmos.DrawWireSphere(transform.position, attackRange);
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
        collider.enabled = false; //�浹 ����
        animator.SetTrigger("Die");
        StartCoroutine(DieDelays());
    }

    IEnumerator DieDelays()
    {
        yield return new WaitForSeconds(dieDelay);

        Destroy(gameObject);
    }
}
