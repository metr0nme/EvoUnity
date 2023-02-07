using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRecoil : MonoBehaviour
{
    private Vector3 currentRotation;
    private Vector3 targetRotation;

    //TODO: change float values upon weapon equip (unless fire animation is playing)
    private float recoilReturnRate = 1f;
    private float recoilSnap = 6f;

    private bool fireSwitch = false;

    private void Start()
    {
        ClientEventManager.current.onFireShake += FireShake;
    }

    private void FireShake(float[] vectorArray)
    {
        targetRotation += new Vector3(vectorArray[0], vectorArray[1]/(16/9), 0);
    }

    private void Update()
    {
        if(targetRotation == Vector3.zero) { return; }
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, recoilReturnRate * Time.fixedDeltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, recoilSnap * Time.fixedDeltaTime);
        transform.localRotation = Quaternion.Euler(currentRotation);
    }

}
