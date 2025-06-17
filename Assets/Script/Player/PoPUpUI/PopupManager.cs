using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance;

    public GameObject popupPrefab;
    public Transform popupContainer;

    public AudioClip popupSFX;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ShowPopup(string message, float duration = 2f)
    {
        // 최대 5개 이상이면 가장 오래된 팝업 제거
        if (popupContainer.childCount >= 5)
        {
            // 첫 번째 팝업 제거 (가장 오래된 것)
            Transform oldest = popupContainer.GetChild(0);
            Destroy(oldest.gameObject);
        }

        GameObject popupGO = Instantiate(popupPrefab, popupContainer);
        popupGO.transform.SetAsLastSibling(); // 밑에 쌓이게

        PopupUI popup = popupGO.GetComponent<PopupUI>();
        popup.Setup(message, duration);

        SoundManager.instance.PlayPlayerSFX(SoundManager.EPlayerSfx.Error);
    }
}
