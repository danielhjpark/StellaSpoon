using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CuttingBoard : MonoBehaviour
{
    public event Action OnCuttingSystem;

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Ingredient")) {
            OnCuttingSystem?.Invoke();
        }
    }
}
