using StarterAssets;
using UnityEngine;

public class BearKingHand : MonoBehaviour
{

    public int attackDamage = 40;      // 공격 데미지

    public ThirdPersonController thirdPersonController; // 플레이어의 ThirdPersonController 스크립트 참조

    private void Start()
    {
        thirdPersonController = GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdPersonController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"{other.name}가 공격을 맞았습니다!");

            thirdPersonController.TakeDamage(attackDamage, transform.position); //플레이어 데미지

        }
    }
}