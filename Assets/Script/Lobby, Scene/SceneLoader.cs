using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public enum SceneNames { Lobby = 0, Playground }

namespace UnityNote
{
    public class SceneLoader : MonoBehaviour
    {
        public static SceneLoader Instance {  get; private set; }

        [SerializeField]
        private GameObject loadingScreen; // 로딩 화면
        [SerializeField]
        private Image loadingBackground; // 로딩 화면에 출력되는 배경 이미지
        [SerializeField]
        private TextMeshProUGUI[] tipText; // 출력될 팁 목록
        [SerializeField]
        private Slider loadingProgress; // 로딩 진행도
        [SerializeField]
        private TextMeshProUGUI textProgress; // 로딩 진행도 텍스트

        private WaitForSeconds waitChangeDelay; // 씬 변경 지연 시간

        private void Awake()
        {
            if(Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                waitChangeDelay = new WaitForSeconds(0.5f);

                DontDestroyOnLoad(gameObject);
            }
        }

        public void LoadScene(string name)
        {
            // 모든 팁을 비활성화
            foreach (var text in tipText)
            {
                text.gameObject.SetActive(false);
            }

            int index = Random.Range(0, tipText.Length);
            tipText[index].gameObject.SetActive(true);
            loadingProgress.value = 0f;
            loadingScreen.SetActive(true);

            StartCoroutine(LoadSceneAsync(name));
        }

        public void LoadScene(SceneNames name)
        {
            LoadScene(name.ToString());
        }

        private IEnumerator LoadSceneAsync(string name)
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(name);
            asyncOperation.allowSceneActivation = false; // 씬 자동 전환 방지

            // 비동기 작업(씬 불러오기)이 완료될 때까지 반복
            /*
            while(asyncOperation.isDone == false)
            {
                // 비동기 작업의 진행 상황 (0.0 ~ 1.0)
                loadingProgress.value = asyncOperation.progress;
                textProgress.text = $"{Mathf.RoundToInt(asyncOperation.progress * 100)}%";

                yield return null;
            }
            */

            // 너무 빠르기 때문에 임의의 시간만큼 로딩을 지속하도록 수정
            float percent = 0f;
            float loadingTime = 2.5f;

            while(percent < 1f)
            {
                percent += Time.deltaTime / loadingTime;
                loadingProgress.value = percent;
                textProgress.text = $"{Mathf.RoundToInt(percent * 100)}%";

                yield return null;
            }

            yield return waitChangeDelay;
            asyncOperation.allowSceneActivation = true;

            loadingScreen.SetActive(false);
        }
    }
}
