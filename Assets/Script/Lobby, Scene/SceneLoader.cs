using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;

public enum SceneNames { Lobby = 0, Playground, aRedForest }

namespace UnityNote
{
    public class SceneLoader : MonoBehaviour
    {
        public static SceneLoader Instance { get; private set; }

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

        private void Awake()
        {
            if (Instance != null && Instance != this)
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
            StartCoroutine(DelayedAssignButtonEvents());  // ���� �ð� �� ��ư ã��
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

        private IEnumerator LoadSceneAsync(string name)
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(name);
            asyncOperation.allowSceneActivation = false;  // ���� �ڵ����� ��ȯ���� �ʵ��� ����

            float percent = 0f;
            float loadingTime = 2.5f;

            // �� �ε� �����Ȳ ǥ��
            while (percent < 1f)
            {
                percent += Time.deltaTime / loadingTime;
                loadingProgress.value = percent;
                textProgress.text = $"{Mathf.RoundToInt(percent * 100)}%";  // ���൵ �ؽ�Ʈ ������Ʈ

                yield return null;
            }

            // �ε� �� ������ �ΰ� �� Ȱ��ȭ
            yield return waitChangeDelay;
            asyncOperation.allowSceneActivation = true;

            loadingScreen.SetActive(false);  // �ε� ȭ�� ��Ȱ��ȭ
        }
    }
}
