using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum MonsterStates //���� ����
{
    Idle, //�Ϲ� ����
    Attack, //���� ����
    Chasing, //�i�� ����
    RandomMove, //���� �̵� ����
    Death //���� ����
}
public abstract class MonsterBase1 : MonoBehaviour
{
    [Header("Basic information")]
    public int maxHealth; //�ִ� ü��
    public int currentHealth; //���� ü��
    public int damage; //���ݷ�
    protected Vector3 initialPosition; //���� ��ġ
    protected bool isDead; //���� üũ ����
    protected bool isMove; //������ üũ ����
    protected float wanderTimer;
    protected float idleMoveInterval; //���� �̵� ���ð�
    protected float damageDelayTime; //���� ������ �ð�
    protected float lastAttackTime; //������ ���� �ð�
    protected bool isAttack = false; //������ üũ ����

    [Header("Range")]
    public float attackRange; //���� ����
    public float playerDetectionRange; //���� ����
    public float randomMoveRange; //���� �̵� ����
    public float damageRange; //�÷��̾� ���� ���� ����

    [Header("Drop item")]
    public GameObject[] dropItems; //��� ������ ����Ʈ
    public float[] dropProbability; //������ �� ��� Ȯ��
    public int[] maxDropItems; //������ �� �ִ� ��� ����

    protected MonsterStates currentState;
    protected NavMeshAgent nav;
    protected Animator animator;
    protected GameObject player;
    protected Collider coll;
    protected ThirdPersonController thirdPersonController;

    [SerializeField]
    private bool canDamage; //�÷��̾� ���� ���� ���� ���� �ִ��� üũ ����
    private bool isDamage; //�������� �Ծ����� üũ ����
    private bool RandomPositionDecide = false; //���� ��ΰ� ���������� üũ ����

    [SerializeField]
    private float distanceToPlayer; //�÷��̾���� �Ÿ�

    protected void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        coll = GetComponent<Collider>();
        thirdPersonController = player.GetComponent<ThirdPersonController>();

        initialPosition = transform.position; //���� ��ġ ����
        currentHealth = maxHealth;

        currentState = MonsterStates.Idle; //���۽� Idle����

        lastAttackTime = -damageDelayTime; //���� ������ �ʱ�ȭ
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
                if (!isAttack) //�������� �ƴҶ�
                {

                    //������ ���� ������ �������� ������ ���� 
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
                Debug.Log("�Ϲ� ����");
                break;

            case MonsterStates.Attack:
                Debug.Log("���� ����");
                HandleAttack();
                break;

            case MonsterStates.Chasing:
                Debug.Log("�i�� ����");
                HandleChasing();
                break;

            case MonsterStates.RandomMove:
                Debug.Log("���� �̵� ����");
                HandleRandomMove();
                break;
            case MonsterStates.Death:
                Debug.Log("���� ����");
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
        //�ƹ� �������� ������ �ʾҴٸ� �����̵����� ��ȯ
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
            animator.SetBool("Attack", true); //Attack �ִϸ��̼� ����
            thirdPersonController.TakeDamage(damage, transform.position); //�÷��̾� ������
            //�÷��̾�� ������ �ֱ�
        }

    }
    protected virtual void HandleChasing()
    {
        animator.SetBool("Walk", true);
        nav.SetDestination((player.transform.position) - (player.transform.position - transform.position).normalized * 0.8f);
        if (canDamage && isDamage)
        {
            //5�� �ڿ� idle ���·� ��ȯ
            StartCoroutine(DamageChasingDelay());
        }
        else
        {
            if (distanceToPlayer > playerDetectionRange)
            {
                currentState = MonsterStates.Idle;
            }
        }
        //�÷��̾��� ��ġ �޾ƿͼ� ��� ����
        //Walk �ִϸ��̼� ����
    }
    protected virtual void HandleRandomMove()
    {
        wanderTimer += Time.deltaTime; //�ð� üũ
        if (wanderTimer >= idleMoveInterval)
        {
            if (!RandomPositionDecide) // ���� ��ΰ� �������� �ʾ��� ��
            {
                Debug.Log("���� ��ġ ����");
                Vector3 newDestination = GetRandomPoint(initialPosition, randomMoveRange);
                nav.SetDestination(newDestination);
                RandomPositionDecide = true;
                animator.SetBool("Walk", true);
                wanderTimer = 0; // Ÿ�̸� �ʱ�ȭ
            }
            else
            {
                // ������ ��ġ�� �̵�
                if (!nav.pathPending && nav.remainingDistance <= 0.5f) //���������� 0.3 ���� �ִ���
                {
                    animator.SetBool("Walk", false);
                    currentState = MonsterStates.Idle;
                    RandomPositionDecide = false; // ���� ��� ���� ����
                }
            }
            //���� ��ġ ����
        }
        //randomMoveRange ���� ������ ������ ��ġ ���� //�Լ��� ����� �� �ѹ��� ����
        //������ ��ġ�� �̵�

    }
    protected virtual void HandleDeath()
    {
        animator.SetBool("Walk", false);

        Debug.Log($"{gameObject.name}��(��) ����߽��ϴ�.");
        currentHealth = 0;
        //�ִϸ��̼� ���� ����
        animator.SetTrigger("Die");
        //�浹 ����
        coll.enabled = false;
        //�׺�Ž�����
        nav.ResetPath();
        //5�ʵ� ������Ʈ ����
        Destroy(gameObject, 5f);
        //������ ���
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
        Debug.Log(damage + " ������ ����! " + currentHealth + " ü�� ����");
        if (currentHealth <= 0)
        {
            isDead = true;
            currentState = MonsterStates.Death;
            HandleState();
        }
        if (canDamage)
        {
            isDamage = true;
            //�÷��̾� �i��
            currentState = MonsterStates.Chasing;
        }
    }

    protected bool IsHpZero() //ü���� 0 �������� �˻�
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
            Debug.Log("���ݹ��� ����");
            return true;
        }
        return false;
    }
    protected bool CanSeePlayer()
    {
        if (distanceToPlayer <= playerDetectionRange)
        {
            Debug.Log("���� ���� ����");
            return true;
        }
        return false;
    }
    protected bool CanDamage()
    {
        if (distanceToPlayer <= damageRange)
        {
            Debug.Log("������ ���� ����");
            return true;
        }
        return false;
    }

    IEnumerator DamageChasingDelay()
    {
        yield return new WaitForSeconds(5f); //todo ���� ������ ���� �ʿ�
        //���� �÷��̾ ���� ���� ���� �ִ��� Ȯ��
        if (distanceToPlayer < playerDetectionRange)
        {
            isDamage = false;
            currentState = MonsterStates.Chasing;
            //�i�� ���� ��ȯ
        }
        else
        {
            isDamage = false;
            currentState = MonsterStates.Idle;
            //�Ϲ� ���� ��ȯ
        }

    }
    protected virtual void DropItems()
    {
        List<GameObject> droppedItems = new List<GameObject>();
        int k = 0; //������ ���� ī��Ʈ
        for (int i = 0; i < dropItems.Length; i++) //����Ǵ� ������ ������ŭ
        {
            for (int j = 0; j < maxDropItems[k]; j++) //�����ۺ� �ִ� ��� ������ŭ
            {
                float itemPercent = Random.Range(0f, 100f); //�������� �������� ������ ����
                GameObject itemToDrop = null;
                if (itemPercent <= dropProbability[k]) //���� ������ �ۼ�Ʈ���� �����Ǹ� 
                {
                    itemToDrop = dropItems[i];
                }
                if (itemToDrop != null)
                {
                    Vector3 dropPosition = transform.position + new Vector3(0f, 1f, 0f);
                    GameObject droppedItem = Instantiate(itemToDrop, dropPosition, Quaternion.identity);
                    droppedItems.Add(droppedItem);
                    // ���� ����� �����۰� ������ ����� �����۵� ������ �浹 ����
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
        Debug.Log("���� ��� ����");
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
            // ȸ���� ���ߵ��� ���� ȸ���� �����մϴ�.
            transform.rotation = transform.rotation;
        }
    }

    // ���� �� ���� ���� �ð�ȭ
    private void OnDrawGizmos()//�׻� ���̰� //���ý� ���̰� OnDrawGizmosSelected
    {
        Gizmos.color = Color.red; //���� ����
        Gizmos.DrawWireSphere(transform.position, playerDetectionRange);

        Gizmos.color = Color.blue; // ���� ����
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.green; //������ ����
        Gizmos.DrawWireSphere(initialPosition, randomMoveRange);

        Gizmos.color = Color.magenta; //�÷��̾� ���� ���� ���� 
        Gizmos.DrawWireSphere(transform.position, damageRange);
    }
}