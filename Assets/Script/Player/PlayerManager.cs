using Cinemachine;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.SceneManagement; // �� ���� ������ ���� �߰�

public class PlayerManager : MonoBehaviour
{
    private StarterAssetsInputs _input;
    private ThirdPersonController controller;
    private Animator anim;

    [Header("Aim")]
    [SerializeField]
    private CinemachineVirtualCamera aimCam;

    [SerializeField]
    private GameObject aimImage;

    [SerializeField]
    private LayerMask targetLayer;

    [SerializeField]
    private GameObject aimObject;

    [SerializeField]
    private float aimObjDis = 10f;

    [SerializeField]
    private GameObject[] rifleObjects;

    [Header("IK")]
    [SerializeField]
    public Rig handRig;
    [SerializeField]
    private Rig aimRig;

    private bool noAim = true;

    // ** ���ο� �ƹ�Ÿ �� �ִϸ����� ���� ���� �߰� **
    [Header("Avatar & Animator")]
    [SerializeField]
    private Avatar defaultAvatar;
    [SerializeField] 
    private Avatar specialAvatar;
    [SerializeField]
    private RuntimeAnimatorController defaultAnimator;
    [SerializeField]
    private RuntimeAnimatorController specialAnimator;
    [SerializeField]
    private SkinnedMeshRenderer skinnedMeshRenderer; // �÷��̾��� SkinnedMeshRenderer
    [SerializeField]
    private Mesh normalMesh; // �⺻ ����
    [SerializeField]
    private Mesh specialMesh; // �ٲ� ����
    [SerializeField]
    private Material normalMaterial;
    [SerializeField]
    private Material specialMaterial;


    [SerializeField]
    private AudioClip Reload_SFX; // ������ �Ҹ�

    public bool isRestaurant;

    private bool rigTemporarilyDisabled = false;
    [SerializeField] private TwoBoneIKConstraint leftHandConstraint; // �޼� IK

    void Start()
    {
        _input = GetComponent<StarterAssetsInputs>();
        controller = GetComponent<ThirdPersonController>();
        anim = GetComponent<Animator>();
        RifleManager.instance.SwitchWeapon(0);
        SceneManager.sceneLoaded += OnSceneLoaded; // �� ���� �̺�Ʈ ���

        // ���� �̹� �ε�� �� ������ ��츦 ���� ���� ȣ��
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // �̺�Ʈ ����
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string[] restaurantScenes = { "Restaurant", "RestaurantTest2", "WokMergeTest", "FryingPanMergeTest", "CuttingBoardMergeTest", "PotMergeTest", "Store", "Shop" };
        foreach (string restaurantScene in restaurantScenes)
        {
            if (scene.name == restaurantScene)
            {
                isRestaurant = true;
                break;
            }
            else isRestaurant = false;
        }

        if (isRestaurant)
        {
            isRestaurant = true;
            SetRigWeight(0);
            ChangeAvatar(specialAvatar, specialMesh, specialAnimator, specialMaterial); // Ư�� ���� ��� ����
        }
        else
        {
            isRestaurant = false;
            ChangeAvatar(defaultAvatar, normalMesh, defaultAnimator, normalMaterial); // �⺻ ���̸� �������
        }
    }

    private void ChangeAvatar(Avatar avatar, Mesh mesh, RuntimeAnimatorController newAnimator, Material material)
    {
        anim.avatar = avatar;
        skinnedMeshRenderer.sharedMesh = mesh;
        skinnedMeshRenderer.material = material;
        anim.runtimeAnimatorController = newAnimator;
        anim.Rebind();
        anim.Update(0);

        if (!isRestaurant) handRig.weight = 1;
    }

    void Update()
    {
        if (isRestaurant)
        {
            InventoryManager.instance.isWeaponRifle = false;
            EquipRifleCheck();
            return;
        }
        else // ���߿� ���⸦ ������ �� ������ �߰��ؼ� �ֱ�. �ȱ׷��� false�� �ѹ� �ص� ��� true�� �ٲ�
        {
            InventoryManager.instance.isWeaponRifle = true;
            EquipRifleCheck();
        }

        CheckJumpOrDodge();
        if(controller.isDie)
        {
            return;
        }
        AimCheck();
    }

    private void AimCheck()
    {
        if (_input.reload)
        {
            _input.reload = false;

            if (controller.isReload || InventoryManager.instance.isWeaponRifle == false || anim.GetCurrentAnimatorStateInfo(2).IsTag("Reload"))
            {
                return;
            }

            AimControll(false);
            SetRigWeight(0);

            anim.SetLayerWeight(2, 1);
            anim.SetTrigger("Reload");
            // ������ �Ҹ�
            AudioSource.PlayClipAtPoint(Reload_SFX, transform.position, controller.FootstepAudioVolume);
            controller.isReload = true;
        }

        if (controller.isReload)
        {
            return;
        }

        if (_input.aiming && InventoryManager.instance.isWeaponRifle == true)
        {
            if (!noAim)
            {
                return;
            }

            AimControll(true);

            anim.SetLayerWeight(2, 1);

            Vector3 targetPosition = Vector3.zero;
            Transform camTransform = Camera.main.transform;
            RaycastHit hit;

            if (Physics.Raycast(camTransform.position, camTransform.forward, out hit, Mathf.Infinity, targetLayer))
            {
                targetPosition = hit.point;
                aimObject.transform.position = hit.point;
            }
            else
            {
                targetPosition = camTransform.position + camTransform.forward * aimObjDis;
                aimObject.transform.position = camTransform.position + camTransform.forward * aimObjDis;
            }
            Vector3 targetAim = targetPosition;
            targetAim.y = transform.position.y;
            Vector3 aimDir = (targetAim - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * 50f);

            SetRigWeight(1);

            if (_input.shoot)
            {
                if (RifleManager.instance.currentBullet <= 0)
                {
                    _input.reload = true;
                }
                anim.SetBool("Shot", true);
                RifleManager.instance.Shooting(targetPosition);
            }
            else
            {
                anim.SetBool("Shot", false);
            }
        }
        else
        {
            AimControll(false);
            SetRigWeight(0);
            anim.SetLayerWeight(2, 0);
            anim.SetBool("Shot", false);
        }
    }

    private void AimControll(bool isCheck)
    {
        aimCam.gameObject.SetActive(isCheck);
        aimImage.SetActive(isCheck);
        aimObject.SetActive(isCheck);
        controller.isAiming = isCheck;
    }

    public void Reload()
    {
        controller.isReload = false;
        SetRigWeight(1);
        anim.SetLayerWeight(2, 0);
    }

    private void EquipRifleCheck()
    {
        if (controller.isDie) return;

        bool hasWeapon = InventoryManager.instance.isWeaponRifle;

        // ��� ��Ȱ��ȭ (�ʱ�ȭ)
        foreach (var rifle in rifleObjects)
        {
            rifle.SetActive(false);
        }

        if (hasWeapon)
        {
            int level = RifleManager.instance.CurrentWeaponLevel; // currentWeaponIndex ���

            // ���� ����
            if (level >= 0 && level < rifleObjects.Length)
            {
                rifleObjects[level].SetActive(true); // �ش� ������ �Ѹ� ���̰�
            }

            anim.SetLayerWeight(1, 1);
            StartCoroutine(EnableRigDelayed());
            RifleManager.instance.WeaponUI.SetActive(true);
            RifleManager.instance.SpriteUI.SetActive(true);
        }
        else
        {
            if (!isRestaurant) anim.SetLayerWeight(1, 0);
            handRig.weight = 0;
            if (isRestaurant)
            {
                if (RifleManager.instance.WeaponUI)
                    RifleManager.instance.WeaponUI.SetActive(false);
                if (RifleManager.instance.SpriteUI)
                    RifleManager.instance.SpriteUI.SetActive(false);
            }
        }
    }


    private void SetRigWeight(float weight)
    {
        aimRig.weight = weight;
        handRig.weight = weight;
    }

    private void CheckJumpOrDodge()
    {
        bool isJumping = anim.GetCurrentAnimatorStateInfo(1).IsTag("Jump");
        bool isDodging = anim.GetCurrentAnimatorStateInfo(1).IsTag("Dodge");
        bool isReloading = anim.GetCurrentAnimatorStateInfo(2).IsTag("Reload");
        bool isHitting = anim.GetCurrentAnimatorStateInfo(1).IsTag("Hit");

        if (isJumping || isDodging || isReloading || isHitting)
        {
            if (!rigTemporarilyDisabled)
            {
                DisableHandIK();
                rigTemporarilyDisabled = true;
            }
        }
        else
        {
            if (rigTemporarilyDisabled && InventoryManager.instance.isWeaponRifle && !isRestaurant)
            {
                EnableHandIK();
                rigTemporarilyDisabled = false;
            }
        }
    }

    public void ReloadClip()
    {
        if (controller.isHit) return;

        RifleManager.instance.ReloadClip();
    }

    public void BulletInit()
    {
        if (controller.isHit) return;

        RifleManager.instance.InitBullet();
    }

    public void TrueAim()
    {
        noAim = true;
    }

    public void FalseAim()
    {
        noAim = false;
    }

    IEnumerator EnableRigDelayed()
    {
        yield return null; // �� ������ ����
        handRig.weight = 1;
        //Debug.Log("������ �� handRig.weight: " + handRig.weight);
    }
    private void DisableHandIK()
    {
        leftHandConstraint.weight = 0;
    }

    private void EnableHandIK()
    {
        leftHandConstraint.weight = 1;
    }
}
