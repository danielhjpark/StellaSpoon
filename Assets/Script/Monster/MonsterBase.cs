using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class MonsterBase : MonoBehaviour
{
    public float detectionRange = 10f; //감지범위
    public float attackCooldown = 5f; //공격 딜레이
    public float dieDelay = 5f; //죽음 딜레이

    [SerializeField]
    protected int maxHp = 100;
    [SerializeField]
    protected int currentHP; //현재 체력

    protected GameObject player;
    protected float lastAttackTime;

    protected Animator animator; // Animator 컴포넌트
    [SerializeField]
    protected ThirdPersonController thirdPersonController;

    [SerializeField]
    protected bool collDamage = false;

    protected void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        currentHP = maxHp;

        thirdPersonController = player.GetComponent<ThirdPersonController>();
    }

    private void Update()
    {
        if (player.transform == null)
            return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if(distanceToPlayer<= detectionRange)
        {
            FacePlayer();
            if(Time.time >= lastAttackTime + attackCooldown)
            {
                Attack();
                lastAttackTime = Time.time;
            }
        }
    }

    protected void FacePlayer()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    protected abstract void Attack();

    public virtual void Damage(int bulletDamage)
    {
        currentHP -= bulletDamage;
        Debug.Log($"{gameObject.name} 체력: {currentHP}/{maxHp}");
        //몬스터 넉백
        //bulletDamage를 UI로 출력
        if (currentHP <= 0)
        {
            Die();
        }
        
    }
    protected virtual void Die()
    {
        Debug.Log($"{gameObject.name}이(가) 사망했습니다.");
        GetComponent<NavMeshAgent>().isStopped = true; //이동 멈춤
        GetComponent<Collider>().enabled = false; //충돌 제거
        animator.SetTrigger("Die");
        StartCoroutine(DieDelays());
    }

    private IEnumerator DieDelays()
    {
        yield return new WaitForSeconds(dieDelay);
        Destroy(gameObject);
    }

    protected IEnumerator AttackDelay()
    {
        collDamage = true;
        yield return new WaitForSeconds(3f);
        collDamage = false;
    }
}
