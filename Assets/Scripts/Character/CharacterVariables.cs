using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterVariables : MonoBehaviour
{

    private string weaponName = "AK";

    [SerializeField] public Transform WeaponController;
    [SerializeField] public GameObject Camera;
    public bool IsLoaded = false;

    void Start()
    {
        IsLoaded = true;
        EventManager.current.WeaponAdd(transform, weaponName);
    }

}
