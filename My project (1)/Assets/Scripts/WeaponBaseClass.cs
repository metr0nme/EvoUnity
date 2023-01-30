using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBaseClass : MonoBehaviour
{

    public WeaponScriptable weaponValues;

    private float nextFireTick;
    private bool mouseDown = false;
    //Animator m_animator;

    private Vector3 currentRotation;
    private Vector3 targetRotation;

    private Camera cam;
    
    private float recoilSnap;
    private float recoilReturnRate;
    private Vector3 recoilValue;
    private float fireRate;
    private int magSize;
    private int totalAmmo;

    // Changing Var
    public int currentMagSize;
    public int currentTotalAmmo;

    void Start()
    {
        nextFireTick = Time.time;
        //m_animator = GetComponent<Animator>();
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        recoilSnap = weaponValues.recoilSnap;
        recoilReturnRate = weaponValues.recoilReturnRate;
        recoilValue = weaponValues.recoilValue;
        fireRate = weaponValues.fireRate;
        magSize = weaponValues.magazineSize;
        totalAmmo = weaponValues.totalAmmo;
        currentMagSize = magSize;
        currentTotalAmmo = totalAmmo;
    }

    void setCamRotFire()
    {
        targetRotation += new Vector3(-recoilValue.x, Random.Range(-recoilValue.y, recoilValue.y), Random.Range(-recoilValue.z, recoilValue.z));
    }

    void Fire()
    {
        if (currentMagSize <= 0)
            return;
        
        currentMagSize--;
        nextFireTick = Time.time + fireRate;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //RaycastHit hit;
        //if(Physics.Raycast(ray, out hit, 100))
        //m_animator.SetTrigger("Shoot");
        setCamRotFire();
    }

    void Update()
    {

        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, recoilReturnRate * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, recoilSnap * Time.fixedDeltaTime);
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
