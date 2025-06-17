using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPoisonTrail : MonoBehaviour
{
    public GameObject poisonPrefab; //ƽ������ ������ ������
    public float spawnInterval = 0.5f; //������ ���� ����
    public float poisonDuration = 5f; //������ ���� �ð�
    public float fadeDuration = 2f; //���̵� �ƿ� ���� �ð�

    private EscapeMonster escapeMonster;


    private List<GameObject> poisonClouds = new List<GameObject>(); //������ ������ ���

    private void Start()
    {
        escapeMonster = GetComponent<EscapeMonster>();
        StartCoroutine(SpawnPoisonTrail());
    }
    IEnumerator SpawnPoisonTrail()
    {
        while (true)
        {
            if(escapeMonster != null)
            {
                if(escapeMonster.isDead) //���Ͱ� �׾����� ������ ������ �ڷ�ƾ ����
                {
                    ClearPoisonClouds();
                    yield break; // �ڷ�ƾ ��� ����
                }
                if(escapeMonster.isEscaping) //���Ͱ� �������� ���̸� ������ ����
                {
                    GameObject poison = Instantiate(poisonPrefab, transform.position, Quaternion.identity);
                    poisonClouds.Add(poison);

                    StartCoroutine(FadeAndDestroyPoison(poison, poisonDuration, fadeDuration));
                }
                yield return new WaitForSeconds(spawnInterval);
            }
            else
            {
                yield return null; //���� ���� ����
            }
        }
    }
    void ClearPoisonClouds()
    {
        foreach (GameObject poison in poisonClouds)
        {
            if (poison != null)
            {
                StartCoroutine(FadeAndDestroyPoison(poison, 0f, fadeDuration)); // ��� ���̵� �ƿ� ����
            }
        }
        poisonClouds.Clear();
    }

    IEnumerator FadeAndDestroyPoison(GameObject poison, float delay, float fadeTime)
    {
        yield return new WaitForSeconds(delay);
        float elapsedTime = 0f;
        if(poison != null)
        {
            Renderer poisonRenderer = poison.GetComponent<Renderer>();
            if (poisonRenderer != null)
            {
                Material material = poisonRenderer.material;
                Color startColor = material.color;
                Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

                while (elapsedTime < fadeTime)
                {
                    elapsedTime += Time.deltaTime;
                    material.color = Color.Lerp(startColor, targetColor, elapsedTime / fadeTime);
                    yield return null;
                }
            }

            poisonClouds.Remove(poison);
            Destroy(poison);
        }
    }
}
