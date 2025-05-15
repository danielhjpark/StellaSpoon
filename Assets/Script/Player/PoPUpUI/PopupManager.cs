using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance;

    public GameObject popupPrefab;
    public Transform popupContainer;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ShowPopup(string message, float duration = 2f)
    {
        GameObject popupGO = Instantiate(popupPrefab, popupContainer);
        popupGO.transform.SetAsLastSibling(); // ¹Ø¿¡ ½×ÀÌ°Ô

        PopupUI popup = popupGO.GetComponent<PopupUI>();
        popup.Setup(message, duration);
    }
}
