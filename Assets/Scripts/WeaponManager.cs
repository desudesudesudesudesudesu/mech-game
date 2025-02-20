using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("Primary Weapon Settings")]
    public Weapon primaryWeaponLeft; // Left primary weapon
    public Weapon primaryWeaponRight; // Right primary weapon

    [Header("Secondary Weapon Settings")]
    public Weapon secondaryWeaponLeft; // Left secondary weapon
    public Weapon secondaryWeaponRight; // Right secondary weapon

    private void Start()
    {
        // Ensure all weapons are assigned
        if (primaryWeaponLeft == null || primaryWeaponRight == null ||
            secondaryWeaponLeft == null || secondaryWeaponRight == null)
        {
            Debug.LogError("All weapons must be assigned in the WeaponManager!");
        }
    }

    private void Update()
    {
        // Fire primary weapons on left-click
        if (Input.GetButton("Fire1"))
        {
            primaryWeaponLeft.Fire();
            primaryWeaponRight.Fire();
        }

        // Fire secondary weapons on right-click
        if (Input.GetButton("Fire2"))
        {
            secondaryWeaponLeft.Fire();
            secondaryWeaponRight.Fire();
        }

        // Reload primary weapons on R key press
        if (Input.GetKeyDown(KeyCode.R))
        {
            primaryWeaponLeft.Reload();
            primaryWeaponRight.Reload();
        }

        // Reload secondary weapons on F key press
        if (Input.GetKeyDown(KeyCode.F))
        {
            secondaryWeaponLeft.Reload();
            secondaryWeaponRight.Reload();
        }
    }
}