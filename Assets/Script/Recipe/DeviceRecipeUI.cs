using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeviceRecipeUI : MonoBehaviour
{
    [SerializeField]
    private GameObject recipeBase;

    [SerializeField]
    private List<GameObject> deActivatePanels; // 여러 개의 패널을 관리하는 리스트

    private bool isActivated;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X)) // 테스트 코드
        {
            ToggleAllPanels(false); // 모든 패널 비활성화
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleSpecificPanel(1, false); // 두 번째 패널만 비활성화 (인덱스 1)
        }
    }
    public void OpenRecipe()
    {
        if (isActivated) return;

        recipeBase.SetActive(true);
        isActivated = true;
    }

    public void CloseRecipe()
    {
        recipeBase.SetActive(false);
        isActivated = false;
    }
    private void ToggleAllPanels(bool isActive) // 테스트
    {
        foreach (GameObject panel in deActivatePanels)
        {
            panel.SetActive(isActive);
        }
    }

    private void ToggleSpecificPanel(int index, bool isActive) // 테스트
    {
        if (index >= 0 && index < deActivatePanels.Count) // 유효한 인덱스인지 체크
        {
            deActivatePanels[index].SetActive(isActive);
        }
        else
        {
            Debug.LogWarning("잘못된 패널 인덱스: " + index);
        }
    }
}
