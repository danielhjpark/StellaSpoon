using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public List<FollowTargetController> followTargetControllers;
    public Material yellow, green;

    private void OnTriggerEnter(Collider other)
    {
        foreach( var controller in followTargetControllers)
        {
            //controller.ChangePosition();
        }
        ChangeMaterial();
    }

    async void ChangeMaterial()
    {
        GetComponent<Renderer>().material = yellow;
        await Task.Delay(1000);
        GetComponent<Renderer>().material = green;
    }
}
