using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrycesMovement : MonoBehaviour
{

    // PORTED SOURCE MOVEMENT FROM EVOSTRIKE-RBX BY: Bryce "beters" Peters
    // THANKS TO: pdnghiaqoi, Dani

    [SerializeField] public float friction = 16f;
    [SerializeField] public float airAccel = 500f;
    [SerializeField] public float airMaxSpeed = 7f;
    [SerializeField] public float groundAccel = 10f;
    [SerializeField] public float groundMaxSpeed = 20f;
    [SerializeField] public float jumpVelocity = 50f;
    [SerializeField] public float gravity = 100f;


    // Assignables
    public Transform playerCam;
    public Transform orientation;
    public LayerMask groundLayer;

    // Script Var
    private float sensitivity = 50f;
    private float sensMultiplier = 1f;
    private float desiredX;
    private float xRotation;
    private string[] inputButtonsDef = {"W", "A", "S", "D", "Space"};
    private IDictionary<string, int> currInputs;
    private Rigidbody rb;

    private void initInputs()
    {

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currInputs = new Dictionary<string, int>();

        foreach(string str in inputButtonsDef)
            currInputs.Add(str, 0);

    }

    void updateGravity()
    {
        Vector3 grav = new Vector3(0, -gravity, 0);
        if(Physics.gravity != grav)
            Physics.gravity = grav;
    }

    void Start()
    {
        initInputs();
        rb = GetComponent<Rigidbody>();
    }

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

    private void RegisterInputs()
    {

        foreach(string str in inputButtonsDef)
        {
            KeyCode code = (KeyCode) System.Enum.Parse(typeof(KeyCode), str); // convert string into keycode
            if(Input.GetKey(code))
                currInputs[str] = 1;
            else
                currInputs[str] = 0;

        }

    }

    // Custom Movement Input
    private int RightDir()
    {
        int i = 0;
        if(currInputs["D"] == 1)
            i = 1;
        else if(currInputs["A"] == 1)
            i = -1;

        return i;
    }

    private int ForwardDir()
    {
        int i = 0;
        if(currInputs["W"] == 1)
            i = 1;
        else if(currInputs["S"] == 1)
            i = -1;

        return i;
    }
    //

    // GetMovementVelocity taken from "pdnghiaqoi" on Roblox DevForums, ported to unity by "beters" (me bitch)
    private Vector3 GetMovementVelocity(Vector3 previousVelocity, float acceleration, float maxSpeed, bool ground)
    {
        int currForwardDir = ForwardDir();
        int currRightDir = RightDir();

        Vector3 accelForward = Camera.main.transform.forward * currForwardDir;
        Vector3 accelSide = Camera.main.transform.right * currRightDir;
        Vector3 accelDir = (accelForward + accelSide).normalized;

        // fix zero errors
        if(currForwardDir == 0 && currRightDir == 0)
            accelDir = Vector3.zero;
            
        float projVel = Vector3.Dot(previousVelocity, accelDir);
        float accelVel = acceleration * Time.fixedDeltaTime;

        if(projVel + accelVel > maxSpeed)
            accelVel = Mathf.Max(maxSpeed - projVel, 0);

        Vector3 newVel = previousVelocity + accelDir * accelVel;
        if(ground)
            newVel.y = 0;

        return newVel;
    }

    private bool IsGrounded()
    {
        bool isGrounded = false;
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.TransformDirection(-Vector3.up), out hit, 0.03f, groundLayer))
            isGrounded = true;

        return isGrounded;
    }

    private void Run()
    {

        Vector3 prevVel = rb.velocity;
        float speed = prevVel.magnitude;

        // apply friction if player is moving
        if(speed != 0)
        {
            float drop = speed * friction * Time.fixedDeltaTime;
            prevVel *= Mathf.Max(speed - drop, 0) / speed;
        }
        
        rb.velocity = GetMovementVelocity(prevVel, groundAccel, groundMaxSpeed, true);

    }

    private void Air()
    {
        rb.velocity = GetMovementVelocity(rb.velocity, airAccel, airMaxSpeed, false);
        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y - (gravity * Time.fixedDeltaTime), rb.velocity.z);
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, jumpVelocity, rb.velocity.z);
        Air();
    }

    private void Movement()
    {
        if(IsGrounded())
        {
            if (currInputs["Space"] == 1)
                Jump();
            else
                Run();
        } else {
            Air();
        }
    }

    void Update()
    {

        updateGravity();

        Look();
        RegisterInputs();

        Invoke("Movement", 0f);

        //rb.velocity = playerVelocity;

    }
}
