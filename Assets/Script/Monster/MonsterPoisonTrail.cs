using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPoisonTrail : MonoBehaviour
{
    public GameObject poisonPrefab; //틱데미지 독구름 프리팹
    public float spawnInterval = 0.5f; //독구름 생성 간격
    public float poisonDuration = 5f; //독구름 지속 시간
    public float fadeDuration = 2f; //페이드 아웃 지속 시간

    private EscapeMonster escapeMonster;


    private List<GameObject> poisonClouds = new List<GameObject>(); //생성된 독구름 목록

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
                if(escapeMonster.isDead) //몬스터가 죽었으면 독구름 삭제후 코루틴 종료
                {
                    ClearPoisonClouds();
                    yield break; // 코루틴 즉시 종료
                }
                if(escapeMonster.isEscaping) //몬스터가 도망가는 중이면 독구름 생성
                {
                    GameObject poison = Instantiate(poisonPrefab, transform.position, Quaternion.identity);
                    poisonClouds.Add(poison);

                    StartCoroutine(FadeAndDestroyPoison(poison, poisonDuration, fadeDuration));
                }
                yield return new WaitForSeconds(spawnInterval);
            }
            else
            {
                yield return null; //무한 루프 방지
            }
        }
    }
    void ClearPoisonClouds()
    {
        foreach (GameObject poison in poisonClouds)
        {
            if (poison != null)
            {
                StartCoroutine(FadeAndDestroyPoison(poison, 0f, fadeDuration)); // 즉시 페이드 아웃 시작
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
