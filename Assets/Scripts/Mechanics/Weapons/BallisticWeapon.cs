using UnityEngine;
using System.Collections;

public class BallisticWeapon : MonoBehaviour
{
    public WeaponData weaponData;
    public Transform firePoint;
    public Transform shellEjectPoint;
    public bool isLeftGun; //dirty way to figure out if the gun is on the left or right of the mech to handle ammo independently for no apparant reason
    private float _fireTimer;
    private int ammoCount;


    private void Awake()
    {
        ammoCount = weaponData.ammoCount;
        //if (isLeftGun) { UIManager.Instance.SetLeftAmmo(ammoCount, 1000); } else { UIManager.Instance.SetRightAmmo(ammoCount, 1000); } // ugly stuff
    }
    void Update()
    {
        // Handle cooldown
        _fireTimer -= Time.deltaTime;
    }

    /*
     * public void Initialize(WeaponData config)
    {
       weaponData = config;
    }
    */

    public void Fire()
    {
        if (ammoCount > 0 && _fireTimer <= 0)
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

            if (weaponData.bulletShellPrefab != null)
            {
                GameObject bulletShell = ObjectPool.Instance.SpawnFromPool(
                    weaponData.bulletShellPrefab.name,
                    shellEjectPoint.position,
                    shellEjectPoint.rotation
                );
                StartCoroutine(ReturnToPoolAfterDelay(muzzleFlash, 10f));
            }

            ammoCount -= 1;
            _fireTimer = weaponData.fireRate;
            // Recoil
            // ApplyRecoil();
        }
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