using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeClass : WeaponBaseClass
{

    public override void Fire()
    {
        
        /**if (currentMagSize <= 0 || reloading)
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
        setCamRotFire();**/

        Debug.Log("this bitch is working!");

    }

}
