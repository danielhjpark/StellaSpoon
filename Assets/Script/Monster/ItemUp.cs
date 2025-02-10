using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUp : MonoBehaviour
{
    private Rigidbody rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        rigid.AddForce(new Vector3(Random.Range(-30f, 30f), 100, Random.Range(-30f, 30f)));
    }
}
