using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class WokController : MonoBehaviour
{
    [SerializeField] GameObject wokObject;
    [SerializeField] GameObject wokObjectCenter;
    [SerializeField] GameObject spawnPoint;
    [SerializeField] GameObject[] spawnPrefabs;
    Quaternion initRotation;
    Vector3 initPosition;
    float additionalX = 10f;
    float additionalY = 10f;
    bool isTossing = false;

    void Start()
    {
        initPosition = wokObject.transform.position;
        initRotation = wokObject.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q)) {
            wokObject.transform.rotation = initRotation;
        }
        if(!isTossing) RayCheck();
    }

    void AddForceIngredients() {
        
    }

    void RayCheck() {
        Camera mainCamera = Camera.main;

        Ray ray = mainCamera.ScreenPointToRay(UnityEngine.InputSystem.Mouse.current.position.ReadValue());
        RaycastHit hit;
      
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Wok")))
        {
            Vector3 wokPosition = wokObjectCenter.transform.position;

            Vector2 wok2DPosition = new Vector2(wokPosition.x, wokPosition.z);
            Vector2 hit2DPosition = new Vector2(hit.point.x, hit.point.z);

            Vector2 direction = (hit2DPosition - wok2DPosition).normalized;

            float tiltAngle = 10f;
            float angle = Mathf.Atan2(direction.y, direction.x);
            float xTilt = Mathf.Sin(angle) * tiltAngle; 
            float yTilt = Mathf.Cos(angle) * tiltAngle; 
            Quaternion currentRotation = wokObjectCenter.transform.rotation;

            Quaternion tiltRotation = Quaternion.Euler(xTilt, yTilt, 0f);

            wokObject.transform.rotation = initRotation;
            wokObject.transform.rotation *= tiltRotation;

        }
        else {
            wokObject.transform.rotation = initRotation;
        }
        
    }
}
