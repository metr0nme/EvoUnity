using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningAnimation : MonoBehaviour
{

    public GameObject playerObj;

    private Rigidbody rb;
    private Animator animator;

    void Start()
    {
        rb = playerObj.GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Run Animation
        float speedPerc = rb.velocity.magnitude / 20f;
        animator.SetFloat("speed", speedPerc);
    }


}
