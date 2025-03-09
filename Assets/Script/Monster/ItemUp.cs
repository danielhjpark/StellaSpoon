using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUp : MonoBehaviour
{
    private Rigidbody rigid;

    [SerializeField]
    private int deleteTime = 7; //아이템 삭제 시간

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        rigid.AddForce(new Vector3(Random.Range(-30f, 30f), 100, Random.Range(-30f, 30f)));

        StartCoroutine(deleteItem());
    }

    IEnumerator deleteItem()
    {
        yield return new WaitForSeconds(deleteTime);

        Destroy(gameObject);
    }

}
