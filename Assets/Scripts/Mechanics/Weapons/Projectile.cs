using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    public WeaponData weaponData;
    private Rigidbody rb;

    void Awake() => rb = GetComponent<Rigidbody>();

    void OnEnable()
    {
        // Reset physics state when spawned
        rb.linearVelocity = Vector3.zero;        
        rb.angularVelocity = Vector3.zero;
    }

    public void Launch(Vector3 direction)
    {
        rb.AddForce(direction * weaponData.projectileSpeed, ForceMode.VelocityChange);
        StartCoroutine(DeactivateAfterLifetime(5f));
    }

    IEnumerator DeactivateAfterLifetime(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }

    void OnCollisionEnter(Collision collision)
    {        
        /*Simple explosion 
        if (weaponData.explosionRadius > 0)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, weaponData.explosionRadius);
            foreach (Collider hit in hits)
            {

                if (hit.TryGetComponent<Health>(out Health hitHealth))
                {
                    hitHealth.TakeDamage(weaponData.damage * 0.5f);
                }

            }
        }*/

        // Return to pool on hit
        gameObject.SetActive(false);
    }






























}