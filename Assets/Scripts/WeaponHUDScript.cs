using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponHUDScript : MonoBehaviour
{

    public GameObject player;
    private Transform gunController;
    //private WeaponHandler weaponHandler;
    private WeaponBaseClass baseClass;

    public Text totalAmmoText;
    public Text magazineSizeText;

    private Transform equippedWeapon;

    void updateWeaponVar()
    {
        baseClass = equippedWeapon.GetComponent<WeaponBaseClass>();
    }

    void Start()
    {
        gunController = player.transform.Find("Head/Camera/GunController");
        //weaponHandler = gunController.GetComponent<WeaponHandler>();
    }

    void Update()
    {

        //if (weaponHandler.equippedWeapon == null)
            //return;

        //if (equippedWeapon != weaponHandler.equippedWeapon)
            //equippedWeapon = weaponHandler.equippedWeapon;
            //updateWeaponVar();

        totalAmmoText.text = baseClass.currentTotalAmmo.ToString();
        magazineSizeText.text = baseClass.currentMagSize.ToString();

    }
}