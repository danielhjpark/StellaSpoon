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
            // ���� ������ ī�޶�� ���ϰ�
            Vector3 dir = Camera.main.transform.position - transform.position;
            dir.y = 0; // y�� ����
            transform.rotation = Quaternion.LookRotation(-dir);
        }
    }
}
