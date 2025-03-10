using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GunRecoil : MonoBehaviour
{
    public CinemachineVirtualCamera cinemachineCamera;
    public float baseRecoilIntensity = 1f; // �⺻ �ݵ� ����
    public float maxRecoilIntensity = 5f; // �ִ� �ݵ� ����
    public float recoilIncreaseRate = 0.5f; // ���� �� �ݵ� ������
    public float recoilRecoveryRate = 2f; // �ݵ� ���� �ӵ�

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
        // �ð��� ������ �ݵ� ������ ����
        if (noise != null && Time.time - lastShotTime > 0.1f) // ���� ���߸� 0.1�� �� ������ ����
        {
            currentRecoil = Mathf.Lerp(currentRecoil, 0f, Time.deltaTime * recoilRecoveryRate);
            noise.m_AmplitudeGain = currentRecoil;
        }
    }

    public void ApplyRecoil()
    {
        if (noise != null)
        {
            // �����Ҽ��� �ݵ� ���� (�ִ� �ݵ� ����)
            currentRecoil += recoilIncreaseRate;
            currentRecoil = Mathf.Clamp(currentRecoil, baseRecoilIntensity, maxRecoilIntensity);

            noise.m_AmplitudeGain = currentRecoil;
            lastShotTime = Time.time; // ������ �߻� �ð� ����
        }
    }
}