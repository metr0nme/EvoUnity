using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBaseClass : MonoBehaviour
{

    // Script Var
    private Camera cam;
    private Animator armsAnimator;
    private Animator playerAnimator;
    private CharacterVariables charvar;

    public GameObject player;
    public GameObject playerCam;
    [SerializeField] public GameObject FirePoint;
    [SerializeField] public GameObject BulletTemplate;
    
    // WeaponScriptables Var
    public WeaponScriptable weaponValues;
    private float recoilSnap;
    private float recoilReturnRate;
    private Vector3 recoilValue;
    private float fireRate;
    private float reloadRate;
    private float recoilResetRate;
    private int magSize;
    private int totalAmmo;
    private bool automatic;
    private Vector2[] sprayPattern;
    private Vector3 vmOffset;

    // Changing (Mut) Var
    public int currentMagSize;
    public int currentTotalAmmo;
    private int currentBullet;
    private float nextFireTick;
    private float nextRecoilResetTick;
    private bool mouseDown;
    private bool autoCanFire;
    private bool reloading;
    private Vector2 currRecoilVal;
    private SpringVector3 swaySpring;

    void Start()
    {
        charvar = player.GetComponent<CharacterVariables>();
        playerCam = charvar.Camera;
        cam = playerCam.GetComponent<CameraVar>().MainCamera.GetComponent<Camera>();
        armsAnimator = charvar.Arms.GetComponent<Animator>();
        playerAnimator = charvar.playerAnimator;

        recoilSnap = weaponValues.recoilSnap;
        recoilReturnRate = weaponValues.recoilReturnRate;
        recoilValue = weaponValues.recoilValue;
        fireRate = weaponValues.fireRate;
        magSize = weaponValues.magazineSize;
        totalAmmo = weaponValues.totalAmmo;
        automatic = weaponValues.automatic;
        sprayPattern = weaponValues.sprayPattern;
        recoilResetRate = weaponValues.recoilResetRate;
        reloadRate = weaponValues.reloadRate;
        vmOffset = weaponValues.viewmodelOffset;
        mouseDown = false;
        autoCanFire = false;
        nextFireTick = Time.time;
        nextRecoilResetTick = Time.time;
        currentMagSize = magSize;
        currentTotalAmmo = totalAmmo;

        swaySpring = new SpringVector3()
        {
            StartValue = Vector3.zero,
            EndValue = Vector3.zero,
            Damping = 6,
            Stiffness = 6
        };

    }

    void setCurrentBullet()
    {
        if(Time.time >= nextRecoilResetTick)
            currentBullet = 0;
        else
            currentBullet++;
        
        nextRecoilResetTick = Time.time + recoilResetRate;
    }

    void setCamRotFire()
    {
        currRecoilVal = sprayPattern[currentBullet];
        float[] va = {-currRecoilVal.x, Random.Range(-currRecoilVal.y, currRecoilVal.y)};
        ClientEventManager.current.FireShake(va);
    }

    private float absValueRandom(float value)
    {
        float newValue;
        float[] floats = {1f, -1f};
        var randomIndex = Random.Range(0, floats.Length);
        newValue = value * floats[randomIndex];
        return newValue;
    }

    void Fire()
    {
        if (currentMagSize <= 0 || reloading)
            return;
        
        if(!automatic) // automatic weapon registration
            if(!autoCanFire)
                return;
            else
                autoCanFire = false;
        
        setCurrentBullet();
        currentMagSize--;
        nextFireTick = Time.time + fireRate;

        // this will cast a ray and give you the result ( this is where you can apply damage or do whatever :3 )
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3 targetPos;
        if(Physics.Raycast(ray, out hit, 100))
            targetPos = hit.point;
        else   
            targetPos = ray.GetPoint(75);

        // play fire animation
        //playerAnimator.SetTrigger("Shoot"); // server
        armsAnimator.SetTrigger("Fire"); // client
        
        ClientEventManager.current.VMFireShake(new Vector3(absValueRandom(Random.Range(0.05f, 0.15f)), Random.Range(0.2f, 0.3f), -Random.Range(0.45f, 0.55f)));
        CreateFakeBullet(targetPos);
        setCamRotFire();
    }

    void ReloadUpdateVar()
    {
        int need = magSize - currentMagSize;

        if(need == 0)
            return;
        
        if((currentTotalAmmo - need) <= 0)
        {
            currentMagSize += currentTotalAmmo;
            currentTotalAmmo = 0;
        } else {
            currentMagSize = magSize;
            currentTotalAmmo -= need;
        }
        
        reloading = false;
    }

    void Reload()
    {

        if(reloading || currentTotalAmmo <= 0)
            return;

        Invoke("ReloadUpdateVar", reloadRate); // Invoke is one of Unity's Multi-Threading operations! This will delay the task the set time, and run the rest of the code while this function is delayed.
        
    }

    void CreateFakeBullet(Vector3 hitPos)
    {
        Vector3 dir = hitPos - FirePoint.transform.position;
        GameObject newBullet = Instantiate(BulletTemplate, FirePoint.transform.position, Quaternion.identity);
        newBullet.GetComponent<Rigidbody>().AddForce(dir.normalized * 100, ForceMode.Impulse);
        Destroy(newBullet, 0.5f);
    }

    void OldHandleMouseSway()
    {
        // handle sway spring
        //float mouseX = Input.GetAxisRaw("Mouse X") * 5; //TODO: replace mousex, mousey with MouseDelta
        //float mouseY = Input.GetAxisRaw("Mouse Y") * 5;
        //swaySpring.InitialVelocity = new Vector3(-mouseX / 5, mouseY / 2, 0);
        //transform.localPosition += swaySpring.Evaluate(Time.fixedDeltaTime); // update sway spring pos to be added to other springs
    }

    void Update()
    {

        if(Input.GetMouseButton(0)) // so theres "GetMouseButton" and "GetMouseButtonDown". GetMouseButtonDown registers only the first time the user clicks, GetMouseButton will register every frame the user is clicking.
            mouseDown = true;
        else
            mouseDown = false;
            if(!autoCanFire)
                autoCanFire = true;

        if(mouseDown && Time.time >= nextFireTick)
            Fire();

        if(Input.GetKey(KeyCode.R))
            Reload();

    }

    public void Equip()
    {
        //animator.SetTrigger("Equip");
        transform.localPosition = vmOffset;
        armsAnimator.SetTrigger("AK" + "Equip");
        playerAnimator.SetTrigger("AKEquip");
    }

    public void Unequip()
    {
        armsAnimator.SetTrigger("Unequip");
        playerAnimator.SetTrigger("Unequip");
    }

}
