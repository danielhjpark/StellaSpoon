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


    //[Header("IK")]
    //[SerializeField]
    //private Rig aimRig;

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
            //anim.SetLayerWeight(1, 1);
            //anim.SetTrigger("Reload");
            controller.isReload = true;
        }

        if (controller.isReload)
        {
            return;
        }

        if (_input.aiming)
        {
            AimControll(true);

            //anim.SetLayerWeight(1, 1);

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
                //anim.SetBool("Shot", true);
            }
            else
            {
                //anim.SetBool("Shot", false);
            }
        }
        else
        {
            AimControll(false);
            SetRigWeight(0);
            //anim.SetLayerWeight(1, 0);
            //anim.SetBool("Shot", false);
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
        //anim.SetLayerWeight(1, 0);
    }

    private void SetRigWeight(float _weight)
    {
        //aimRig.weight = _weight;
    }
}
