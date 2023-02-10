using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigTargetToMouse : MonoBehaviour
{   

    public Camera playerCam;

    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        Ray ray = playerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        Vector3 targetPos = ray.GetPoint(2);
        transform.position = targetPos;
    }
}
