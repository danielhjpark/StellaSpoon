using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectMap : MonoBehaviour
{
    [SerializeField]
    private GameObject blueMap;

    [SerializeField]
    private Sprite blueMapCloseSprite;
    [SerializeField]
    private Sprite blueMapOpenSprite;

    private void Start()
    {
        blueMap.GetComponent<Image>().sprite = blueMapCloseSprite;
        blueMap.GetComponent<Button>().enabled = false;
    }
    private void Update()
    {
        if(Manager.stage_01_clear)
        {
            blueMap.GetComponent<Image>().sprite = blueMapOpenSprite;
            blueMap.GetComponent<Button>().enabled = true;
        }
    }
}
