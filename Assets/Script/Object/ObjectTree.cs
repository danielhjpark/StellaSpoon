using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTree : MonoBehaviour
{
    [SerializeField]
    private GameObject[] seeAliverrys; //시각적으로 보이기만 하는 오브젝트

    [SerializeField]
    private GameObject aliverrys; //실제적으로 떨어트리는 오브젝트

    [SerializeField]
    private int objectRespawnTime;

    public static bool canshake = true;

    private void Start()
    {
        canshake = true; //트리 흔들기 가능
    }
    public void DropAliverry()
    {
        foreach (GameObject obj in seeAliverrys) //배열내의 갯수만큼 반복해서 떨어트림
        {
            obj.SetActive(false);
            if (obj != null)
            {
                Instantiate(aliverrys, obj.transform.position, Quaternion.identity);
            }
        }
                canshake = false;
    }
}
