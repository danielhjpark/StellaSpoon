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

        // ?��?��?�� RectTransform?�� �??��?��?��?��.
        Vector3[] worldCorners = new Vector3[4];
        targetPanel.GetWorldCorners(worldCorners);

        // ?��면에?�� 좌표�? ?��?��
        Vector2 minBounds = worldCorners[0]; // Bottom-left corner
        Vector2 maxBounds = worldCorners[2]; // Top-right corner

        // 마우?�� ?��치�?? �??��?��?��?��.
        Vector3 mousePosition = Input.mousePosition;

        // ?��?��?��?�� 마우?�� ?���?
        mousePosition.x = Mathf.Clamp(mousePosition.x, minBounds.x, maxBounds.x);
        mousePosition.y = Mathf.Clamp(mousePosition.y, minBounds.y, maxBounds.y);

        // 마우?�� ?��치�?? ?��?��?��?��?��?��?��.
        // Cursor.lockState = CursorLockMode.None; // 마우?�� 커서�? ?��?��?��?��?��?���? ?��금을 ?��?��?��?�� ?��?��?��.
        // Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        // Cursor.SetCursor(Texture2D.blackTexture, mousePosition, CursorMode.Auto);
    }
}
