using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBaseClass : MonoBehaviour
{

    // Script Var
    private Animator animator;
    private Camera cam;
    
    // WeaponScriptables Var
    public WeaponScriptable weaponValues;
    private float recoilSnap;
    private float recoilReturnRate;
    private Vector3 recoilValue;
    private float fireRate;
    private int magSize;
    private int totalAmmo;
    private bool automatic;

    // Changing (Mut) Var
    public int currentMagSize;
    public int currentTotalAmmo;
    private float nextFireTick;
    private bool mouseDown;
    private bool autoCanFire;
    private Vector3 currentRotation;
    private Vector3 targetRotation;

    void Start()
    {
        animator = GetComponent<Animator>();
        cam = GameObject.Find("Main Camera").GetComponent<Camera>(); // apparently GameObject.Find is a very bad practice because it is SLOW. It's faster & easier to attatch the main camera to the script in the hierarchy
        recoilSnap = weaponValues.recoilSnap;
        recoilReturnRate = weaponValues.recoilReturnRate;
        recoilValue = weaponValues.recoilValue;
        fireRate = weaponValues.fireRate;
        magSize = weaponValues.magazineSize;
        totalAmmo = weaponValues.totalAmmo;
        automatic = weaponValues.automatic;
        mouseDown = false;
        autoCanFire = false;
        nextFireTick = Time.time;
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
        
        if(!automatic) // automatic weapon registration
            if(!autoCanFire)
                return;
            else
                autoCanFire = false;
            
        currentMagSize--;
        nextFireTick = Time.time + fireRate;

        // this will cast a ray and give you the result ( this is where you can apply damage or do whatever :3 )
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //RaycastHit hit;
        //if(Physics.Raycast(ray, out hit, 100)) (hit detection right here)

        animator.SetTrigger("Shoot"); // play fire animation
        setCamRotFire();
    }

    void Update()
    {

        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, recoilReturnRate * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, recoilSnap * Time.fixedDeltaTime);
        cam.transform.localRotation = Quaternion.Euler(currentRotation);

        // so theres "GetMouseButton" and "GetMouseButtonDown". GetMouseButtonDown registers only the first time the user clicks, GetMouseButton will register every frame the user is clicking.
        if(Input.GetMouseButton(0))
            mouseDown = true;
        else
            mouseDown = false;
            if(!autoCanFire)
                autoCanFire = true;

        if(mouseDown && Time.time >= nextFireTick)
            Fire();
            Debug.Log("Firing");

    }

}
