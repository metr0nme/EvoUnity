using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{

    public static EventManager current;

    private void Awake()
    {
        current = this;
    }

    public event Action<Transform, string> onWeaponAdd;
    public void WeaponAdd(Transform player, string weaponName)
    {
        if(onWeaponAdd != null)
            onWeaponAdd(player, weaponName);
    }

}
