using UnityEngine;

public class Rocket : MonoBehaviour
{
    [Header("Projectile Settings")]
    public float speed = 20f; // Speed of the projectile
    public float damage = 50f; // Damage dealt on impact
    public float lifetime = 5f; // Time before the projectile is destroyed
    public GameObject explosionEffect; // Explosion effect prefab

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Projectile requires a Rigidbody component!");
            return;
        }

        // Set the projectile's velocity
        rb.linearVelocity = transform.forward * speed;

        // Destroy the projectile after its lifetime expires
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Apply damage to the target
        Health targetHealth = other.GetComponent<Health>();
        if (targetHealth != null)
        {
            targetHealth.TakeDamage(damage);
        }

        // Spawn explosion effect
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, transform.rotation);
        }

        // Destroy the projectile on impact
        Destroy(gameObject);
    }
}