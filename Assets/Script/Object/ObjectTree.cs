using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTree : MonoBehaviour
{
    [SerializeField]
    private GameObject[] seeAliverrys; //�ð������� ���̱⸸ �ϴ� ������Ʈ

    [SerializeField]
    private GameObject aliverrys; //���������� ����Ʈ���� ������Ʈ

    [SerializeField]
    private int objectRespawnTime;

    public static bool canshake = true;

    private void Start()
    {
        canshake = true; //Ʈ�� ���� ����
    }
    public void DropAliverry()
    {
        foreach (GameObject obj in seeAliverrys) //�迭���� ������ŭ �ݺ��ؼ� ����Ʈ��
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
