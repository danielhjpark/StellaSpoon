using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BypinMonster : RangedMonster
{
    public override void Damage(int damage)
    {
        Debug.Log("BypinMonster Damage called with damage: " + damage);
        if (currentState == MonsterStates.Death) return; // �̹� �׾����� ����

        //todo�ڷ� �˹��ϴ� �ڵ� �ʿ�
        currentHealth -= damage;
        //Debug.Log(damage + " ������ ����! " + currentHealth + " ü�� ����");
        nav.isStopped = true;

        if (currentHealth <= 0)
        {
            currentState = MonsterStates.Death;
            HandleState();
            return;
        }

        animator.SetBool("Walk", false);

        previousState = currentState; //���� ���� ����

        isDamage = true; // ������ ���·� ����

        StartCoroutine(KnockbackCoroutine());
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
}
