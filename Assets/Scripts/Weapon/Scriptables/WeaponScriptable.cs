using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Scripts/WeaponScriptables/WeaponScriptableObject", order = 1)]
public class WeaponScriptable : ScriptableObject
{
    public string weaponName;
    public string weaponInventoryType;
    public bool automatic;
    public int magazineSize;
    public int totalAmmo;
    public float fireRate;
    public float reloadRate;
    public float recoilResetRate;
    public float recoilSnap;
    public float recoilReturnRate;
    public Vector3 recoilValue;
    public Vector2[] sprayPattern;
    public Vector3 viewmodelOffset = new Vector3(0.19f, -0.17f, 0.6f);
}
