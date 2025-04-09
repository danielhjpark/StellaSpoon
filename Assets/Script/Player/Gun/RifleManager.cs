using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.UI;

public class RifleManager : MonoBehaviour
{
    public static RifleManager instance;

    //private GunRecoil recoilScript;

    [Header("Weapons")]
    [SerializeField] private List<WeaponData> weaponLevels; // 무기 리스트
    private int currentWeaponIndex = 0; // 현재 무기 인덱스

    private WeaponData CurrentWeapon => weaponLevels[currentWeaponIndex];

    [Header("Bullet")]
    [SerializeField] private float maxShootDelay = 0.2f;
    [SerializeField] private float currentShootDelay = 0.2f;

    public GameObject WeaponUI;
    public GameObject SpriteUI;

    [SerializeField] public Text bulletText;
    [SerializeField] private Text maxBulletText;
    private int maxBullet = 10;
    public int currentBullet = 0;

    [Header("Weapon FX")]
    [SerializeField] private GameObject weaponFlashFX;
    [SerializeField] private GameObject bulletCaseFX;
    [SerializeField] private GameObject weaponClipFX;

    [Header("Weapon State")]
    public int attackDamage = 20;
    public int CurrentWeaponLevel => currentWeaponIndex;

    void Start()
    {
        instance = this;
        currentShootDelay = 0;
        InitBullet();
        //recoilScript = GetComponent<GunRecoil>();
        SwitchWeapon(0); // 기본 무기 선택
    }

    void Update()
    {
        bulletText.text = currentBullet.ToString();
        maxBulletText.text = "/ " + maxBullet;

        if (currentShootDelay < maxShootDelay)
        {
            currentShootDelay += Time.deltaTime;
        }
    }

    public void Shooting(Vector3 targetPosition)
    {
        if (currentShootDelay < maxShootDelay || currentBullet <= 0) return;

        currentShootDelay = 0;
        currentBullet--;

        var aim = (targetPosition - CurrentWeapon.bulletPoint.position).normalized;

        var flashFX = BulletPoolManager.instance.ActivateObj(1);
        SetObjPosition(flashFX, CurrentWeapon.bulletPoint);
        flashFX.transform.rotation = Quaternion.LookRotation(aim, Vector3.up);

        var caseFX = BulletPoolManager.instance.ActivateObj(2);
        SetObjPosition(caseFX, CurrentWeapon.bulletCasePoint);

        var bulletObj = BulletPoolManager.instance.ActivateObj(0);
        SetObjPosition(bulletObj, CurrentWeapon.bulletPoint);
        bulletObj.transform.rotation = Quaternion.LookRotation(aim, Vector3.up);

        var bullet = bulletObj.GetComponent<BulletManager>();
        if (bullet != null)
        {
            bullet.SetDamage(attackDamage);
        }

        //recoilScript.ApplyRecoil();
    }

    public void ReloadClip()
    {
        var clipFX = BulletPoolManager.instance.ActivateObj(3);
        SetObjPosition(clipFX, CurrentWeapon.weaponClipPoint);
    }

    public void InitBullet()
    {
        currentBullet = maxBullet;
    }

    private void SetObjPosition(GameObject obj, Transform targetTransform)
    {
        obj.transform.position = targetTransform.position;
    }

    public void SwitchWeapon(int levelIndex)
    {
        // 무기 전환
        for (int i = 0; i < weaponLevels.Count; i++)
        {
            weaponLevels[i].weaponObject.SetActive(i == levelIndex);
        }
        currentWeaponIndex = levelIndex;
        InitBullet(); // 무기 교체 시 총알 초기화
    }
}
