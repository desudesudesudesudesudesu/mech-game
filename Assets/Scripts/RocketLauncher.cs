using UnityEngine;
public class RocketLauncher : Weapon
{
    private void Start()
    {
        weaponName = "Rocket Launcher";
        damage = 50f;
        fireRate = 1f;
        range = 100f;
        maxAmmo = 5;
        currentAmmo = maxAmmo;
        reloadTime = 5f;
        isAutomatic = false;
        isHitscan = false;
    }

    public override void Fire()
    {
        TryFire();
    }

    public override void Reload()
    {
        Debug.Log("Reloading Rocket Launcher...");
        currentAmmo = maxAmmo;
    }
}