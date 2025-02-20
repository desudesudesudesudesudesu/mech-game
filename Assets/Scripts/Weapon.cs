using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [Header("Weapon Settings")]
    public string weaponName; // Name of the weapon
    public float damage; // Damage per shot
    public float fireRate; // Shots per second
    public float range; // Maximum range of the weapon
    public int maxAmmo; // Maximum ammo capacity
    public int currentAmmo; // Current ammo count
    public float reloadTime; // Time to reload the weapon
    public bool isAutomatic; // Can the weapon fire continuously?
    public bool isHitscan; // Is the weapon hitscan or projectile?

    [Header("Projectile Settings")]
    public GameObject projectilePrefab; // Prefab for projectile weapons
    public float projectileSpeed; // Speed of the projectile

    [Header("Effects")]
    public ParticleSystem muzzleFlash; // Muzzle flash effect
    public AudioClip fireSound; // Sound played when firing
    public AudioClip reloadSound; // Sound played when reloading

    private float nextFireTime; // Time when the weapon can fire again

    // Abstract methods for firing and reloading
    public abstract void Fire();
    public abstract void Reload();

    // Common method to handle firing logic
    protected void TryFire()
    {
        if (Time.time >= nextFireTime && currentAmmo > 0)
        {
            nextFireTime = Time.time + 1f / fireRate; // Set the next fire time
            currentAmmo--;

            // Play muzzle flash and sound
            if (muzzleFlash != null) muzzleFlash.Play();
            if (fireSound != null) AudioSource.PlayClipAtPoint(fireSound, transform.position);

            // Handle hitscan or projectile firing
            if (isHitscan)
            {
                FireHitscan();
            }
            else
            {
                FireProjectile();
            }
        }
        else if (currentAmmo <= 0)
        {
            Reload();
        }
    }

    // Hitscan firing logic
    private void FireHitscan()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range))
        {
            // Handle hit detection (e.g., apply damage to the target)
            Debug.Log($"Hit: {hit.collider.name} for {damage} damage");
            Debug.DrawLine(ray.origin, hit.point, Color.white, 1f);

            // Example: Apply damage to a target with a Health component
            Health targetHealth = hit.collider.GetComponent<Health>();
            if (targetHealth != null)
            {
                targetHealth.TakeDamage(damage);
            }
        }
    }

    // Projectile firing logic
    private void FireProjectile()
    {
        if (projectilePrefab != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.linearVelocity = transform.forward * projectileSpeed;
            }

            //Example: Add a script to the projectile to handle damage on collision
            Rocket projectileScript = projectile.GetComponent<Rocket>();
            if (projectileScript != null)
            {
                projectileScript.damage = damage;
            }
            
        }
    }
}