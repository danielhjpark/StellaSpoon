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
        SwitchWeapon(0); // �ʱ� ���� ����
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

        // ���� ����
        int weaponLevel = CurrentWeaponLevel;

        // �ѱ� ȭ��
        var flashFX = BulletPoolManager.instance.GetObject(weaponLevel, "muzzle", CurrentWeapon.muzzleFlashPrefab);
        SetObjPosition(flashFX, CurrentWeapon.bulletPoint);
        flashFX.transform.rotation = Quaternion.LookRotation(aim, Vector3.up);

        // ź��
        var caseFX = BulletPoolManager.instance.GetObject(weaponLevel, "case", CurrentWeapon.bulletCasePrefab);
        SetObjPosition(caseFX, CurrentWeapon.bulletCasePoint);

        // �Ѿ�
        var bulletObj = BulletPoolManager.instance.GetObject(weaponLevel, "bullet", CurrentWeapon.bulletPrefab);
        SetObjPosition(bulletObj, CurrentWeapon.bulletPoint);
        bulletObj.transform.rotation = Quaternion.LookRotation(aim, Vector3.up);

        var bullet = bulletObj.GetComponent<BulletManager>();
        if (bullet != null)
        {
            bullet.SetDamageFromWeapon(CurrentWeapon);
        }

        // �� �ݵ��� �ִٸ� ���⿡ recoil �߰� ����
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

        // ���� ��� ���� Ȯ��
        if (levelIndex == 1 && !tempestFang)
        {
            Debug.Log("���� �ر� �ȵ� tempestFang");
            return; // tempestFang ���� �ر� �� ��
        }
        if (levelIndex == 2 && !infernoLance)
        {
            Debug.Log("���� �ر� �ȵ� infernoLance");
            return; // infernoLance ���� �ر� �� ��
        }

        for (int i = 0; i < weaponLevels.Count; i++)
        {
            weaponLevels[i].weaponObject.SetActive(i == levelIndex);
        }

        currentWeaponIndex = levelIndex;

        // ���� ���� ����
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
