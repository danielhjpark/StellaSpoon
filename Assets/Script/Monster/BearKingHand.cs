using UnityEngine;

public class BearKingHand : MonoBehaviour
{

    public int attackDamage = 10;      // 공격 데미지


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"{other.name}가 공격을 맞았습니다!");

            // 플레이어에 데미지 처리 로직
            
        }
    }
}