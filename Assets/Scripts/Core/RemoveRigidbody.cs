using UnityEngine;
using System.Collections;
using UnityEditor.Animations;
using Unity.VisualScripting;

public class RemoveRigidbody : MonoBehaviour
{

    private Rigidbody[] childArray;

    private void Start()
    {
        childArray = gameObject.GetComponentsInChildren<Rigidbody>();
        CoroutineManager.Instance.StartCoroutine(FreezePhysics(60f, childArray));
    }
    public IEnumerator FreezePhysics(float delay, Rigidbody[] children)
    {
        yield return new WaitForSeconds(delay);

        foreach (Rigidbody child in children) {
            if (child.linearVelocity == Vector3.zero)
            {
                Destroy(child);
            }
            else {
                CoroutineManager.Instance.StartCoroutine(DeleteRigidbody(10f, child)); //start a recursive coroutine that will check so often to destroy rb
            }
        }

    }


    public IEnumerator DeleteRigidbody(float delay, Rigidbody rb) {
        yield return new WaitForSeconds(delay);
        if (rb.linearVelocity == Vector3.zero)
        {
            Destroy(rb);
        }
        else { CoroutineManager.Instance.StartCoroutine(DeleteRigidbody(10f, rb)); }
    }

}
