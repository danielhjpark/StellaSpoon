using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TypingEffect : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;  // �Ҵ����� TextMeshProUGUI
    public float typingSpeed = 0.05f;     // Ÿ���� �ӵ�

    private Coroutine typingCoroutine;

    public void StartTyping(string sentence)
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        typingCoroutine = StartCoroutine(TypeSentence(sentence));
    }

    private IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = null; // ���� �ؽ�Ʈ �ʱ�ȭ

        //���Ⱑ �ι��̸� �� �ٲ�
        if (sentence.Contains("  "))
        {
            sentence = sentence.Replace("  ", "\n");
        }
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
