using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;
using System.IO;

public enum SceneNames { Lobby = 0, Playground, Restaurant, Serenoxia, Shop, aRedForest }

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

        private bool isInChainedLoad = false; // 중간 단계에서는 로딩창 유지
        private bool isNewGame = false;
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
            if (isNewGame)
            {
                GameTimeManager timeManager = FindObjectOfType<GameTimeManager>();
                if (timeManager != null)
                {
                    timeManager.DisableAutoLoad(); // 자동 로드 막기
                    timeManager.InitializeNewGameTime();
                    Debug.Log("GameTime 초기화 완료 (NewGame)");
                }
                isNewGame = false;
            }
            // 저장 데이터 로딩은 여기서 확실하게 처리
            if (loadSaveFile)
            {
                theSaveNLoad = FindObjectOfType<SaveNLoad>();
                if (theSaveNLoad != null)
                {
                    theSaveNLoad.LoadData();
                    Debug.Log("씬 로드 후 저장 데이터 불러오기 완료 (OnSceneLoaded)");
                }
                loadSaveFile = false; // 반드시 초기화
            }

            // 로딩 화면 종료
            if (!isInChainedLoad && loadingScreen != null)
                loadingScreen.SetActive(false);

            StartCoroutine(DelayedAssignButtonEvents());
            if (scene.name == "Lobby")
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
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
            float minLoadingTime = 2.0f; // 최소 로딩 시간 (예: 2초)
            float startTime = Time.time;

            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(name);
            asyncOperation.allowSceneActivation = false;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            while (asyncOperation.progress < 0.9f)
            {
                float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
                loadingProgress.value = progress;
                textProgress.text = $"{Mathf.RoundToInt(progress * 100)}%";
                yield return null;
            }

            loadingProgress.value = 1f;
            textProgress.text = "100%";

            // 최소 로딩 시간 대기
            float remainingTime = minLoadingTime - (Time.time - startTime);
            if (remainingTime > 0f)
                yield return new WaitForSeconds(remainingTime);

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
            StartCoroutine(ContinueGameRoutine());
        }

        private IEnumerator ContinueGameRoutine()
        {
            string path = Application.dataPath + "/Saves/SaveFile.txt";

            // 로딩 화면 시작
            foreach (var text in tipText)
                text.gameObject.SetActive(false);

            int index = Random.Range(0, tipText.Length);
            tipText[index].gameObject.SetActive(true);

            loadingProgress.value = 0f;
            loadingScreen.SetActive(true);
            isInChainedLoad = true;

            if (!File.Exists(path))
            {
                Debug.LogWarning("세이브 파일 없음. 기본 씬으로 이동");
                yield return StartCoroutine(LoadSceneAsync("Restaurant"));
                isInChainedLoad = false;
                loadingScreen.SetActive(false);
                yield break;
            }

            string json = File.ReadAllText(path);
            SaveData tempData = JsonUtility.FromJson<SaveData>(json);
            string targetScene = tempData.currentSceneName;

            if (string.IsNullOrEmpty(targetScene))
            {
                Debug.LogWarning("저장된 씬 없음. 기본 씬으로 이동");
                yield return StartCoroutine(LoadSceneAsync("Restaurant"));
                isInChainedLoad = false;
                loadingScreen.SetActive(false);
                yield break;
            }

            // 여기 추가: 중복 로딩 방지
            if (targetScene == "Restaurant")
            {
                Debug.Log("저장된 씬이 Restaurant면 바로 로드");

                loadSaveFile = true;

                PlayerSpawn.useSavedPosition = true;
                yield return StartCoroutine(LoadSceneAsync(targetScene));

                isInChainedLoad = false;
                loadingScreen.SetActive(false);
                yield break;
            }

            // 기존 중간 로딩 단계 유지
            // 1. RestaurantTest2 임시 로드
            AsyncOperation setupLoadOp = SceneManager.LoadSceneAsync("Restaurant");
            setupLoadOp.allowSceneActivation = false;

            while (setupLoadOp.progress < 0.9f)
            {
                float progress = Mathf.Clamp01(setupLoadOp.progress / 0.9f);
                loadingProgress.value = progress * 0.4f;
                textProgress.text = $"{Mathf.RoundToInt(progress * 40)}%";
                yield return null;
            }

            setupLoadOp.allowSceneActivation = true;
            yield return new WaitUntil(() => setupLoadOp.isDone);

            GameObject player = GameObject.FindWithTag("Player");
            GameObject canvas = GameObject.Find("Canvas");
            GameObject manager = GameObject.Find("GameManager");

            if (player != null) DontDestroyOnLoad(player);
            if (canvas != null) DontDestroyOnLoad(canvas);
            if (manager != null) DontDestroyOnLoad(manager);

            yield return new WaitForSeconds(0.1f);

            // 2. 저장된 씬 로드
            AsyncOperation mainLoadOp = SceneManager.LoadSceneAsync(targetScene);
            mainLoadOp.allowSceneActivation = false;

            while (mainLoadOp.progress < 0.9f)
            {
                float progress = 0.4f + (mainLoadOp.progress / 0.9f) * 0.6f;
                loadingProgress.value = progress;
                textProgress.text = $"{Mathf.RoundToInt(progress * 100)}%";
                yield return null;
            }

            mainLoadOp.allowSceneActivation = true;
            yield return new WaitUntil(() => mainLoadOp.isDone);

            yield return new WaitForSeconds(0.1f);

            theSaveNLoad = FindObjectOfType<SaveNLoad>();
            if (theSaveNLoad != null)
            {
                theSaveNLoad.LoadData();
                Debug.Log("ContinueGame: 저장 데이터 불러오기 완료");
            }

            isInChainedLoad = false;
            loadingScreen.SetActive(false);
        }

        public void OnClick_NewGame()
        {
            PlayerSpawn.useSavedPosition = false;
            isNewGame = true;
            // 여기서 변수들 초기화
            Manager.stage_01_clear = false;
            Manager.stage_02_clear = false;
            Manager.gold = 0;
            StoreUIManager.currentPanLevel = 1;
            StoreUIManager.currentPotLevel = 1;
            StoreUIManager.currentWorLevel = 1;
            StoreUIManager.currentCuttingBoardLevel = 1;
            LoadScene(SceneNames.Restaurant, false);
        }
    }
}
