using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AutoDisableOnParticleEnd : MonoBehaviour
{
    private ParticleSystem ps;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        if (ps == null) ps = GetComponent<ParticleSystem>();
        StartCoroutine(WaitAndDisable());
    }

    private IEnumerator WaitAndDisable()
    {
        yield return new WaitUntil(() => !ps.IsAlive(true));
        gameObject.SetActive(false);
    }
}

