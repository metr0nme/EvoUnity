using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BrycesMovement : NetworkBehaviour
{

    // PORTED SOURCE MOVEMENT FROM EVOSTRIKE-RBX BY: Bryce "beters" Peters
    // THANKS TO: "pdnghiaqoi" on Roblox

    // Movement Settings
    [Header("Movement Settings")]
    [SerializeField] public float friction = 16f;
    [SerializeField] public float airAccel = 500f;
    [SerializeField] public float airMaxSpeed = 7f;
    [SerializeField] public float groundAccel = 10f;
    [SerializeField] public float groundMaxSpeed = 20f;
    [SerializeField] public float jumpVelocity = 50f;
    [SerializeField] public float gravity = 100f;

    [Header("Assignables")]
    [SerializeField] public Transform playerCam;
    [SerializeField] public Transform orientation;
    [SerializeField] public LayerMask groundLayer;

    // Script Var
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

        if(!isLocalPlayer)
            playerCam.gameObject.SetActive(false);

        initInputs();
        rb = GetComponent<Rigidbody>();
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

    // beters: i made this function to shorten the conversion of 1, 0 to 1, -1
    // its very important you have the arrays in the correct order {+, -} or {forward, backward}
    private int InverseInputOperation(int[] inputIntegers)
    {
        int i = 0;
        i = inputIntegers[0] == 1 ? 1 : (inputIntegers[1] == 1 ? -1 : 0);
        return i;
    }

    // Custom Movement Input
    private int RightDir()
    {
        int[] rightInputArray = {currInputs["D"], currInputs["A"]};
        return InverseInputOperation(rightInputArray);
    }

    private int ForwardDir()
    {
        int[] forwardInputArray = {currInputs["W"], currInputs["S"]};
        return InverseInputOperation(forwardInputArray);
    }

    // GetMovementVelocity taken from "pdnghiaqoi" on Roblox DevForums, ported to unity by "beters" (me bitch)
    private Vector3 GetMovementVelocity(Vector3 previousVelocity, float acceleration, float maxSpeed, bool ground)
    {
        int currForwardDir = ForwardDir();
        int currRightDir = RightDir();

        Vector3 accelForward = playerCam.transform.forward * currForwardDir;
        Vector3 accelSide = playerCam.transform.right * currRightDir;
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
        //Physics.Raycast(transf)
        if(Physics.Raycast(orientation.transform.position, orientation.transform.TransformDirection(-Vector3.up), out hit, 0.18f)) //, groundlayer
            isGrounded = true;

        Debug.Log(isGrounded.ToString());

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

        if(!isLocalPlayer) { return; }

        updateGravity();

        RegisterInputs();

        Invoke("Movement", 0f);

    }
}
