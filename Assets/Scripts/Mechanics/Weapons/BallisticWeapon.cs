using UnityEngine;
using System.Collections;

public class BallisticWeapon : MonoBehaviour
{
    public WeaponData weaponData;
    public Transform firePoint;
    private float _fireTimer;

    void Update()
    {
        // Handle cooldown
        _fireTimer -= Time.deltaTime;

        // Check for continuous fire
        if (Input.GetMouseButton(0) && _fireTimer <= 0)
        {
            Fire();
            _fireTimer = weaponData.fireRate; // Reset cooldown
        }
    }

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

    void ApplyRecoil()
    {
        GetComponentInParent<Rigidbody>().AddForce(
            -firePoint.forward * weaponData.recoilForce,
            ForceMode.Impulse
        );
    }
}