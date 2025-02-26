using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    public GameObject wreckage;
    public GameObject child; //used for buildings to destroy the next building, recursive beware
    public float childDeathDelay = 1f;
    
    

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage. Current health: {currentHealth}");
        if (currentHealth < 0)
        { currentHealth = 0; }
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public IEnumerator DieAfterTime(float delay)
    {
        gameObject.AddComponent<Rigidbody>();
        gameObject.GetComponent<Rigidbody>().mass = 50;
        yield return new WaitForSeconds(delay);
        gameObject.GetComponent<Health>().Die();
        //Die();
    }
    private void Die()
    {
        
        if (wreckage != null) {
            Instantiate(wreckage, transform.position, transform.rotation);
        }
        if (child != null) {
            CoroutineManager.Instance.StartCoroutine(child.GetComponent<Health>().DieAfterTime(childDeathDelay));
            
            
        }

        Destroy(gameObject);
        Debug.Log($"{gameObject.name} has been destroyed.");
    }
}