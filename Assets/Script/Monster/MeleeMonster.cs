using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeMonster : MonsterBase1
{
    private void Start()
    {
        base.Start();
        maxHealth = 100;
        currentHealth = maxHealth;
        damage = 10;
        idleMoveInterval = 2f;
        damageDelayTime = 5f;


        isDead = false;
        isMove = false;

        attackRange = 3f;
        playerDetectionRange = 5f;
        randomMoveRange = 7f;
        damageRange = 10f;
    }

    // 감지 및 공격 범위 시각화
    private void OnDrawGizmos()//항상 보이게 //선택시 보이게 OnDrawGizmosSelected
    {
        Gizmos.color = Color.red; //감지 범위
        Gizmos.DrawWireSphere(transform.position, playerDetectionRange);

        Gizmos.color = Color.blue; // 공격 범위
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.green; //움직임 범위
        Gizmos.DrawWireSphere(initialPosition, randomMoveRange);

        Gizmos.color = Color.magenta; //플레이어 공격 인지 범위 
        Gizmos.DrawWireSphere (transform.position, damageRange);
    }

    protected override void DropItems()
    {
        List<GameObject> droppedItems = new List<GameObject>();
        int k = 0; //아이템 종류 카운트
        for(int i = 0; i < dropItems.Length; i++) //드랍되는 아이템 종류만큼
        {
            for(int j = 0; j < maxDropItems[k]; j++) //아이템별 최대 드랍 갯수만큼
            {
                float itemPercent = Random.Range(0f, 100f); //아이템이 떨어지는 랜덤값 생성
                GameObject itemToDrop = null;
                if(itemPercent <= dropProbability[k]) //현재 아이템 퍼센트내에 충족되면 
                {
                    itemToDrop = dropItems[i];
                }
                if(itemToDrop != null)
                {
                    Vector3 dropPosition = transform.position + new Vector3(0f, 1f, 0f);
                    GameObject droppedItem = Instantiate(itemToDrop, dropPosition, Quaternion.identity);
                    droppedItems.Add(droppedItem);
                    // 현재 드랍된 아이템과 이전에 드랍된 아이템들 사이의 충돌 무시
                    for (int m = 0; m < droppedItems.Count - 1; m++)
                    {
                        Physics.IgnoreCollision(droppedItem.GetComponent<Collider>(), droppedItems[m].GetComponent<Collider>());
                    }
                }
            }
            k++;
        }
    }
}
