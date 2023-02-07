using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewmodelScript : MonoBehaviour
{
    private SpringVector3 fireSpring;
    private float swaySmooth = 4;
    private float swayMultiplier = 1;

    void Start()
    {
        fireSpring = new SpringVector3()
        {
            StartValue = Vector3.zero,
            EndValue = Vector3.zero,
            Damping = 6,
            Stiffness = 100,
        };
        ClientEventManager.current.onVMFireShake += VMFireShake;
    }

    void Update()
    {
        transform.localPosition = fireSpring.Evaluate(Time.fixedDeltaTime); // update spring pos
    }

    void VMFireShake(Vector3 push)
    {
        fireSpring.Reset();
        fireSpring.InitialVelocity = push;
    }

    void HandleMouseSway()
    {
        // get mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * swayMultiplier -90f;
        float mouseY = Input.GetAxisRaw("Mouse Y") * swayMultiplier;
        // calculate target rotation
        Quaternion rotationX = Quaternion.AngleAxis( mouseY, Vector3.left);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);
        Quaternion targetRotation = rotationX * rotationY;
        // rotate 
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, swaySmooth * Time.deltaTime);
    }

}
