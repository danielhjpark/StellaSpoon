using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TypingEffect : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;  // 할당해줄 TextMeshProUGUI
    public float typingSpeed = 0.05f;     // 타이핑 속도

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
        dialogueText.text = null; // 기존 텍스트 초기화

        //띄어쓰기가 두번이면 줄 바꿈
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
