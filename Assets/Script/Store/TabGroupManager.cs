using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabGroupManager : MonoBehaviour
{
    // 그룹에 속한 버튼들의 리스트
    private List<TabButton> tabButtons = new List<TabButton>();

    // 현재 선택된 버튼을 저장할 변수
    private static TabButton selectedTab;

    // 버튼이 클릭되었을 때 호출될 함수
    public void OnTabSelected(TabButton button)
    {
        // 만약 이전에 선택된 버튼이 있고, 새로 선택된 버튼과 다르다면
        if (selectedTab != null && selectedTab != button)
        {
            // 이전에 선택된 버튼을 'Normal' 상태로 되돌림
            selectedTab.Deselect();
        }

        // 새로 선택된 버튼을 'Selected' 상태로 만듦
        button.Select();

        // 현재 선택된 버튼을 새로 누른 버튼으로 업데이트
        selectedTab = button;
    }

    public static void OffTabSelected()
    {
        if (selectedTab != null)
        {
            selectedTab.Deselect();
            selectedTab = null;
        }
    }

    // 리스트에 버튼을 추가하는 함수
    public void RegisterButton(TabButton button)
    {
        if (!tabButtons.Contains(button))
        {
            tabButtons.Add(button);
        }
    }
}
