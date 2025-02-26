using UnityEngine;

public class SelfPool : MonoBehaviour
{
    public float AutoPoolTimer;
    System.Collections.IEnumerator ReturnToPoolAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
    }
    private void OnEnable()
    {
        StartCoroutine(ReturnToPoolAfterDelay(gameObject, AutoPoolTimer));
    }
}
