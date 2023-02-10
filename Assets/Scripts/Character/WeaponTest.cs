using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTest : MonoBehaviour
{
    private string weaponName = "Knife";

    void Start()
    {
        EventManager.current.WeaponAdd(transform, weaponName);
    }

}
