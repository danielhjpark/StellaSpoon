using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;
using System.IO;

public enum SceneNames { Lobby = 0, Playground, Restaurant, RestaurantTest2, Shop, aRedForest }

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

        private SaveNLoad theSaveNLoad; // ���
        private bool loadSaveFile = false;

        private bool isInChainedLoad = false; // �߰� �ܰ迡���� �ε�â ����

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
            // �� �ε� �� ����� �̺�Ʈ�� ����
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Log($"�� �ε���: {scene.name}");

            // �ε� ȭ�� ����
            if (!isInChainedLoad && loadingScreen != null)
                loadingScreen.SetActive(false);

            StartCoroutine(DelayedAssignButtonEvents());
        }

        private IEnumerator DelayedAssignButtonEvents()
        {
            yield return new WaitForSeconds(0.1f); // UI �ε�� �ð��� ��ٸ�
            AssignButtonEvents();
        }

        private void AssignButtonEvents()
        {
            // 1. FindWithTag() ��� (Ȱ��ȭ�� ������Ʈ ã��)
            GameObject buttonObject = GameObject.FindWithTag("YesButton");

            if (buttonObject == null)
            {
                // 2. FindObjectsOfTypeAll() ��� (��Ȱ��ȭ�� ������Ʈ�� ã��)
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

                // ��ư Ȱ��ȭ
                if (!titleButton.gameObject.activeSelf)
                {
                    titleButton.gameObject.SetActive(true);
                    Debug.Log("YesButton�� ��Ȱ��ȭ�Ǿ� �־����Ƿ� Ȱ��ȭ ��Ŵ.");
                }

                // ���� �̺�Ʈ ���� �� �ٽ� ����
                titleButton.onClick.RemoveAllListeners();
                titleButton.onClick.AddListener(() => LoadScene(SceneNames.Lobby));
                Debug.Log("YesButton�� ã��, �̺�Ʈ �߰� �Ϸ�.");
            }
            else
            {
                Debug.LogWarning("YesButton�� ã�� �� ����. �±� ���� �Ǵ� ��Ȱ��ȭ ���� Ȯ�� �ʿ�.");
            }
        }

        public void LoadScene(string name)
        {
            foreach (var text in tipText)
            {
                text.gameObject.SetActive(false);  // ��� �� ��Ȱ��ȭ
            }

            int index = Random.Range(0, tipText.Length);  // �� �� �ϳ��� �������� Ȱ��ȭ
            tipText[index].gameObject.SetActive(true);

            loadingProgress.value = 0f;
            loadingScreen.SetActive(true);  // �ε� ȭ�� Ȱ��ȭ

            StartCoroutine(LoadSceneAsync(name));
        }

        public void LoadScene(SceneNames name)
        {
            LoadScene(name.ToString());  // ������ �ϴ� ���� �̸��� �޾Ƽ� �ε�
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

            // �ε� �Ϸ� �� 100%�� ǥ��
            loadingProgress.value = 1f;
            textProgress.text = "100%";

            // ����� ��� �ð� (���� ����)
            yield return waitChangeDelay;

            // �� ��ȯ
            asyncOperation.allowSceneActivation = true;

            yield return new WaitForSeconds(0.1f);

            theSaveNLoad = FindObjectOfType<SaveNLoad>();

            if (loadSaveFile && theSaveNLoad != null)
            {
                theSaveNLoad.LoadData();
                Debug.Log("�� �δ��� �ִ� �ε� ������ �Ϸ�");
            }
        }
        public void OnClick_ContinueGame()
        {
            StartCoroutine(ContinueGameRoutine());
        }

        private IEnumerator ContinueGameRoutine()
        {
            string path = Application.dataPath + "/Saves/SaveFile.txt";

            // �ε� ȭ�� ����
            foreach (var text in tipText)
                text.gameObject.SetActive(false);

            int index = Random.Range(0, tipText.Length);
            tipText[index].gameObject.SetActive(true);

            loadingProgress.value = 0f;
            loadingScreen.SetActive(true);

            // �߰� �ε� ���� Ȱ��ȭ
            isInChainedLoad = true;

            if (!File.Exists(path))
            {
                Debug.LogWarning("���̺� ���� ����. �⺻ ������ �̵�");
                yield return StartCoroutine(LoadSceneAsync("RestaurantTest2"));
                isInChainedLoad = false; // ���� �ε��̹Ƿ� ����
                loadingScreen.SetActive(false);
                yield break;
            }

            string json = File.ReadAllText(path);
            SaveData tempData = JsonUtility.FromJson<SaveData>(json);
            string targetScene = tempData.currentSceneName;

            if (string.IsNullOrEmpty(targetScene))
            {
                Debug.LogWarning("����� �� ����. �⺻ ������ �̵�");
                yield return StartCoroutine(LoadSceneAsync("RestaurantTest2"));
                isInChainedLoad = false;
                loadingScreen.SetActive(false);
                yield break;
            }

            // 1. RestaurantTest2 �� �ε� (���� ������Ʈ�� ���� ����)
            AsyncOperation setupLoadOp = SceneManager.LoadSceneAsync("RestaurantTest2");
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

            // ���� ������Ʈ ó��
            GameObject player = GameObject.FindWithTag("Player");
            GameObject canvas = GameObject.Find("Canvas");
            GameObject manager = GameObject.Find("GameManager");

            if (player != null) DontDestroyOnLoad(player);
            if (canvas != null) DontDestroyOnLoad(canvas);
            if (manager != null) DontDestroyOnLoad(manager);

            yield return new WaitForSeconds(0.1f);

            // 2. ����� �� �ε�
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
                Debug.Log("ContinueGame: ���� ������ �ҷ����� �Ϸ�");
            }

            // ���� �ε��̹Ƿ� �ε� ����
            isInChainedLoad = false;
            loadingScreen.SetActive(false);
        }

        public void OnClick_NewGame()
        {
            LoadScene(SceneNames.RestaurantTest2, false);
        }
    }
}
