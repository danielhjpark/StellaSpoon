using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.UI;

public class RifleManager : MonoBehaviour
{
    public static RifleManager instance;

    private GunRecoil recoilScript;

    [Header("Bullet")]
    [SerializeField]
    private Transform bulletPoint;
    [SerializeField]
    private GameObject bulletObj;
    [SerializeField]
    private float maxShootDelay = 0.2f;
    [SerializeField]
    private float currentShootDelay = 0.2f;

    public GameObject WeaponUI;

    [SerializeField]
    public Text bulletText;
    [SerializeField]
    private Text maxBulletText;
    private int maxBullet = 10;
    public int currentBullet = 0;

    [Header("Weapon FX")]
    [SerializeField]
    private GameObject weaponFlashFX;
    [SerializeField]
    private Transform bulletCasePoint;
    [SerializeField]
    private GameObject bulletCaseFX;
    [SerializeField]
    private Transform weaponClipPoint;
    [SerializeField]
    private GameObject weaponClipFX;

    // RifleDamage
    [Header("Weapon State")]
    public int attackDamage = 20;

    void Start()
    {
        instance = this;

        currentShootDelay = 0;

        InitBullet();

        recoilScript = GetComponent<GunRecoil>();
    }

    // Update is called once per frame
    void Update()
    {
        bulletText.text = currentBullet + "";
        maxBulletText.text = "/" + maxBullet;
        if (currentShootDelay < maxShootDelay)
        {
            currentShootDelay += Time.deltaTime;
        }
    }

    public void Shooting(Vector3 targetPosition)
    {
        currentShootDelay += Time.deltaTime;

        if (currentShootDelay < maxShootDelay) return;

        if (currentBullet <= 0) return;

        currentBullet -= 1;
        currentShootDelay = 0;

        Instantiate(weaponFlashFX, bulletPoint);
        Instantiate(bulletCaseFX, bulletCasePoint);

        Vector3 aim = (targetPosition - bulletPoint.position).normalized;
        GameObject newBullet = Instantiate(bulletObj, bulletPoint.position, Quaternion.LookRotation(aim, Vector3.up));

        BulletManager bullet = newBullet.GetComponent<BulletManager>();
        if (bullet != null)
        {
            bullet.SetDamage(attackDamage); // RifleManager의 공격력 전달
        }
        // 반동 적용
        recoilScript.ApplyRecoil();
    }

    public void ReloadClip()
    {
        Instantiate(weaponClipFX, weaponClipPoint);
        InitBullet();
    }

    private void InitBullet()
    {
        currentBullet = maxBullet;
    }
}
