using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SlotToolTip : MonoBehaviour
{
    [SerializeField]
    private GameObject go_Base;

    [SerializeField]
    private TextMeshProUGUI txt_ItemName;

    public void ShowToolTip(Item _item, Vector3 _pos)
    {
        go_Base.SetActive(true);

        _pos += new Vector3(go_Base.GetComponent<RectTransform>().rect.width * 0.5f,
                            -go_Base.GetComponent<RectTransform>().rect.height * 0.5f, 0);
        go_Base.transform.position = _pos;

        txt_ItemName.text = _item.itemName;
    }

    public void HideToolTip()
    {
        go_Base.SetActive(false);
    }
    public bool IsVisible()
    {
        return go_Base.activeSelf;
    }
}
