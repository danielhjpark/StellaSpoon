using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearKingTriggerProxy : MonoBehaviour
{
    public BearKingScenes parent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            parent.OnPlayerTriggered(other.gameObject);
        }
    }
}
