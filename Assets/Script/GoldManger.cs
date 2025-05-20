using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoldManger : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI goldText;
    void Update()
    {
        goldText.text = Manager.gold.ToString();
    }
}
