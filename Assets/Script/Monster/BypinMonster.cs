using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BypinMonster : RangedMonster
{
    public override void Damage(int damage)
    {
        Debug.Log("BypinMonster Damage called with damage: " + damage);
        if (currentState == MonsterStates.Death) return; // 이미 죽었으면 무시

        //todo뒤로 넉백하는 코드 필요
        currentHealth -= damage;
        //Debug.Log(damage + " 데미지 입음! " + currentHealth + " 체력 남음");
        nav.isStopped = true;

        if (currentHealth <= 0)
        {
            currentState = MonsterStates.Death;
            HandleState();
            return;
        }

        animator.SetBool("Walk", false);

        previousState = currentState; //이전 상태 저장

        isDamage = true; // 데미지 상태로 변경

        StartCoroutine(KnockbackCoroutine());
        if (canDamage)
        {
            isDamage = true;
            //플레이어 쫒기
            currentState = MonsterStates.Chasing;
        }
    }
    private IEnumerator KnockbackCoroutine()
    {
        // 넉백 처리
        Vector3 knockbackDir = (transform.position - player.transform.position).normalized;
        float knockbackForce = 3f; // 넉백 거리
        float knockbackTime = 0.2f; // 넉백 지속 시간

        float elapsed = 0f;
        while (elapsed < knockbackTime)
        {
            nav.Move(knockbackDir * knockbackForce * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.3f); // 넉백 후 잠시 대기

        // 넉백 후 상태 복귀
        animator.SetBool("Hit", false);
        nav.isStopped = false;
        isDamage = false;
        currentState = previousState; // 이전 상태로 복귀
    }
}
