using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeWeapon : MonoBehaviour
{
    [SerializeField]
    private GameObject weaponChangeUI; // 무기 변경 UI

    private bool collPlayer = false; //플레이어 충돌체크

    void Update()
    {
        if(!weaponChangeUI.activeSelf && collPlayer && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("WeaponUION");
        }
        if(weaponChangeUI.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("WeaponUIOff");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            collPlayer = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            collPlayer = false;
        }
    }
}
