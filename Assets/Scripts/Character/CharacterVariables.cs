using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterVariables : MonoBehaviour
{

    private string weaponName = "AK";

    [SerializeField] public Transform WeaponController;
    [SerializeField] public Transform ServerWeaponController;
    [SerializeField] public GameObject Camera;
    [SerializeField] public Transform Arms;
    [SerializeField] public Animator playerAnimator;
    [SerializeField] public Transform playerModel;
    public bool IsLoaded = false;

    void Start()
    {
        IsLoaded = true;
    }

}
