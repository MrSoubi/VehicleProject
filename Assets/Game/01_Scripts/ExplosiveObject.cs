using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveObject : MonoBehaviour
{
    [SerializeField] private float rangeOfExplosion;
    [SerializeField] private float delayBeforeExplosion;
    [SerializeField] private float forceOfExplosion;
    [SerializeField] private float upForce;

    private Material material;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<CarController>() != null)
        {
            StartCoroutine(DelayExplosion(delayBeforeExplosion));
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, rangeOfExplosion);
    }

    void Detonate()
    {

        Vector3 explosionPosition = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, rangeOfExplosion);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.AddExplosionForce(forceOfExplosion, explosionPosition, rangeOfExplosion, upForce, ForceMode.Impulse);
            }
        }

        Destroy(gameObject);
    }

    IEnumerator DelayExplosion(float delay)
    {
        yield return new WaitForSeconds(delay);
        Detonate();
    }

}
