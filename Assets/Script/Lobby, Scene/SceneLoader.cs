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
        private GameObject loadingScreen; // �ε� ȭ��
        [SerializeField]
        private Image loadingBackground; // �ε� ȭ�鿡 ��µǴ� ��� �̹���
        [SerializeField]
        private TextMeshProUGUI[] tipText; // ��µ� �� ���
        [SerializeField]
        private Slider loadingProgress; // �ε� ���൵
        [SerializeField]
        private TextMeshProUGUI textProgress; // �ε� ���൵ �ؽ�Ʈ

        private WaitForSeconds waitChangeDelay; // �� ���� ���� �ð�

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
            // ��� ���� ��Ȱ��ȭ
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
            asyncOperation.allowSceneActivation = false; // �� �ڵ� ��ȯ ����

            // �񵿱� �۾�(�� �ҷ�����)�� �Ϸ�� ������ �ݺ�
            /*
            while(asyncOperation.isDone == false)
            {
                // �񵿱� �۾��� ���� ��Ȳ (0.0 ~ 1.0)
                loadingProgress.value = asyncOperation.progress;
                textProgress.text = $"{Mathf.RoundToInt(asyncOperation.progress * 100)}%";

                yield return null;
            }
            */

            // �ʹ� ������ ������ ������ �ð���ŭ �ε��� �����ϵ��� ����
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
