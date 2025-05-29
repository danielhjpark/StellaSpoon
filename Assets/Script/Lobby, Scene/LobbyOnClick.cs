using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyOnClick : MonoBehaviour
{
    public AudioClip soundClip;     // 재생할 오디오 클립
    public Button targetButton;     // 클릭할 버튼
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
            // 현재 위치에서 한 번만 재생 (3D 사운드: 카메라 위치 사용)
            AudioSource.PlayClipAtPoint(soundClip, Camera.main.transform.position);
        }
    }
}
