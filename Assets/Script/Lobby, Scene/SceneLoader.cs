using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;

public enum SceneNames { Lobby = 0, Playground, Restaurant }

namespace UnityNote
{
    public class SceneLoader : MonoBehaviour
    {
        public static SceneLoader Instance { get; private set; }
        public static SaveNLoad instance { get; private set; }

        [SerializeField]
        private GameObject loadingScreen;
        [SerializeField]
        private Image loadingBackground;
        [SerializeField]
        private TextMeshProUGUI[] tipText;
        [SerializeField]
        private Slider loadingProgress;
        [SerializeField]
        private TextMeshProUGUI textProgress;

        private WaitForSeconds waitChangeDelay;

        private SaveNLoad theSaveNLoad; // 사용
        private bool loadSaveFile = false;


        private void Awake()
        {

            if (Instance != null && Instance != this && instance != null && instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                waitChangeDelay = new WaitForSeconds(0.5f);

                DontDestroyOnLoad(gameObject);
                SceneManager.sceneLoaded += OnSceneLoaded;
            }
        }

        private void OnDestroy()
        {
            // 씬 로드 후 연결된 이벤트를 해제
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Log($"씬 로딩됨: {scene.name}");

            // 로딩 화면 종료
            if (loadingScreen != null)
                loadingScreen.SetActive(false);

            StartCoroutine(DelayedAssignButtonEvents());
        }

        private IEnumerator DelayedAssignButtonEvents()
        {
            yield return new WaitForSeconds(0.1f); // UI 로드될 시간을 기다림
            AssignButtonEvents();
        }

        private void AssignButtonEvents()
        {
            // 1. FindWithTag() 사용 (활성화된 오브젝트 찾기)
            GameObject buttonObject = GameObject.FindWithTag("YesButton");

            if (buttonObject == null)
            {
                // 2. FindObjectsOfTypeAll() 사용 (비활성화된 오브젝트도 찾기)
                Button[] allButtons = Resources.FindObjectsOfTypeAll<Button>();
                Button titleButton = allButtons.FirstOrDefault(b => b.name == "YesButton");

                if (titleButton != null)
                {
                    buttonObject = titleButton.gameObject;
                }
            }

            if (buttonObject != null)
            {
                Button titleButton = buttonObject.GetComponent<Button>();

                // 버튼 활성화
                if (!titleButton.gameObject.activeSelf)
                {
                    titleButton.gameObject.SetActive(true);
                    Debug.Log("YesButton이 비활성화되어 있었으므로 활성화 시킴.");
                }

                // 기존 이벤트 제거 후 다시 연결
                titleButton.onClick.RemoveAllListeners();
                titleButton.onClick.AddListener(() => LoadScene(SceneNames.Lobby));
                Debug.Log("YesButton을 찾음, 이벤트 추가 완료.");
            }
            else
            {
                Debug.LogWarning("YesButton을 찾을 수 없음. 태그 설정 또는 비활성화 상태 확인 필요.");
            }
        }

        public void LoadScene(string name)
        {
            foreach (var text in tipText)
            {
                text.gameObject.SetActive(false);  // 모든 팁 비활성화
            }

            int index = Random.Range(0, tipText.Length);  // 팁 중 하나를 랜덤으로 활성화
            tipText[index].gameObject.SetActive(true);

            loadingProgress.value = 0f;
            loadingScreen.SetActive(true);  // 로딩 화면 활성화

            StartCoroutine(LoadSceneAsync(name));
        }

        public void LoadScene(SceneNames name)
        {
            LoadScene(name.ToString());  // 열고자 하는 씬의 이름을 받아서 로드
        }
        public void LoadScene(SceneNames name, bool loadSave)
        {
            loadSaveFile = loadSave;
            LoadScene(name.ToString());
        }

        private IEnumerator LoadSceneAsync(string name)
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(name);
            asyncOperation.allowSceneActivation = false;

            while (asyncOperation.progress < 0.9f)
            {
                float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
                loadingProgress.value = progress;
                textProgress.text = $"{Mathf.RoundToInt(progress * 100)}%";
                yield return null;
            }

            // 로딩 완료 후 100%로 표시
            loadingProgress.value = 1f;
            textProgress.text = "100%";

            // 잠깐의 대기 시간 (선택 사항)
            yield return waitChangeDelay;

            // 씬 전환
            asyncOperation.allowSceneActivation = true;

            yield return new WaitForSeconds(0.1f);

            theSaveNLoad = FindObjectOfType<SaveNLoad>();

            if (loadSaveFile && theSaveNLoad != null)
            {
                theSaveNLoad.LoadData();
                Debug.Log("씬 로더에 있는 로드 데이터 완료");
            }
        }
        public void OnClick_ContinueGame()
        {
            LoadScene(SceneNames.RestaurantTest2, true); // 나중에 변경
        }

        public void OnClick_NewGame()
        {
            LoadScene(SceneNames.RestaurantTest2, false);
        }
    }
}
