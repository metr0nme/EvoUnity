using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMain : MonoBehaviour
{

    public Transform playerCam;
    public Transform orientation;

    [SerializeField] public float sensitivity = 50f;

    private float desiredX;
    private float xRotation;
    private Vector3 currentRotation;
    private Vector3 targetRotation;

    //TODO: change float values upon weapon equip (unless fire animation is playing)
    private float recoilReturnRate = 1.6f;
    private float recoilSnap = 6f;

    private bool fireSwitch = false;


    private void Start()
    {
        ClientEventManager.current.onFireShake += FireShake;
    }

    private void FireShake(float[] vectorArray)
    {
        targetRotation += new Vector3(vectorArray[0], vectorArray[1]/50, 0);
    }

    private void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        //Find current look rotation
        Vector3 rot = playerCam.transform.rotation.eulerAngles;
        desiredX = rot.y + mouseX;

        //Rotate, and also make sure we dont over- or under-rotate.
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, recoilReturnRate * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, recoilSnap * Time.deltaTime);

        playerCam.transform.localRotation = Quaternion.Euler(xRotation + currentRotation.x, desiredX + currentRotation.y, 0);
        orientation.transform.rotation = Quaternion.Euler(0, desiredX, 0);
    }

    private void Update()
    {
        Look();
    }

}
