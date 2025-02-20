using UnityEngine;
public class MachineGun : Weapon
{
    private void Start()
    {
        weaponName = "Machine Gun";
        damage = 5f;
        fireRate = 8f;
        range = 200f;
        maxAmmo = 100;
        currentAmmo = maxAmmo;
        reloadTime = 3f;
        isAutomatic = true;
        isHitscan = true;
    }

    public override void Fire()
    {
        TryFire();
    }

    public override void Reload()
    {
        Debug.Log("Reloading Machine Gun...");
        currentAmmo = maxAmmo;
    }
}