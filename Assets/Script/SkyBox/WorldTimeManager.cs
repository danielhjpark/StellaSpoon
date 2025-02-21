using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTimeManager : MonoBehaviour
{
    public GameTimeManager gameTimeManager;
    public Material[] skyboxes;
    int skyIdx = 0;

    [SerializeField]
    int currentHor = 0;

    private void Update()
    {
        if (currentHor != gameTimeManager.gameHours)
        {
            SunUpdate();
        }
    }

    void SunUpdate()
    {
        currentHor = gameTimeManager.gameHours;
        switch (currentHor)
        {
            case 9:
                StartCoroutine(NextSkyBox(skyboxes[0], 0));
                break;

            case 1:
                Debug.Log("ㅋㅋ");
                StartCoroutine(NextSkyBox(skyboxes[1], 1));
                break;

            case 18:

                StartCoroutine(NextSkyBox(skyboxes[2], 2));
                break;

            case 22:
                StartCoroutine(NextSkyBox(skyboxes[3], 3));
                break;
        }
    }
    void SetBlendSkyTexture(Material skybox)
    {
        // 새 텍스처 생성

        string[] texParts = { "_FrontTex", "_BackTex", "_LeftTex", "_RightTex", "_UpTex", "_DownTex" };

        // 각 면의 텍스처를 혼합
        foreach (string texPart in texParts)
        {
            RenderSettings.skybox.SetTexture(texPart, skyboxes[skyIdx].GetTexture(texPart));
            RenderSettings.skybox.SetTexture(texPart + "2", skybox.GetTexture(texPart));
        }

    }

    IEnumerator NextSkyBox(Material skybox, int idx)
    {

        float value = 0.0f;

        // 각 면의 텍스처를 혼합
        SetBlendSkyTexture(skybox);
        while (value < 1)
        {
            yield return null;
            value += Time.deltaTime * 0.1f;
            value = Mathf.Clamp01(value);

            RenderSettings.skybox.SetFloat("_Blend", value); // 혼합 비율 조절해 서서히 스카이 박스가 전환됨.
        }

        skyIdx = idx;
    }
}
