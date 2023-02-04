using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaniPlayerLook : MonoBehaviour
{

    [SerializeField] public float sensitivity = 50f;
    private float sensMultiplier = 1f;

    public Transform playerCam;
    public Transform orientation;

    private float xRotation;
    private float desiredX;

    // Look taken from Dani
    private void Look()
    {

        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime * sensMultiplier;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime * sensMultiplier;

        //Find current look rotation
        Vector3 rot = playerCam.transform.localRotation.eulerAngles;
        desiredX = rot.y + mouseX;

        //Rotate, and also make sure we dont over- or under-rotate.
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //Perform the rotations
        playerCam.transform.localRotation = Quaternion.Euler(xRotation, desiredX, 0);
        orientation.transform.localRotation = Quaternion.Euler(0, desiredX, 0);
    }

    // Update is called once per frame
    void Update()
    {
        Look();
    }
}
