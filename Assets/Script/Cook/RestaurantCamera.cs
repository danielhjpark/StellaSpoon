using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class RestaurantCamera : MonoBehaviour
{
    private CinemachineVirtualCamera vCam; // 연결된 가상 카메라
    public Vector3 startPos;
    public Quaternion startRot;
    public float startFOV;
    public static RestaurantCamera instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
                if (vCam == null)
        vCam = GameObject.Find("PlayerFollowCamera").GetComponent<CinemachineVirtualCamera>();

        // 처음 상태 저장
        startPos = vCam.transform.position;
        startRot = vCam.transform.rotation;
        startFOV = vCam.m_Lens.FieldOfView;
    }


    /// <summary>
    /// 카메라를 처음 실행 시 상태로 되돌림
    /// </summary>
    public void ResetCamera()
    {
        vCam.transform.position = startPos;
        vCam.transform.rotation = startRot;
        vCam.m_Lens.FieldOfView = startFOV;
    }
}
