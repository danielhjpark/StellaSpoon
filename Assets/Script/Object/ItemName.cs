using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemName : MonoBehaviour
{
    public Item item;

    [SerializeField]
    private TextMeshProUGUI itemNameText;

    [SerializeField]
    private GameObject nameCanvas;

    void Start()
    {
        itemNameText.text = item.descItemName;
    }

    private void Update()
    {
        Vector3 dir = Camera.main.transform.position - transform.position;
        dir.y = 0; // y√‡ ∞Ì¡§
        nameCanvas.transform.rotation = Quaternion.LookRotation(-dir);
    }
}
