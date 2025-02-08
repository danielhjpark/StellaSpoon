using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonCloud : MonoBehaviour
{
    public int damage = 5;  // �÷��̾�� �� ���ط�
    public float damageInterval = 1f;  // ���� ���� ����

    [SerializeField]
    protected ThirdPersonController thirdPersonController;

    private void Start()
    {
        thirdPersonController = GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdPersonController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(ApplyPoisonDamage(other));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopAllCoroutines();
        }
    }

    IEnumerator ApplyPoisonDamage(Collider player)
    {
        Vector3 attackerPosition = transform.position; // �÷��̾ �����ϴ� ����

        while (!thirdPersonController.isDie)
        {
            thirdPersonController.TakeDamage(damage, attackerPosition);
            yield return new WaitForSeconds(damageInterval);
        }
    }
}
