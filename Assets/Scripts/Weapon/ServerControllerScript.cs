using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerControllerScript : MonoBehaviour
{

    [SerializeField] public Transform playerTransform;

    private CharacterVariables charvar;
    private Transform weaponController;

    void Start()
    {
        charvar = playerTransform.GetComponent<CharacterVariables>();
        weaponController = charvar.WeaponController;
    }

    void Update()
    {
        ReplicateFromClientController();
    }

    void ReplicateFromClientController()
    {
        int i = 0;
        if(weaponController.childCount < i+1) {return;}
        foreach(Transform wct in weaponController)
        {
            GameObject child = transform.GetChild(i).gameObject;
            bool clientIsActive = wct.gameObject.activeSelf;
            if(clientIsActive != child.activeSelf)
                child.SetActive(clientIsActive);
            i++;
        }
    }

}
