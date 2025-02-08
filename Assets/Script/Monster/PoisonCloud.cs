using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonCloud : MonoBehaviour
{
    public int damage = 5;  // 플레이어에게 줄 피해량
    public float damageInterval = 1f;  // 지속 피해 간격

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
        Vector3 attackerPosition = transform.position; // 플레이어를 공격하는 방향

        while (!thirdPersonController.isDie)
        {
            thirdPersonController.TakeDamage(damage, attackerPosition);
            yield return new WaitForSeconds(damageInterval);
        }
    }
}
