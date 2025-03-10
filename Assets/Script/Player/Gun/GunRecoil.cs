using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GunRecoil : MonoBehaviour
{
    public CinemachineVirtualCamera cinemachineCamera;
    public float baseRecoilIntensity = 1f; // 기본 반동 강도
    public float maxRecoilIntensity = 5f; // 최대 반동 강도
    public float recoilIncreaseRate = 0.5f; // 연사 시 반동 증가율
    public float recoilRecoveryRate = 2f; // 반동 복구 속도

    private CinemachineBasicMultiChannelPerlin noise;
    private float currentRecoil = 0f;
    private float lastShotTime = 0f;

    void Start()
    {
        if (cinemachineCamera != null)
        {
            noise = cinemachineCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }
    }

    void Update()
    {
        // 시간이 지나면 반동 서서히 감소
        if (noise != null && Time.time - lastShotTime > 0.1f) // 연사 멈추면 0.1초 후 서서히 감소
        {
            currentRecoil = Mathf.Lerp(currentRecoil, 0f, Time.deltaTime * recoilRecoveryRate);
            noise.m_AmplitudeGain = currentRecoil;
        }
    }

    public void ApplyRecoil()
    {
        if (noise != null)
        {
            // 연사할수록 반동 증가 (최대 반동 제한)
            currentRecoil += recoilIncreaseRate;
            currentRecoil = Mathf.Clamp(currentRecoil, baseRecoilIntensity, maxRecoilIntensity);

            noise.m_AmplitudeGain = currentRecoil;
            lastShotTime = Time.time; // 마지막 발사 시간 저장
        }
    }
}