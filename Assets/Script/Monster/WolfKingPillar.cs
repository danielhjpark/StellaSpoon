using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfKingPillar : MonoBehaviour
{
    protected ThirdPersonController thirdPersonController;

    [SerializeField]
    private int pillarDamage;


    private Coroutine damageCoroutine;

    private void Start()
    {
        thirdPersonController = GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdPersonController>();
    }

    //Ʈ���Ű� �浹���� �� ȣ��Ǵ� �Լ�
    private void OnTriggerEnter(Collider other)
    {
        //�浹�� ��ü�� "Player" �±׸� ���� ���
        if (other.CompareTag("Player"))
        {
            //1�� �ں��� 1�� �������� �������� �ִ� �ڷ�ƾ ����
            if (damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(DealDamageOverTime(other));
            }
        }
    }

    //Ʈ���ſ��� ����� �� ȣ��Ǵ� �Լ� (�÷��̾ Pillar�� ����� ������ �ݺ��� ����)
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //�ڷ�ƾ�� �ߴܽ��� ������ �ݺ��� ����
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }
    }

    //1�� �������� �������� �ִ� �ڷ�ƾ
    private IEnumerator DealDamageOverTime(Collider player)
    {
        //1�� ������ �� ������ ����
        yield return new WaitForSeconds(1f);

        while (player != null && player.CompareTag("Player"))
        {
            //������ �ֱ�
            thirdPersonController.TakeDamage(pillarDamage, transform.position);

            //1�� �������� ������ �ֱ�
            yield return new WaitForSeconds(1f);
        }

        //�ڷ�ƾ ���� �� damageCoroutine�� null�� ����
        damageCoroutine = null;
    }
}
