using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class WeaponService : MonoBehaviour
{

    [Header("Weapons")]

    [Header("AK")]
    [SerializeField] public WeaponScriptable Scriptable_AK;
    [SerializeField] public GameObject GameObject_AK;
    [SerializeField] public GameObject ServerGameObject_AK;

    [Header("Knife")]
    [SerializeField] public WeaponScriptable Scriptable_Knife;
    [SerializeField] public GameObject GameObject_Knife;
    [SerializeField] public GameObject ServerGameObject_Knife;

    // WEAPON LISTS
    private IDictionary<string, WeaponScriptable> WeaponScriptables;
    private IDictionary<string, GameObject> WeaponPrefabs;

    void Start()
    {

        // Init Script Var
        WeaponScriptables = new Dictionary<string, WeaponScriptable>();
        WeaponScriptables["AK"] = Scriptable_AK;
        WeaponScriptables["Knife"] = Scriptable_Knife;

        WeaponPrefabs = new Dictionary<string, GameObject>();
        WeaponPrefabs["AK"] = GameObject_AK;
        WeaponPrefabs["AKServer"] = ServerGameObject_AK;
        WeaponPrefabs["Knife"] = GameObject_Knife;
        WeaponPrefabs["KnifeServer"] = ServerGameObject_Knife;

        EventManager.current.onWeaponAdd += AddWeapon;
    }

    public void AddWeapon(Transform player, string weaponName)
    {
        
        // Gather Var
        CharacterVariables charvar = player.GetComponent<CharacterVariables>();
        Transform weaponController = charvar.WeaponController;
        Transform serverWeaponController = charvar.ServerWeaponController;
        WeaponScriptable weaponScriptable = WeaponScriptables[weaponName];
        GameObject weaponPrefab = WeaponPrefabs[weaponName];
        GameObject serverWeaponPrefab = WeaponPrefabs[weaponName + "Server"];
        string weaponType = weaponScriptable.weaponInventoryType;

        // Instantiate Server Model
        //GameObject serverPrefab = Instantiate(serverWeaponPrefab);
        //serverPrefab.transform.parent = serverWeaponController.Find(weaponType);
        //serverPrefab.transform.localPosition = Vector3.zero;

        // Instantiate Client Model
        GameObject newPrefab = Instantiate(weaponPrefab);
        WeaponBaseClass baseClass = newPrefab.GetComponent<WeaponBaseClass>();

        baseClass.player = player.gameObject;
        baseClass.playerCam = player.Find("Head/Camera").gameObject;

        newPrefab.transform.parent = weaponController.Find(weaponType);
        newPrefab.transform.localPosition = weaponScriptable.viewmodelOffset;

    }

}
