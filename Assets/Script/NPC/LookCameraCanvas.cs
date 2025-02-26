using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookCameraCanvas : MonoBehaviour
{
    //메뉴 UI가 카메라를 따라가게 구현
    GameObject cam;
    private void Start()
    {
        cam = Camera.main.gameObject;

    }

    private void LateUpdate()
    {
        if (cam != null)
        {
            transform.LookAt(cam.transform);
            transform.Rotate(0, 180, 0);
        }
    }
}
