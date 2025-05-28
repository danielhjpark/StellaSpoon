using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractUI : MonoBehaviour
{
    [SerializeField] GameObject target;
    void Start()
    {
        this.transform.position = Camera.main.WorldToScreenPoint(target.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = Camera.main.WorldToScreenPoint(target.transform.position);
        Vector3 dir = Camera.main.transform.position - transform.position;
        dir.y = 0; // y√‡ ∞Ì¡§
        transform.rotation = Quaternion.LookRotation(-dir);
    }
}
