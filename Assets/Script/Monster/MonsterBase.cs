using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterBase : MonoBehaviour
{
    public float detectionRange = 10f; //°¨Áö¹üÀ§
    public float attackCooldown = 2f; //°ø°Ý µô·¹ÀÌ
    public float DieDelay = 5f; //Á×À½ µô·¹ÀÌ

    protected Transform playerTf;
    protected float lastAttackTime;

    protected void Start()
    {
        playerTf = GameObject.FindGameObjectWithTag("Player").transform;
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

    protected abstract void Damage();
    protected abstract void Die();
}
