using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefrigeratorInventory : MonoBehaviour
{
    public RefrigeratorSlot[] refrigeratorSlots;
    
    bool isFullInventory = false;
    
    void Start()
    {
        refrigeratorSlots = this.GetComponentsInChildren<RefrigeratorSlot>();
    }

    private RefrigeratorSlot CheckEmptySlot() {
        foreach(RefrigeratorSlot slot in refrigeratorSlots) {
            if(slot.IsEmpty()) return slot;
        }
        Debug.Log("Not found Slot");
        return null;
    }
}
