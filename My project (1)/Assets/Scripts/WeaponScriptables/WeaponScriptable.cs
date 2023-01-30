using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Scripts/WeaponScriptables/WeaponScriptableObject", order = 1)]
public class WeaponScriptable : ScriptableObject
{
    public string weaponName;
    public int magazineSize;
    public int totalAmmo;
    public float fireRate;
    public float recoilSnap;
    public float recoilReturnRate;
    public Vector3 recoilValue;
}
