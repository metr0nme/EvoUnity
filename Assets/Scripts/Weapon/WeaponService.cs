using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class WeaponService : MonoBehaviour
{

    [Header("Weapons")]
    [SerializeField] public WeaponScriptable Scriptable_AK;
    [SerializeField] public GameObject GameObject_AK;

    // WEAPON LISTS
    private IDictionary<string, WeaponScriptable> WeaponScriptables;
    private IDictionary<string, GameObject> WeaponPrefabs;

    void Start()
    {

        // Init Script Var
        WeaponScriptables = new Dictionary<string, WeaponScriptable>();
        WeaponScriptables["AK"] = Scriptable_AK;

        WeaponPrefabs = new Dictionary<string, GameObject>();
        WeaponPrefabs["AK"] = GameObject_AK;

        EventManager.current.onWeaponAdd += AddWeapon;
    }

    public void AddWeapon(Transform player, string weaponName)
    {

        Transform weaponController = player.GetComponent<CharacterVariables>().WeaponController;
        WeaponScriptable weaponScriptable = WeaponScriptables[weaponName];
        GameObject weaponPrefab = WeaponPrefabs[weaponName];
        string weaponType = weaponScriptable.weaponInventoryType;

        GameObject newPrefab = Instantiate(weaponPrefab);
        WeaponBaseClass baseClass = newPrefab.GetComponent<WeaponBaseClass>();

        baseClass.player = player.gameObject;
        baseClass.playerCam = player.Find("Head/Camera").gameObject;

        newPrefab.transform.parent = weaponController.Find(weaponType);
        newPrefab.transform.localPosition = weaponScriptable.viewmodelOffset;

    }

}
