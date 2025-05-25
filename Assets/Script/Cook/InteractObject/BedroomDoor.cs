using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedroomDoor : InteractObject
{
    Animator doorAnimator;
    private string doorOpenName = "character_nearby";

    void Start()
    {
        doorAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        doorAnimator.SetBool(doorOpenName, true);
    }

    private void OnTriggerExit(Collider other)
    {
        doorAnimator.SetBool(doorOpenName, false);
    }
}
