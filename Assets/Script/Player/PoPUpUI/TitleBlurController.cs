using UnityEngine;

public class TitleBlurController : MonoBehaviour
{
    [SerializeField] private GameObject blurPanel;
    [SerializeField] private GameObject confirmUI;

    public void OnClickBackToTitle()
    {
        blurPanel.SetActive(true);
        confirmUI.SetActive(true);
    }

    public void OnClickCancel()
    {
        blurPanel.SetActive(false);
        confirmUI.SetActive(false);
    }

    public void OnClickConfirm()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScene");
    }
}
