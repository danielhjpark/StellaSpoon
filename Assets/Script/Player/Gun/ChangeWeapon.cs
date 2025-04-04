using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeWeapon : MonoBehaviour
{
    [SerializeField]
    private GameObject weaponChangeUI; // ���� ���� UI

    private bool collPlayer = false; //�÷��̾� �浹üũ

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
