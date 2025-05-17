using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class WolfKingFadeParticle : MonoBehaviour
{
    private ParticleSystem ps;
    private ParticleSystem.Particle[] particles;

    public float fadeInTime = 1f;
    public float fadeOutStart = 4f;
    public float fadeOutTime = 1f;

    // ������ ���� ����
    public float damageAmount = 10f; // ������ ��

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        particles = new ParticleSystem.Particle[ps.main.maxParticles];
    }

    void Update()
    {
        int count = ps.GetParticles(particles);

        for (int i = 0; i < count; i++)
        {
            float lifetime = particles[i].startLifetime;
            float age = lifetime - particles[i].remainingLifetime;

            float alpha = 1f;
            // ���̵� ��
            if (age < fadeInTime)
            {
                alpha = age / fadeInTime;
            }
            // ����
            else if (age < fadeOutStart)
            {
                alpha = 1f;
            }
            // ���̵� �ƿ�
            else if (age < fadeOutStart + fadeOutTime)
            {
                alpha = 1f - ((age - fadeOutStart) / fadeOutTime);
            }
            else
            {
                alpha = 0f;
            }

            Color32 c = particles[i].startColor;
            c.a = (byte)(alpha * 255);
            particles[i].startColor = c;

            Debug.Log($"Particle {i} Alpha: {alpha}");
        }

        ps.SetParticles(particles, count);
    }
}
