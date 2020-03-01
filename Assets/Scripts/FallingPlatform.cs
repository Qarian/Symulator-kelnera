using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] private int jumpToFallPlatform = 10;
    [SerializeField] private float timeToDestroyPlatform = 4f;

    private Rigidbody rb = default;

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerMovementWallRun player))
        {
            if (jumpToFallPlatform > 0)
            {
                jumpToFallPlatform--;
            }
            
            if (jumpToFallPlatform == 0)
            {
                FallPlatform();
                StartCoroutine(DestroyObject(timeToDestroyPlatform));
            }
        }
    }

    private void FallPlatform()
    {
        rb = gameObject.AddComponent<Rigidbody>() as Rigidbody;
    }

    private IEnumerator DestroyObject(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
