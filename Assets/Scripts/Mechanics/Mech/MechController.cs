using UnityEngine;

public class MechController : MonoBehaviour
{

    [SerializeField] private BallisticWeapon[] _primaryWeapons;
    [SerializeField] private BallisticWeapon[] _secondaryWeapons;
    

    /*
    [SerializeField] private MechData _mechConfig;
    [SerializeField] private WeaponData[] _primaryWeapons;
    [SerializeField] private WeaponData[] _secondaryWeapons;

    private void Start()
    {
        InitializeWeapons();
    }

    private void InitializeWeapons()
    {
        // Spawn primary weapons
        for (int i = 0; i < _mechConfig.primaryWeaponCount && i < _primaryWeapons.Length; i++)
        {
            SetupWeapon(_primaryWeapons[i], "PrimaryWeaponSlot_" + i);
        }

        // Spawn secondary weapons
        for (int i = 0; i < _mechConfig.secondaryWeaponCount && i < _secondaryWeapons.Length; i++)
        {
            SetupWeapon(_secondaryWeapons[i], "SecondaryWeaponSlot_" + i);
        }
    }

    private void SetupWeapon(WeaponData config, string slotName)
    {
        GameObject weapon = new GameObject(config.weaponName);
        weapon.transform.SetParent(transform.Find(slotName));
        BallisticWeapon weaponScript = weapon.AddComponent<BallisticWeapon>();
        weaponScript.Initialize(config);
    }
    */

    private void Update()
    {      

        // Check for continuous fire on Left mouse for Primary, Right mouse for Secondary 
        if (Input.GetMouseButton(0))
        {
            foreach (BallisticWeapon weapon in _primaryWeapons)
            {
                weapon.Fire();                
                
                //if (weapon.isLeftGun) { UIManager.Instance.SetLeftAmmo(ammoCount, 1000); } else { UIManager.Instance.SetRightAmmo(ammoCount, 1000); } // ugly stuff
            }
        }
        if (Input.GetMouseButton(1))
        {
            foreach (BallisticWeapon weapon in _secondaryWeapons) {
                weapon.Fire();
            }
        }
    }
}

