using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{

    private float nextFireTick;
    private bool mouseDown = false;
    //Animator m_animator;

    private Vector3 currentRotation;
    private Vector3 targetRotation;

    private Camera cam;

    [SerializeField] private float fireRate = 0.11f;
    [SerializeField] private float recoilX = 1.0f;
    [SerializeField] private float recoilY = 1.0f;
    [SerializeField] private float recoilZ = 1.0f;

    [SerializeField] private float snappiness;
    [SerializeField] private float returnSpeed;

    void createFakeBullet()
    {

    }

    void setCamRotFire()
    {
        targetRotation += new Vector3(-recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
    }

    void Fire()
    {
        nextFireTick = Time.time + fireRate;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //RaycastHit hit;
        //if(Physics.Raycast(ray, out hit, 100))
        //m_animator.SetTrigger("Shoot");
        setCamRotFire();
    }

    void Start()
    {
        nextFireTick = Time.time;
        //m_animator = GetComponent<Animator>();
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    void Update()
    {

        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappiness * Time.fixedDeltaTime);
        cam.transform.localRotation = Quaternion.Euler(currentRotation);

        if(Input.GetMouseButtonDown(0))
            mouseDown = true;

        if(Input.GetMouseButtonUp(0))
            mouseDown = false;

        if(mouseDown && Time.time >= nextFireTick)
        {
            Fire();
            Debug.Log("Firing");
        };


    }

}
