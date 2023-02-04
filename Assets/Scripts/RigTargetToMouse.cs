using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigTargetToMouse : MonoBehaviour
{
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 targetPos = ray.GetPoint(2);
        transform.position = targetPos;
    }
}
