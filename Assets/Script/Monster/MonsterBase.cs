using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterBase : MonoBehaviour
{
    public float detectionRange = 10f; //감지범위
    public float attackCooldown = 2f; //공격 딜레이
    public float dieDelay = 5f; //죽음 딜레이

    [SerializeField]
    protected int maxHp = 100;
    [SerializeField]
    protected int currentHP; //현재 체력

    protected Transform playerTf;
    protected float lastAttackTime;

    protected Animator animator; // Animator 컴포넌트

    protected void Start()
    {
        playerTf = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        currentHP = maxHp;
    }

    private void Update()
    {
        if (playerTf == null)
            return;

        float distanceToPlayer = Vector3.Distance(transform.position, playerTf.position);

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
        Vector3 direction = (playerTf.position - transform.position).normalized;
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
    }
}
