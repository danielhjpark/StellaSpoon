using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndingUI : MonoBehaviour
{
    [SerializeField] GameObject fadePanel;
    [SerializeField] TextMeshProUGUI TitleText;
    [SerializeField] TextMeshProUGUI GameDayText;
    [SerializeField] TextMeshProUGUI KillMonsterCountText;
    [SerializeField] TextMeshProUGUI FirstCreateRecipeText;
    [SerializeField] TextMeshProUGUI EndRecipeText;
    [SerializeField] TextMeshProUGUI NPCSpawnCountText;
    [SerializeField] TextMeshProUGUI PressAnyButtonText;

    public float typingSpeed = 0.5f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            StartCoroutine(StartEndingScene());
        }
    }

    void SetText()
    {
        GameDayText.text += FindObjectOfType<GameTimeManager>().gameDays + "��";
        KillMonsterCountText.text += Manager.KillMonsterCount + "����"; //óġ�� ���� ��
        FirstCreateRecipeText.text += Manager.FirstCreateRecipe;
        NPCSpawnCountText.text += Manager.NPCSpawnCount + "��";
    }

    public IEnumerator StartEndingScene()
    {
        SetText();
        //yield return StartCoroutine(FadeOut());
        yield return StartCoroutine(fadePanel.GetComponent<FadeEffect>().Fade(0, 1f));
        yield return StartCoroutine(StartTyping(TitleText));
        yield return StartCoroutine(StartTyping(GameDayText));
        yield return StartCoroutine(StartTyping(KillMonsterCountText));
        yield return StartCoroutine(StartTyping(FirstCreateRecipeText));
        yield return StartCoroutine(StartTyping(EndRecipeText));
        yield return StartCoroutine(StartTyping(NPCSpawnCountText));


        yield return StartCoroutine(FadeInOutText());
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
         UnityNote.SceneLoader.Instance.LoadScene("Lobby");
    }

    private IEnumerator StartTyping(TextMeshProUGUI targetText)
    {
        string sentence = targetText.text;
        targetText.gameObject.SetActive(true);
        targetText.text = null; // ���� �ؽ�Ʈ �ʱ�ȭ

        //���Ⱑ �ι��̸� �� �ٲ�
        if (sentence.Contains("  "))
        {
            sentence = sentence.Replace("  ", "\n");
        }
        foreach (char letter in sentence.ToCharArray())
        {
            targetText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    IEnumerator FadeOut()
    {
        float elapsedTime = 0f; // ���� ��� �ð�
        float fadedTime = 3f; // �� �ҿ� �ð�

        while (elapsedTime <= fadedTime)
        {
            fadePanel.GetComponent<CanvasRenderer>().SetAlpha(Mathf.Lerp(0f, 1f, elapsedTime / fadedTime));

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        yield break;
    }

    IEnumerator FadeInOutText()
    {
        float alphaValue = 0.01f;
        bool isIncrease = true;
        PressAnyButtonText.gameObject.SetActive(true);
        while (true)
        {
            if (Input.anyKeyDown) break;
            if (PressAnyButtonText.alpha >= 0.95f) isIncrease = false;
            else if (PressAnyButtonText.alpha <= 0.1f) isIncrease = true;

            if (isIncrease) PressAnyButtonText.alpha += alphaValue;
            else PressAnyButtonText.alpha -= alphaValue;

            yield return null;
        }
    }
}
