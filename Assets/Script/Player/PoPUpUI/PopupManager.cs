using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance;

    public GameObject popupPrefab;
    public Transform popupContainer;

    public AudioClip popupSFX;
    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void ShowPopup(string message, float duration = 2f)
    {
        // �ִ� 5�� �̻��̸� ���� ������ �˾� ����
        if (popupContainer.childCount >= 5)
        {
            // ù ��° �˾� ���� (���� ������ ��)
            Transform oldest = popupContainer.GetChild(0);
            Destroy(oldest.gameObject);
        }

        GameObject popupGO = Instantiate(popupPrefab, popupContainer);
        popupGO.transform.SetAsLastSibling(); // �ؿ� ���̰�

        PopupUI popup = popupGO.GetComponent<PopupUI>();
        popup.Setup(message, duration);

        if (popupSFX != null)
            audioSource.PlayOneShot(popupSFX);
        else
            Debug.LogWarning("PopupManager: popupSFX�� �Ҵ���� �ʾҽ��ϴ�.");
    }
}
