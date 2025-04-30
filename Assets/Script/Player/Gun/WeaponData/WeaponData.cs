using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponData
{
    public GameObject weaponObject;
    public Transform bulletPoint;
    public Transform bulletCasePoint;
    public Transform weaponClipPoint;

    public GameObject bulletPrefab;
    public GameObject muzzleFlashPrefab;
    public GameObject bulletCasePrefab;
    public GameObject weaponClipPrefab;

    [Header("Weapon Stats")]
    public int damage = 20;
    public int weight = 5;
    public int maxBullet = 10;
    public float fireRate = 0.2f; // 낮을수록 연사력이 빨라진다.
}

