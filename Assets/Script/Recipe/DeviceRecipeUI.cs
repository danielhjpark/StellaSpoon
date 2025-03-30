using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeviceRecipeUI : MonoBehaviour
{
    [SerializeField]
    private GameObject recipeBase;

    [SerializeField]
    private List<GameObject> deActivatePanels; // ���� ���� �г��� �����ϴ� ����Ʈ

    private bool isActivated;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X)) // �׽�Ʈ �ڵ�
        {
            ToggleAllPanels(false); // ��� �г� ��Ȱ��ȭ
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleSpecificPanel(1, false); // �� ��° �гθ� ��Ȱ��ȭ (�ε��� 1)
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
    private void ToggleAllPanels(bool isActive) // �׽�Ʈ
    {
        foreach (GameObject panel in deActivatePanels)
        {
            panel.SetActive(isActive);
        }
    }

    private void ToggleSpecificPanel(int index, bool isActive) // �׽�Ʈ
    {
        if (index >= 0 && index < deActivatePanels.Count) // ��ȿ�� �ε������� üũ
        {
            deActivatePanels[index].SetActive(isActive);
        }
        else
        {
            Debug.LogWarning("�߸��� �г� �ε���: " + index);
        }
    }
}
