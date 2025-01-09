using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CookManager : MonoBehaviour
{
    static public CookManager instance;

    [SerializeField] CuttingManager cuttingManager;

    bool isUseCuttingBoard;

    void Start()
    {
        instance = this;
        isUseCuttingBoard = false;
    }

    

    void Update()
    {
        if(isUseCuttingBoard) {
            cuttingObject();
        }
    }

    void useCuttingBoard(Ingredient ingredient, GameObject cuttingObject) {
        isUseCuttingBoard = true;
        //if(ingredient.type != CuttingBoard) return;
        //cuttingBoardSystem.LocateCuttingBoard(cuttingObject);
    }

    void cuttingObject() {
        //List<GameObject> cuttingObejct = cuttingBoardSystem.GetCurrentCuttingObjects();
    }

    public void DropObject(GameObject cuttingObject) {
        cuttingManager.LocateCuttingBoard(cuttingObject);
    }

}
