using Cinemachine;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

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

    void Start()
    {
        _input = GetComponent<StarterAssetsInputs>();
        controller = GetComponent<ThirdPersonController>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        AimCheck();
        EquipRifleCheck();
        CheckJumpOrDodge();
    }

    private void AimCheck()
    {
        if(_input.reload)
        {
            _input.reload = false;

            if(controller.isReload)
            {
                return;
            }

            AimControll(false);
            SetRigWeight(0);
            anim.SetLayerWeight(2, 1);
            anim.SetTrigger("Reload");
            controller.isReload = true;
        }

        if (controller.isReload)
        {
            return;
        }

        if (_input.aiming && InventoryManager.instance.isWeaponRifle == true)
        {
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

    // 총을 들고 있는 변수가 true면 Rifle을 보이게 하고 들고 있게 하는 애니메이션 LayerWeight를 증가
    private void EquipRifleCheck()
    {
        if (InventoryManager.instance.isWeaponRifle == true)
        {
            rifle.gameObject.SetActive(true);
            anim.SetLayerWeight(1, 1);
            handRig.weight = 1;
            RifleManager.instance.bulletText.gameObject.SetActive(true);
        }
        else
        {
            rifle.gameObject.SetActive(false);
            anim.SetLayerWeight(1, 0);
            handRig.weight = 0;
            RifleManager.instance.bulletText.gameObject.SetActive(false);
        }
    }

    private void SetRigWeight(float weight)
    {
        aimRig.weight = weight;
        handRig.weight = weight;
    }

    private void CheckJumpOrDodge()
    {
        // 애니메이터에서 점프 또는 닷지 애니메이션이 실행 중인지 확인
        bool isJumping = anim.GetCurrentAnimatorStateInfo(1).IsTag("Jump");
        bool isDodging = anim.GetCurrentAnimatorStateInfo(1).IsTag("Dodge");
        bool isReloading = anim.GetCurrentAnimatorStateInfo(2).IsTag("Reload");

        if (isJumping || isDodging || isReloading)
        {
            SetRigWeight(0); // 점프/닷지 중에는 IK 비활성화
        }
    }

    public void ReloadWeaponClip()
    {
        RifleManager.instance.ReloadClip();
    }
}
