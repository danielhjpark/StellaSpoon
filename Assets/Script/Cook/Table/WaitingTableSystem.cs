using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingTableSystem : MonoBehaviour
{
    const int TotalTableCount = 3;

    [SerializeField] GameObject[] waitingTables;

    private bool[] useTableID = new bool[TotalTableCount];

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < TotalTableCount; i++) useTableID[i] = false; 
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UseWaitingTable(GameObject menu)
    {
        for(int i = 0; i < TotalTableCount; i++ )
        {
            if (!useTableID[i])
            {
                menu.transform.SetParent(waitingTables[i].transform);
                menu.transform.localPosition = Vector3.zero;
                useTableID[i] = true;
                return;
            }
        }
    }

    public void CheckUseTable()
    {
        for(int i = 0; i < TotalTableCount; i++ )
        {
            if (waitingTables[i].transform.childCount <= 0)
            {
                useTableID[i] = false;
            }
        }
    }

    public bool IsCanUseTable()
    {
        for (int i = 0; i < TotalTableCount; i++)
        {
            if (!useTableID[i]) return true;
        }
        return false;
    }
}
