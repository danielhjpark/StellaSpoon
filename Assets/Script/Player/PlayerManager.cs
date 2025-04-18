using Cinemachine;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.SceneManagement; // 씬 변경 감지를 위해 추가

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
    private GameObject rifle; // 보여질 총

    [Header("IK")]
    [SerializeField]
    public Rig handRig;
    [SerializeField]
    private Rig aimRig;

    private bool noAim = true;

    // ** 새로운 아바타 및 애니메이터 관련 변수 추가 **
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
    private SkinnedMeshRenderer skinnedMeshRenderer; // 플레이어의 SkinnedMeshRenderer
    [SerializeField]
    private Mesh normalMesh; // 기본 외형
    [SerializeField]
    private Mesh specialMesh; // 바뀔 외형
    [SerializeField]
    private Material normalMaterial;
    [SerializeField]
    private Material specialMaterial;


    [SerializeField]
    private AudioClip Reload_SFX; // 재장전 소리

    private string specialSceneName = "NPCTest"; // 아바타가 변경될 씬 이름
    public bool isRestaurant;

    void Start()
    {
        _input = GetComponent<StarterAssetsInputs>();
        controller = GetComponent<ThirdPersonController>();
        anim = GetComponent<Animator>();

        SceneManager.sceneLoaded += OnSceneLoaded; // 씬 변경 이벤트 등록

        // 씬이 이미 로드된 뒤 생성된 경우를 위해 수동 호출
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // 이벤트 해제
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string[] restaurantScenes = {"RestaurantTest","WokMergeTest", "FryingPanMergeTest", "CuttingBoardMergeTest", "PotMergeTest"}; 
        foreach(string restaurantScene in restaurantScenes) {
            if(scene.name == restaurantScene) {
                isRestaurant = true;
                break;
            }
            else isRestaurant = false;
        }
        
        if (isRestaurant)
        {
            isRestaurant = true;
            SetRigWeight(0);
            ChangeAvatar(specialAvatar, specialMesh, specialAnimator, specialMaterial); // 특정 씬일 경우 변경

        }
        else
        {
            isRestaurant = false;
            ChangeAvatar(defaultAvatar, normalMesh, defaultAnimator, normalMaterial); // 기본 씬이면 원래대로
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
    }

    void Update()
    {
        if (isRestaurant)
        {
            InventoryManager.instance.isWeaponRifle = false;
            EquipRifleCheck();
            return;
        }
        else // 나중에 무기를 변경할 때 조건을 추가해서 넣기. 안그러면 false를 한번 해도 계속 true로 바뀜
        {
            InventoryManager.instance.isWeaponRifle = true;
            EquipRifleCheck();
        }

        CheckJumpOrDodge();
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
            // 재장전 소리
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

        if (InventoryManager.instance.isWeaponRifle == true)
        {
            rifle.gameObject.SetActive(true);
            anim.SetLayerWeight(1, 1);
            handRig.weight = 1;
            RifleManager.instance.WeaponUI.SetActive(true);
            RifleManager.instance.SpriteUI.SetActive(true);
        }
        else
        {
            rifle.gameObject.SetActive(false);
            if(!isRestaurant) anim.SetLayerWeight(1, 0);
            handRig.weight = 0;
            RifleManager.instance.WeaponUI.SetActive(false);
            RifleManager.instance.SpriteUI.SetActive (false);
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
            SetRigWeight(0);
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
}
