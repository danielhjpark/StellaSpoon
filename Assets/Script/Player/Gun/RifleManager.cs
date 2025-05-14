using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.UI;

public class RifleManager : MonoBehaviour
{
    public static RifleManager instance;

    [Header("Weapons")]
    [SerializeField] private List<WeaponData> weaponLevels;
    private int currentWeaponIndex = 0;
    private WeaponData CurrentWeapon => weaponLevels[currentWeaponIndex];

    [Header("Weapon State")]
    private float currentShootDelay;
    private float maxShootDelay;

    public int currentBullet;
    private int maxBullet;
    public int attackDamage;

    public int CurrentWeaponLevel => currentWeaponIndex;

    [Header("UI")]
    public GameObject WeaponUI;
    public GameObject SpriteUI;
    [SerializeField] private Text bulletText;
    [SerializeField] private Text maxBulletText;

    public bool tempestFang = false;
    public bool infernoLance = false;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        currentShootDelay = 0f;
        SwitchWeapon(0); // 초기 무기 설정
    }

    void Update()
    {
        bulletText.text = currentBullet.ToString();
        maxBulletText.text = "/ " + maxBullet;

        if (currentShootDelay < maxShootDelay)
        {
            currentShootDelay += Time.deltaTime;
        }

        //if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchWeapon(0);
        //else if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchWeapon(1);
        //else if (Input.GetKeyDown(KeyCode.Alpha3)) SwitchWeapon(2);
    }

    public void Shooting(Vector3 targetPosition)
    {
        if (currentShootDelay < maxShootDelay || currentBullet <= 0) return;

        currentShootDelay = 0;
        currentBullet--;

        var aim = (targetPosition - CurrentWeapon.bulletPoint.position).normalized;

        // 무기 레벨
        int weaponLevel = CurrentWeaponLevel;

        // 총구 화염
        var flashFX = BulletPoolManager.instance.GetObject(weaponLevel, "muzzle", CurrentWeapon.muzzleFlashPrefab);
        SetObjPosition(flashFX, CurrentWeapon.bulletPoint);
        flashFX.transform.rotation = Quaternion.LookRotation(aim, Vector3.up);

        // 탄피
        var caseFX = BulletPoolManager.instance.GetObject(weaponLevel, "case", CurrentWeapon.bulletCasePrefab);
        SetObjPosition(caseFX, CurrentWeapon.bulletCasePoint);

        // 총알
        var bulletObj = BulletPoolManager.instance.GetObject(weaponLevel, "bullet", CurrentWeapon.bulletPrefab);
        SetObjPosition(bulletObj, CurrentWeapon.bulletPoint);
        bulletObj.transform.rotation = Quaternion.LookRotation(aim, Vector3.up);

        var bullet = bulletObj.GetComponent<BulletManager>();
        if (bullet != null)
        {
            bullet.SetDamageFromWeapon(CurrentWeapon);
        }

        // 총 반동이 있다면 여기에 recoil 추가 가능
    }

    public void ReloadClip()
    {
        int weaponLevel = CurrentWeaponLevel;
        var clipFX = BulletPoolManager.instance.GetObject(weaponLevel, "clip", CurrentWeapon.weaponClipPrefab);
        SetObjPosition(clipFX, CurrentWeapon.weaponClipPoint);
        InitBullet();
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
        if (levelIndex < 0 || levelIndex >= weaponLevels.Count) return;

        // 무기 잠금 조건 확인
        if (levelIndex == 1 && !tempestFang)
        {
            Debug.Log("아직 해금 안된 tempestFang");
            return; // tempestFang 무기 해금 안 됨
        }
        if (levelIndex == 2 && !infernoLance)
        {
            Debug.Log("아직 해금 안된 infernoLance");
            return; // infernoLance 무기 해금 안 됨
        }

        for (int i = 0; i < weaponLevels.Count; i++)
        {
            weaponLevels[i].weaponObject.SetActive(i == levelIndex);
        }

        currentWeaponIndex = levelIndex;

        // 무기 정보 적용
        attackDamage = CurrentWeapon.damage;
        maxBullet = CurrentWeapon.maxBullet;
        maxShootDelay = CurrentWeapon.fireRate;

        InitBullet();
    }
    public WeaponData GetCurrentWeaponData()
    {
        return CurrentWeapon;
    }
}
