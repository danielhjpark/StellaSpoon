using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHPBar : MonoBehaviour
{
    public Slider hpSlider;
    public MonsterBase monster;

    void Update()
    {
        if (monster != null)
        {
            hpSlider.value = monster.currentHealth;
            // 몬스터 방향을 카메라로 향하게
            Vector3 dir = Camera.main.transform.position - transform.position;
            dir.y = 0; // y축 고정
            transform.rotation = Quaternion.LookRotation(-dir);
        }
    }
}
