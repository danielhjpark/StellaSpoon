using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollViewSystem : MonoBehaviour
{
    [Header("Button UI Objects")]
    [SerializeField] GameObject buttonUIObject;
    [SerializeField] GameObject rightButton;
    [SerializeField] GameObject leftButton;
    [SerializeField] RectTransform scrollRect;

    private int rectViewPos = 0;
    private int viewPosValue = -220;
    private Coroutine movePanel;

    [SerializeField] GameObject scrollParent;

    void Start()
    {
        RecipePanelInit();
    }

    void Update()
    {
        HideButton();
    }
    public void RecipePanelInit()
    {
        int correctRecipeCount = scrollParent.transform.childCount;
        int rectWidth = correctRecipeCount * 220, rectHeight = 200;//size + spacing
        scrollRect.sizeDelta = new Vector2(rectWidth, rectHeight);
        scrollRect.pivot = new Vector2(0, 0.5f);

        if (scrollParent.transform.childCount > 3)
        {
            buttonUIObject.SetActive(true);
        }
        else
        {
            buttonUIObject.SetActive(false);
        }
    }

    //---------------Button------------------//

    void HideButton()
    {
        rightButton.SetActive(true);
        leftButton.SetActive(true);

        if (rectViewPos <= 0)
            leftButton.SetActive(false);

        if (rectViewPos >= scrollParent.transform.childCount - 3)
            rightButton.SetActive(false);
    }

    public void RectMoveRight()
    {
        if (rectViewPos >= scrollParent.transform.childCount - 3 || movePanel != null) return;
        rectViewPos++;
        int rectWidth = rectViewPos * viewPosValue;
        Vector2 targetPos = new Vector2(rectWidth, 100);
        movePanel = StartCoroutine(MovePanel(targetPos));
    }

    public void RectMoveLeft()
    {
        if (rectViewPos <= 0 || movePanel != null) return;
        int viewPosValue = -220;
        rectViewPos--;
        int rectWidth = rectViewPos * viewPosValue;
        Vector2 targetPos = new Vector2(rectWidth, 100);
        movePanel = StartCoroutine(MovePanel(targetPos));
    }

    IEnumerator MovePanel(Vector2 targetPos)
    {
        while (true)
        {
            if (Vector2.Distance(scrollRect.anchoredPosition, targetPos) < 2f)
            {
                scrollRect.anchoredPosition = targetPos;
                break;
            }
            scrollRect.anchoredPosition = Vector2.Lerp(scrollRect.anchoredPosition, targetPos, Time.deltaTime * 5f);
            yield return null;
        }
        movePanel = null;
    }
}
