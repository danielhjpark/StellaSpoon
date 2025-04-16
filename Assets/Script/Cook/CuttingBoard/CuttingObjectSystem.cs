using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
using UnityEditor;
using Unity.VisualScripting;

public class CuttingObjectSystem : MonoBehaviour
{
    public GameObject rotateObject;

    public List<GameObject> SliceHorizontal(GameObject obj, int sliceCount, Material sliceMaterial)
    {
        if (obj == null) return null;
        List<GameObject> sliceObjects = new List<GameObject>();
        GameObject sliceTarget = obj;

        // 잘릴 방향과 위치 설정 (랜덤 또는 정해진 위치)
        Vector3 sliceDirection = Vector3.right; // 잘리는 방향
        Renderer objRenderer = obj.GetComponent<Renderer>();
        Vector3 min = objRenderer.bounds.min; // 바운딩 박스의 최소값
        Vector3 max = objRenderer.bounds.max; // 바운딩 박스의 최대값
        for (int i = 1; i < sliceCount; i++)
        {
            float t = i / (float)sliceCount;

            Vector3 slicePosition = Vector3.Lerp(min, max, t);

            SlicedHull hull = sliceTarget.Slice(slicePosition, sliceDirection, sliceMaterial);
            if (hull == null) continue;
            GameObject LowerHull = hull.CreateLowerHull(obj, sliceMaterial);
            GameObject upperHull = hull.CreateUpperHull(obj, sliceMaterial);

            Destroy(LowerHull.GetComponent<Collider>());
            LowerHull.AddComponent<BoxCollider>();
            sliceObjects.Add(LowerHull);

            if (i == sliceCount - 1)
            {
                Destroy(upperHull.GetComponent<Collider>());
                upperHull.AddComponent<BoxCollider>();
                sliceObjects.Add(upperHull);
                upperHull.transform.SetParent(rotateObject.transform);
            }
            if (sliceTarget == obj) { sliceTarget.SetActive(false); }
            else Destroy(sliceTarget);

            sliceTarget = upperHull;
            LowerHull.transform.SetParent(rotateObject.transform);
        }
        return sliceObjects;
    }

    public List<GameObject> SliceVertical(GameObject obj, int sliceCount, Material sliceMaterial)
    {
        if (obj == null) return null;
        List<GameObject> sliceObjects = new List<GameObject>();
        GameObject sliceTarget = obj;

        // 잘릴 방향과 위치 설정 (랜덤 또는 정해진 위치)
        Vector3 sliceDirection = Vector3.forward; // 잘리는 방향
        Renderer objRenderer = obj.GetComponent<Renderer>();
        Vector3 min = objRenderer.bounds.min; // 바운딩 박스의 최소값
        Vector3 max = objRenderer.bounds.max; // 바운딩 박스의 최대값
        for (int i = 1; i < sliceCount; i++)
        {
            float t = i / (float)sliceCount;

            Vector3 slicePosition = Vector3.Lerp(min, max, t);

            SlicedHull hull = sliceTarget.Slice(slicePosition, sliceDirection, sliceMaterial);
            if (hull == null) continue;
            GameObject LowerHull = hull.CreateLowerHull(obj, sliceMaterial);
            GameObject upperHull = hull.CreateUpperHull(obj, sliceMaterial);

            sliceObjects.Add(LowerHull);
            if (i == sliceCount - 1)
            {
                sliceObjects.Add(upperHull);
                upperHull.transform.SetParent(rotateObject.transform);
            }
            Destroy(sliceTarget);
            sliceTarget = upperHull;
            LowerHull.transform.SetParent(rotateObject.transform);
        }
        return sliceObjects;
    }

    public List<GameObject> SliceQuarter(GameObject obj, Material sliceMaterial)
    {
        if (obj == null) return null;
        // 잘릴 방향과 위치 설정 (랜덤 또는 정해진 위치)
        List<GameObject> sliceObjects = SliceHalf(obj, Vector3.right, sliceMaterial);

        List<GameObject> sliceObjects2 = SliceHalf(sliceObjects[0], Vector3.forward, sliceMaterial);
        sliceObjects2.AddRange(SliceHalf(sliceObjects[1], Vector3.forward, sliceMaterial));

        return sliceObjects2;
    }

    private List<GameObject> SliceHalf(GameObject obj, Vector3 dir, Material sliceMaterial)
    {
        if (obj == null) return null;
        // 잘릴 방향과 위치 설정 (랜덤 또는 정해진 위치)
        List<GameObject> sliceObjects = new List<GameObject>();

        Renderer objRenderer = obj.GetComponent<Renderer>();
        Vector3 sliceDirection = dir;
        Vector3 min = objRenderer.bounds.min; // 바운딩 박스의 최소값
        Vector3 max = objRenderer.bounds.max; // 바운딩 박스의 최대값

        Vector3 slicePosition = Vector3.Lerp(min, max, 0.5f);

        SlicedHull hull = obj.Slice(slicePosition, sliceDirection, sliceMaterial);
        GameObject LowerHull = hull.CreateLowerHull(obj, sliceMaterial);
        GameObject upperHull = hull.CreateUpperHull(obj, sliceMaterial);
        sliceObjects.Add(LowerHull);
        sliceObjects.Add(upperHull);
        Destroy(obj);

        return sliceObjects;
    }

}
