using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static UnityEditor.Rendering.ShadowCascadeGUI;


public enum MonsterStates //���� ����
{
    Idle, //�Ϲ� ����
    Attack, //���� ����
    Chasing, //�i�� ����
    RandomMove, //���� �̵� ����
    Death, //���� ����
}
public abstract class MonsterBase : MonoBehaviour
{
    [Header("���� Information")]
    public int maxHealth; //�ִ� ü��
    public int currentHealth; //���� ü��
    public int damage; //���ݷ�
    protected Vector3 initialPosition; //���� ��ġ
    public bool isDead = false; //���� üũ ����
    protected bool isMove = false; //������ üũ ����
    protected float wanderTimer;
    protected float lastAttackTime; //������ ���� �ð�
    public bool isAttack = false; //������ üũ ����
    public bool attackColl = false; //���� �浹 üũ ����
    public bool inAttackRange = false; //���� ���� ���� �ִ��� üũ ����

    [Header("���� ���� ������ �ð�")]
    public float damageDelayTime; //���� ������ �ð�
    [Header("���� �����̵� ���ð�")]
    public float idleMoveInterval; //���� �̵� ���ð�

    [Header("����")]
    public float attackRange; //���� ����
    public float playerDetectionRange; //���� ����
    public float randomMoveRange; //���� �̵� ����
    public float damageRange; //�÷��̾� ���� ���� ����

    [Header("��� ������")]
    public GameObject[] dropItems; //��� ������ ����Ʈ
    public float[] dropProbability; //������ �� ��� Ȯ��
    public int[] maxDropItems; //������ �� �ִ� ��� ����

    protected MonsterStates currentState;
    protected MonsterStates previousState; // ���� ���� �����
    protected NavMeshAgent nav;
    protected Animator animator;
    protected GameObject player;
    protected Collider coll;
    protected Rigidbody rb; //������ ������ٵ�
    [SerializeField]
    protected ThirdPersonController thirdPersonController;

    private bool canDamage; //�÷��̾� ���� ���� ���� ���� �ִ��� üũ ����
    [SerializeField]
    protected bool isDamage; //�������� �Ծ����� üũ ����
    private bool RandomPositionDecide = false; //���� ��ΰ� ���������� üũ ����

    protected float distanceToPlayer; //�÷��̾���� �Ÿ�

    [Header("Health UI")]
    [SerializeField]
    protected Slider HealthSlider;
    [SerializeField]
    protected GameObject sliderFill;

    protected virtual void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();
        thirdPersonController = player.GetComponent<ThirdPersonController>();

        initialPosition = transform.position; //���� ��ġ ����
        currentHealth = maxHealth;

        currentState = MonsterStates.Idle; //���۽� Idle����

        //�ʱⰪ ����
        isAttack = false; //�������� �ƴ�
        isDamage = false; //�ǰ����� �ƴ�
        isMove = false; //�������� �ƴ�
        canDamage = false; //�÷��̾� ���� ���� ���� ���� ����
        inAttackRange = false; //���� ���� ���� ����
        isDead = false; //������ �ƴ�

        lastAttackTime = -damageDelayTime; //���� ������ �ʱ�ȭ

        nav.avoidancePriority = Random.Range(30, 60); // ȸ�� �켱������ �������� ����

        if (HealthSlider != null)
        {
            HealthSlider.maxValue = maxHealth;
            HealthSlider.value = currentHealth;
        }
    }

    protected void Update()
    {
        // ü�� �����̴� ������Ʈ
        if (HealthSlider != null)
        {
            HealthSlider.value = currentHealth;

            sliderFill.SetActive(currentHealth > 0);
            if(currentHealth <=0)
            {
                sliderFill.SetActive(false); //ü���� 0 ������ �� �����̴� ��Ȱ��ȭ
            }
        }

        if (!isDead)
        {
            if (currentHealth <= 0)
            {
                currentState = MonsterStates.Death;
            }
            else
            {
                distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
                if (!isAttack || !isDamage) //�������� �ƴҶ�
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
                        if (this is BearKingMonster && !BearKingMonster.isJumping)
                        {
                            nav.ResetPath();
                        }
                        else
                        {
                            nav.ResetPath();
                        }
                        inAttackRange = true;
                        currentState = MonsterStates.Attack;
                    }
                    else if (distanceToPlayer > attackRange && distanceToPlayer <= playerDetectionRange)
                    {
                        currentState = MonsterStates.Chasing;
                        inAttackRange = false;
                    }
                    else
                    {
                        inAttackRange = false;
                    }
                    HandleState();
                }
                if (!(this is EscapeMonster))
                {
                    if ((this is BearKingMonster))
                    {
                        if (!BearKingMonster.isChargeSetting)
                        {
                            LookPlayer();
                        }
                    }
                    else if ((this is WolfKingMonster))
                    {
                        if (!WolfKingMonster.isThrowWarning)
                        {
                            LookPlayer();
                        }
                    }
                    else
                    {
                        LookPlayer();
                    }
                }
            }
        }
    }
    protected void HandleState()
    {
        switch (currentState)
        {
            case MonsterStates.Idle:
                HandleIdle();
                //Debug.Log("�Ϲ� ����");
                break;

            case MonsterStates.Attack:
                //Debug.Log("���� ����");
                HandleAttack();
                break;

            case MonsterStates.Chasing:
                //Debug.Log("�i�� ����");
                HandleChasing();
                break;

            case MonsterStates.RandomMove:
                //Debug.Log("���� �̵� ����");
                HandleRandomMove();
                break;
            case MonsterStates.Death:
                //Debug.Log("���� ����");
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
        animator.SetBool("Walk", false);
        if (!isAttack)
        {
            isAttack = true;
            animator.SetBool("Walk", false);
            animator.SetBool("Attack", true); //Attack �ִϸ��̼� ����
        }

    }
    protected virtual void HandleChasing()
    {
        if (!canDamage && isDamage)
        {
            currentState = MonsterStates.Idle;
            HandleState();
            return;
        }
        animator.SetBool("Walk", true);
        nav.SetDestination((player.transform.position) - (player.transform.position - transform.position).normalized * 1f);
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
        if (!animator.GetBool("Walk"))
        {
            animator.SetBool("Walk", true);
        }
        wanderTimer += Time.deltaTime; //�ð� üũ
        if (!nav.pathPending && nav.remainingDistance <= 0.2f) //���������� 0.1 ���� �ִ���
        {
            animator.SetBool("Walk", false);
            currentState = MonsterStates.Idle;
            HandleState();
            RandomPositionDecide = false; // ���� ��� ���� ����
        }

        if (wanderTimer >= idleMoveInterval)
        {
            if (!RandomPositionDecide) // ���� ��ΰ� �����Ǿ��� ��
            {
                //Debug.Log("���� ��ġ ����");
                Vector3 newDestination = GetRandomPoint(initialPosition, randomMoveRange);
                nav.SetDestination(newDestination);
                RandomPositionDecide = true;
                animator.SetBool("Walk", true);
                wanderTimer = 0; // Ÿ�̸� �ʱ�ȭ
            }
            //���� ��ġ ����
        }
        //randomMoveRange ���� ������ ������ ��ġ ���� //�Լ��� ����� �� �ѹ��� ����
        //������ ��ġ�� �̵�

    }
    protected virtual void HandleDeath()
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.type == AnimatorControllerParameterType.Bool)
            {
                animator.SetBool(param.name, false);
            }
        }
        //Debug.Log($"{gameObject.name}��(��) ����߽��ϴ�.");
        currentHealth = 0;
        //�ִϸ��̼� ���� ����
        animator.SetTrigger("Die");
        //�߷� ����
        rb.useGravity = false; //�߷� ��� ����
        rb.isKinematic = true; //������������ ����
        //�浹 ����
        coll.enabled = false;
        //�׺�Ž�����
        nav.ResetPath();
        nav.isStopped = true;
        nav.enabled = false;
        StartCoroutine(DeathDelay());
        //������ ���
        DropItems();
        isDead = true; //���� ���·� ����
        Manager.KillMonsterCount++;
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

    public virtual void Damage(int damage)
    {
        //todo�ڷ� �˹��ϴ� �ڵ� �ʿ�
        currentHealth -= damage;
        //Debug.Log(damage + " ������ ����! " + currentHealth + " ü�� ����");
        nav.isStopped = true;
        animator.SetBool("Walk", false);
        if (currentHealth <= 0)
        {
            currentState = MonsterStates.Death;
            HandleState();
        }
        else
        {
            previousState = currentState; //���� ���� ����

            if(this.name != "Bypin")
            {
                if (!isDamage) //ù �ǰ��� ��
                {
                    isDamage = true;
                    animator.SetBool("Hit", true);
                }
                else
                {
                    animator.Play("GetHit", 0, 0f); //�ִϸ��̼��� �̸��� GetHit �̿��� ��.
                }
            }
            else
            {
                StartCoroutine(KnockbackCoroutine());
            }
        }
        if (canDamage)
        {
            isDamage = true;
            //�÷��̾� �i��
            currentState = MonsterStates.Chasing;
        }
    }

    private IEnumerator KnockbackCoroutine()
    {
        // �˹� ó��
        Vector3 knockbackDir = (transform.position - player.transform.position).normalized;
        float knockbackForce = 3f; // �˹� �Ÿ�
        float knockbackTime = 0.2f; // �˹� ���� �ð�

        float elapsed = 0f;
        while (elapsed < knockbackTime)
        {
            nav.Move(knockbackDir * knockbackForce * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.3f); // �˹� �� ��� ���

        // �˹� �� ���� ����
        animator.SetBool("Hit", false);
        nav.isStopped = false;
        isDamage = false;
        currentState = previousState; // ���� ���·� ����
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
            //Debug.Log("���ݹ��� ����");
            return true;
        }
        return false;
    }
    protected bool CanSeePlayer()
    {
        if (distanceToPlayer <= playerDetectionRange)
        {
            //Debug.Log("���� ���� ����");
            return true;
        }
        return false;
    }
    protected bool CanDamage()
    {
        if (distanceToPlayer <= damageRange)
        {
            //Debug.Log("������ ���� ����");
            return true;
        }
        return false;
    }

    IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(5f);
        gameObject.SetActive(false);
        coll.enabled = true;
        currentHealth = maxHealth;
        currentState = MonsterStates.Idle;
        isDead = false;
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
                    Vector3 dropPosition = transform.position + new Vector3(0f, 2f, 0f);
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

    public void AttackCheck()
    {
        if (attackColl)
        {
            PlayerDamage();
        }
    }

    public bool IsAttacking()
    {
        // ���� �ִϸ��̼��� ���� ������ Ȯ��
        return animator.GetCurrentAnimatorStateInfo(0).IsName("Attack");
    }

    public void PlayerDamage()
    {
        thirdPersonController.TakeDamage(damage, transform.position); //�÷��̾� ������
    }
    public void TurnOffAttack()
    {
        animator.SetBool("Attack", false);
        StartCoroutine(EndAttack());
    }

    public void TurnOffDamage()
    {
        animator.SetBool("Hit", false);
        nav.isStopped = false;
        isDamage = false;

        currentState = previousState; //���� ���·� ����
        HandleState();
    }

    IEnumerator EndAttack()
    {
        yield return new WaitForSeconds(damageDelayTime);
        //Debug.Log("���� ��� ����");
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
    protected virtual void OnDrawGizmosSelected()//���ý� ���̰� //�׻� ���̰� OnDrawGizmos
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