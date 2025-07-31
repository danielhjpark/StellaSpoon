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

    //트리거가 충돌했을 때 호출되는 함수
    private void OnTriggerEnter(Collider other)
    {
        //충돌한 객체가 "Player" 태그를 가진 경우
        if (other.CompareTag("Player"))
        {
            //1초 뒤부터 1초 간격으로 데미지를 주는 코루틴 시작
            if (damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(DealDamageOverTime(other));
            }
        }
    }

    //트리거에서 벗어났을 때 호출되는 함수 (플레이어가 Pillar를 벗어나면 데미지 반복을 멈춤)
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //코루틴을 중단시켜 데미지 반복을 멈춤
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }
    }

    //1초 간격으로 데미지를 주는 코루틴
    private IEnumerator DealDamageOverTime(Collider player)
    {
        //1초 딜레이 후 데미지 시작
        yield return new WaitForSeconds(1f);

        while (player != null && player.CompareTag("Player"))
        {
            //데미지 주기
            thirdPersonController.TakeDamage(pillarDamage, transform.position);

            //1초 간격으로 데미지 주기
            yield return new WaitForSeconds(1f);
        }

        //코루틴 종료 후 damageCoroutine을 null로 설정
        damageCoroutine = null;
    }
}
