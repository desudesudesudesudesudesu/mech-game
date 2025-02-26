using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    public WeaponData weaponData;
    private Rigidbody rb;

    private float _debugSphereTimer = 1f; //debug purposes

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


        //spawn the FX -- the prefab has it's own script to return to pool since this gameobject will destroy itself with the coroutine it seems
        GameObject projectileExplosion = ObjectPool.Instance.SpawnFromPool(
            weaponData.projectileExplosion.name,
            transform.position, transform.rotation.normalized);
        

        //Simple explosion 
        if (weaponData.explosionRadius > 0)
        {
            StartCoroutine(ShowExplosionRadiusDebug()); //DEBUG SPHERE GENERATION
            Collider[] hits = Physics.OverlapSphere(transform.position, weaponData.explosionRadius);
            foreach (Collider hit in hits)
            {

                if (hit.TryGetComponent<Health>(out Health hitHealth))
                {
                    hitHealth.TakeDamage(weaponData.damage * 0.5f);
                }

            }
        }

        
        // Return to pool on hit
        gameObject.
        gameObject.SetActive(false);
    }


    /* DEBUG STUFF BELOW 
     -----------------------------------------------------------------------------------------------------------------------------------------------------------------------
     */


    private IEnumerator ShowExplosionRadiusDebug()
    {
        _debugSphereTimer = 1f; // Duration in seconds

        while (_debugSphereTimer > 0)
        {
            // Draw the sphere in red with decreasing alpha
            Debug.DrawRay(transform.position, Vector3.up * weaponData.explosionRadius,
                        new Color(1, 0, 0, _debugSphereTimer));

            // Draw wire sphere using debug lines
            DrawDebugSphere(transform.position, weaponData.explosionRadius,
                          new Color(1, 0, 0, _debugSphereTimer));

            _debugSphereTimer -= Time.deltaTime;
            yield return null;
        }
    }

    // Helper to draw a wire sphere using Debug.DrawLine
    private void DrawDebugSphere(Vector3 center, float radius, Color color)
    {
        float angleStep = 10f;

        // Draw 3 orthogonal circles
        DrawDebugCircle(center, radius, Vector3.up, Vector3.forward, angleStep, color);
        DrawDebugCircle(center, radius, Vector3.right, Vector3.forward, angleStep, color);
        DrawDebugCircle(center, radius, Vector3.up, Vector3.right, angleStep, color);
    }

    private void DrawDebugCircle(Vector3 center, float radius, Vector3 axis1, Vector3 axis2,
                               float angleStep, Color color)
    {
        Vector3 prevPoint = center + axis1 * radius;
        for (float angle = angleStep; angle <= 360f; angle += angleStep)
        {
            float radians = Mathf.Deg2Rad * angle;
            Vector3 nextPoint = center +
                axis1 * (Mathf.Cos(radians) * radius) +
                axis2 * (Mathf.Sin(radians) * radius);

            Debug.DrawLine(prevPoint, nextPoint, color, 0.1f);
            prevPoint = nextPoint;
        }
    }
























}