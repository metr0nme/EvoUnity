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
    [SerializeField] public float walkMaxSpeed = 12f;
    [SerializeField] public float crouchMaxSpeed = 8f;
    [SerializeField] public float jumpVelocity = 50f;
    [SerializeField] public float gravity = 100f;

    [Header("Assignables")]
    [SerializeField] public Transform playerCam;
    [SerializeField] public Transform orientation;
    [SerializeField] public LayerMask groundLayer;
    [SerializeField] public LayerMask stickDetectLayers;

    private BoxCollider collider;
    private Vector3 colliderDefSize;
    private Vector3 colliderDefCenter;
    private Vector3 colliderCrouchSize;
    private Vector3 colliderCrouchCenter;

    // Script Var
    private string[] inputButtonsDef = {"W", "A", "S", "D", "Space", "C", "LeftShift"};
    private IDictionary<string, int> currInputs;
    private Rigidbody rb;
    private CharacterVariables charvar;
    private Animator playerAnimator;
    private float currentGroundMaxSpeed;

    private bool crouching = false;

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
        charvar = gameObject.GetComponent<CharacterVariables>();
        playerAnimator = charvar.playerAnimator;
        if(!isLocalPlayer)
            playerCam.gameObject.SetActive(false);

        initInputs();
        rb = GetComponent<Rigidbody>();
        currentGroundMaxSpeed = groundMaxSpeed;
        collider = charvar.playerModel.GetComponent<BoxCollider>();
        colliderDefSize = collider.size;
        colliderDefCenter = collider.center;
        colliderCrouchSize = colliderDefSize;
        colliderCrouchSize.y -= .2f;
        colliderCrouchCenter = colliderDefCenter;
        colliderCrouchCenter.y += .2f;
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
        return inputIntegers[0] == 1 ? 1 : inputIntegers[1] == 1 ? -1 : 0;
    }

    // Custom Movement Input
    private int RightDir()
    {
        return InverseInputOperation(new int[] {currInputs["D"], currInputs["A"]});
    }

    private int ForwardDir()
    {
        return InverseInputOperation(new int[] {currInputs["W"], currInputs["S"]});
    }

    private Vector3 ?StickDetect() // Returns Sticking Direction if Sticking
    {
        // iterate through 4 directions
        Vector3[] rayDirections = new Vector3[] {orientation.transform.forward, orientation.transform.right, -orientation.transform.right, -orientation.transform.forward};
        foreach(Vector3 dir in rayDirections)
        {
            RaycastHit hit;
            if(Physics.Raycast(orientation.transform.position, dir, out hit, 0.07f, stickDetectLayers))
            {
                return dir;
            }
        }
        return null;
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

        // dont allow acceleration on the Y axis
        Vector3 newVel = previousVelocity + accelDir * accelVel;
        newVel.y = previousVelocity.y;

        // fix sticking
        if(!ground)
        {
            Vector3 ?Sticking = StickDetect();
            if(Sticking != null)
            {
                for(int i = 0; i < 3; i++)
                {
                    if(Sticking.Value[i] != 0)
                        newVel[i] = 0;
                }
            }
        } else {
            newVel.y = 0;
        }

        return newVel;
    }

    private bool IsGrounded()
    {
        bool isGrounded = false;
        RaycastHit hit;
        if(Physics.Raycast(orientation.transform.position, orientation.transform.TransformDirection(-Vector3.up), out hit, 0.18f)) //, groundlayer
            isGrounded = true;

        return isGrounded;
    }

    private void Run()
    {
        
        Vector3 prevVel = rb.velocity;
        float speed = prevVel.magnitude;

        // apply friction if player is moving
        if(speed != 0 && IsGrounded())
        {
            float drop = speed * friction * Time.fixedDeltaTime;
            prevVel *= Mathf.Max(speed - drop, 0) / speed;
        }
        
        rb.velocity = GetMovementVelocity(prevVel, groundAccel, currentGroundMaxSpeed, true);

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

    private void Crouch()
    {
        crouching = true;
        currentGroundMaxSpeed = crouchMaxSpeed;
        playerAnimator.SetBool("Crouch", true); // play crouch animation
        collider.size = colliderCrouchSize; // shrink collider accordingly
        collider.center = colliderCrouchCenter;
    }

    private void UncrouchCheck()
    {
        if(playerAnimator.GetBool("Crouch") == true)
        {
            playerAnimator.SetBool("Crouch", false);
            collider.size = colliderDefSize;
            collider.center = colliderDefCenter;
        }
            
    }

    private void SlowWalk()
    {
        currentGroundMaxSpeed = walkMaxSpeed;
    }

    private void Movement()
    {
        
        // this is where the movement state function is decided,
        // so decide the MovementState str here aswell.
        if(IsGrounded())
        {
            if (currInputs["Space"] == 1)
                Jump();
            else
                Run();
        } else {
            Air();
        }

        // register slow walk/crouch functions
        if(currInputs["C"] == 1)
            Crouch();
        else {
            if(crouching) {
                crouching = false;
                UncrouchCheck();
                currentGroundMaxSpeed = groundMaxSpeed;
            }
        }

        if(currInputs["LeftShift"] == 1)
        {
            if(!crouching) {SlowWalk();}
        } else {
            if(!crouching) {currentGroundMaxSpeed = groundMaxSpeed;}
        }

        playerAnimator.SetFloat("Speed", rb.velocity.magnitude);

    }

    void Update()
    {

        if(!isLocalPlayer) { return; }

        updateGravity();

        RegisterInputs();

        Invoke("Movement", 0f);

    }
}
