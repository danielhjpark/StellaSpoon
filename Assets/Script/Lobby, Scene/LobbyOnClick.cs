using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyOnClick : MonoBehaviour
{
    public AudioClip soundClip;     // ����� ����� Ŭ��
    public Button targetButton;     // Ŭ���� ��ư
    void Start()
    {
        if (targetButton != null)
        {
            targetButton.onClick.AddListener(PlaySound);
        }
    }

    void PlaySound()
    {
        if (soundClip != null)
        {
            // ���� ��ġ���� �� ���� ��� (3D ����: ī�޶� ��ġ ���)
            AudioSource.PlayClipAtPoint(soundClip, Camera.main.transform.position);
        }
    }
}
