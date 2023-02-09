using UnityEngine;

public class WeaponControllerScript : MonoBehaviour
{
    public int selectedWeapon = 0;
    public Transform equippedWeapon;

    private float startTime;
    private bool firstSelect = false;

    void Start()
    {   
        startTime = Time.time + 0.2f;
    }

    void Update()
    {

        if(Time.time < startTime) {return;}
        if(!firstSelect)
        {
            firstSelect = true;
            SelectWeapon();
        }

        int previousSelectedWeapon = selectedWeapon;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            if (selectedWeapon >= transform.childCount - 1)
                selectedWeapon = 0;
            else
                selectedWeapon++; 

        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            if (selectedWeapon <= 0)
                selectedWeapon = transform.childCount - 1;
            else
                selectedWeapon--;

        if(Input.GetKeyDown(KeyCode.Alpha1))
            selectedWeapon = 0;

        if (Input.GetKeyDown(KeyCode.Alpha2))
            selectedWeapon = 1;

        if (previousSelectedWeapon != selectedWeapon)
            SelectWeapon();

    }

    void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            
            GameObject go = weapon.gameObject;

            if(weapon.childCount <= 0)
            {
                go.SetActive(false);
                continue;
            }
            
            Transform child = weapon.GetChild(0);
            WeaponBaseClass wbc = child.GetComponent<WeaponBaseClass>();

            if (i == selectedWeapon)
            {
                go.SetActive(true);
                wbc.Equip();
                equippedWeapon = weapon;
            }
            else
            {
                if(go.activeSelf)
                {
                    go.SetActive(false);
                    wbc.Unequip();
                }
            }
            
            i++;
        }
    }
}
