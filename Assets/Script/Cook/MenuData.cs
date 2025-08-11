using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuData : MonoBehaviour
{
    public Recipe menu;
    public bool useTable = false;
    float makeTime = 0;

    void Update()
    {
        makeTime += Time.deltaTime * 60f;

        if (makeTime >= 3600)
        {
            menu = CookManager.instance.failMenu;
            GameObject failMenuObject = Instantiate(menu.menuPrefab, Vector3.zero, Quaternion.identity);
            Destroy(this.gameObject.transform.GetChild(0).gameObject);
            failMenuObject.transform.SetParent(this.gameObject.transform);
            failMenuObject.transform.localPosition = Vector3.zero;
        }
    }
}
