using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("필수 연결")]
    public TabGroupManager tabGroupManager; // 버튼들을 관리할 매니저

    [Header("색상을 변경할 대상")]
    public Image targetGraphic;  // 버튼 자신의 이미지 (필수)
    public Image otherImage;     // 함께 색을 바꿀 다른 이미지 (선택)
    public TextMeshProUGUI otherText;       // 함께 색을 바꿀 다른 텍스트 (선택)
    // public TMPro.TextMeshProUGUI otherText; // TextMeshPro를 쓴다면 이걸로 사용

    [Header("상태별 색상")]
    public Color normalColor = Color.white;
    public Color highlightedColor = new Color(0.9f, 0.9f, 0.9f);
    public Color pressedColor = new Color(0.7f, 0.7f, 0.7f);

    private Button button;
    private bool isSelected = false; // 현재 선택된 상태인지 확인

    void Start()
    {
        button = GetComponent<Button>();
        // 시작할 때 매니저에게 자신을 등록
        if (tabGroupManager != null)
        {
            tabGroupManager.RegisterButton(this);
        }

        // 버튼의 기본 색상 설정
        SetColor(normalColor);
    }

    // [중요] 이 함수를 버튼의 OnClick 이벤트에 연결해야 합니다.
    public void HandleClick()
    {
        if (tabGroupManager != null)
        {
            tabGroupManager.OnTabSelected(this);
        }
    }

    // 버튼이 '선택됨' 상태로 변경될 때 매니저가 호출
    public void Select()
    {
        isSelected = true;
        SetColor(pressedColor);
    }

    // 버튼이 '선택 해제됨' 상태로 변경될 때 매니저가 호출
    public void Deselect()
    {
        isSelected = false;
        SetColor(normalColor);
    }

    // 마우스가 위에 있을 때
    public void OnPointerEnter(PointerEventData eventData)
    {
        // 선택된 상태가 아닐 때만 하이라이트 색상 적용
        if (!isSelected)
        {
            SetColor(highlightedColor);
        }
    }

    // 마우스가 벗어났을 때
    public void OnPointerExit(PointerEventData eventData)
    {
        // 선택된 상태가 아닐 때만 원래 색상으로 복귀
        if (!isSelected)
        {
            SetColor(normalColor);
        }
    }

    // 실제 색상을 적용하는 헬퍼 함수
    private void SetColor(Color color)
    {
        if (targetGraphic != null) targetGraphic.color = color;
        this.GetComponent<Image>().color = color;
        if (otherImage != null) otherImage.color = color;
        if (otherText != null) otherText.color = color;
    }
}