using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeMenuUI : MonoBehaviour
{
    GameObject[] slot;
    [SerializeField]RectTransform targetPanel;

    void Start()
    {
        
    }

    void Update()
    {
        if (targetPanel == null)
            return;

        // 패널의 RectTransform을 가져옵니다.
        Vector3[] worldCorners = new Vector3[4];
        targetPanel.GetWorldCorners(worldCorners);

        // 화면에서 좌표를 제한
        Vector2 minBounds = worldCorners[0]; // Bottom-left corner
        Vector2 maxBounds = worldCorners[2]; // Top-right corner

        // 마우스 위치를 가져옵니다.
        Vector3 mousePosition = Input.mousePosition;

        // 클램프된 마우스 위치
        mousePosition.x = Mathf.Clamp(mousePosition.x, minBounds.x, maxBounds.x);
        mousePosition.y = Mathf.Clamp(mousePosition.y, minBounds.y, maxBounds.y);

        // 마우스 위치를 업데이트합니다.
        Cursor.lockState = CursorLockMode.None; // 마우스 커서를 업데이트하려면 잠금을 해제해야 합니다.
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        Cursor.SetCursor(Texture2D.blackTexture, mousePosition, CursorMode.Auto);
    }
}
