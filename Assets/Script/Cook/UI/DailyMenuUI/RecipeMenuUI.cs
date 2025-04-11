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

        // ?Œ¨?„?˜ RectTransform?„ ê°?? ¸?˜µ?‹ˆ?‹¤.
        Vector3[] worldCorners = new Vector3[4];
        targetPanel.GetWorldCorners(worldCorners);

        // ?™”ë©´ì—?„œ ì¢Œí‘œë¥? ? œ?•œ
        Vector2 minBounds = worldCorners[0]; // Bottom-left corner
        Vector2 maxBounds = worldCorners[2]; // Top-right corner

        // ë§ˆìš°?Š¤ ?œ„ì¹˜ë?? ê°?? ¸?˜µ?‹ˆ?‹¤.
        Vector3 mousePosition = Input.mousePosition;

        // ?´?¨?”„?œ ë§ˆìš°?Š¤ ?œ„ì¹?
        mousePosition.x = Mathf.Clamp(mousePosition.x, minBounds.x, maxBounds.x);
        mousePosition.y = Mathf.Clamp(mousePosition.y, minBounds.y, maxBounds.y);

        // ë§ˆìš°?Š¤ ?œ„ì¹˜ë?? ?—…?°?´?Š¸?•©?‹ˆ?‹¤.
        // Cursor.lockState = CursorLockMode.None; // ë§ˆìš°?Š¤ ì»¤ì„œë¥? ?—…?°?´?Š¸?•˜? ¤ë©? ? ê¸ˆì„ ?•´? œ?•´?•¼ ?•©?‹ˆ?‹¤.
        // Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        // Cursor.SetCursor(Texture2D.blackTexture, mousePosition, CursorMode.Auto);
    }
}
