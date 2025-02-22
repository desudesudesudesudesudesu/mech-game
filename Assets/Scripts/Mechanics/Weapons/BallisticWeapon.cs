using UnityEngine;
using System.Collections;

public class BallisticWeapon : MonoBehaviour
{
    public WeaponData weaponData;
    public Transform firePoint;    
    private float _fireTimer;
    private int ammoCount;


    private void Awake()
    {
        ammoCount = weaponData.ammoCount;
    }
    void Update()
    {
        // Handle cooldown
        _fireTimer -= Time.deltaTime;

        // Check for continuous fire on Left mouse for Primary, Right mouse for Secondary 
        if (Input.GetMouseButton(0) && _fireTimer <= 0 && weaponData.weaponSlot.ToString() == "Primary" && ammoCount > 0)
        {
            Fire();
            ammoCount -= 1;
            _fireTimer = weaponData.fireRate; // Reset cooldown
        }
        if (Input.GetMouseButton(1) && _fireTimer <= 0 && weaponData.weaponSlot.ToString() == "Secondary" && ammoCount > 0)
        {
            Fire();
            _fireTimer = weaponData.fireRate; // Reset cooldown
            ammoCount -= 1;
        }
    }

    /*
     * public void Initialize(WeaponData config)
    {
       weaponData = config;
    }
    */

    void Fire()
    {
        // Spawn projectile
        GameObject projectile = ObjectPool.Instance.SpawnFromPool(
            weaponData.projectilePrefab.name,
            firePoint.position,
            firePoint.rotation
        );
        Projectile projectileScript = projectile.GetComponent<Projectile>();
        projectileScript.weaponData = weaponData;
        projectileScript.Launch(firePoint.forward);

        // Muzzle flash (auto-return after 0.1s)
        GameObject muzzleFlash = ObjectPool.Instance.SpawnFromPool(
            weaponData.muzzleFlashPrefab.name,
            firePoint.position,
            firePoint.rotation
        );
        StartCoroutine(ReturnToPoolAfterDelay(muzzleFlash, 0.1f));

        // Recoil
       // ApplyRecoil();
    }

    System.Collections.IEnumerator ReturnToPoolAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
    }
    /*
    void ApplyRecoil()
    {
        GetComponentInParent<Rigidbody>().AddForce(
            -firePoint.forward * weaponData.recoilForce,
            ForceMode.Impulse
        );
    }
    */
}