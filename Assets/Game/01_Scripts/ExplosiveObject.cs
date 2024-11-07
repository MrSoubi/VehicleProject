using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveObject : MonoBehaviour
{
    [SerializeField] private bool useDetonateV2;
    [SerializeField] private float rangeOfExplosion;
    [SerializeField] private float delayBeforeExplosion;
    [SerializeField] private float forceOfExplosion;
    [SerializeField] private float upForce;
    [SerializeField] private ParticleSystem particleExplosion;
    [SerializeField] private Animator animBomb;
    [SerializeField] private AnimationCurve curve;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<CarController>() != null)
        {
            animBomb.SetBool("isContact", true);
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
        Instantiate(particleExplosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    void DetonateV2()
    {
        Vector3 explosionPosition = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, rangeOfExplosion);

        foreach (Collider hit in colliders)
        {
            Rigidbody rb;
            
            if (hit.TryGetComponent<Rigidbody>(out rb))
            {
                Vector3 direction = (rb.transform.position - explosionPosition).normalized;
                float distanceToTarget = (rb.transform.position - explosionPosition).magnitude;
                float forceRatio = Mathf.Min(distanceToTarget / rangeOfExplosion, 1);
                float force = forceOfExplosion * curve.Evaluate(forceRatio);
                rb.AddForce(force * direction, ForceMode.Impulse);
                rb.AddForce(Vector3.up * upForce, ForceMode.Impulse);
            }
        }
        Instantiate(particleExplosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    void AnimBomb()
    {
        animBomb.Play("anim_Bomb_Idle", 0, 0.0f);
    }
    IEnumerator DelayExplosion(float delay)
    {
        AnimBomb();
        yield return new WaitForSeconds(delay);
        if (useDetonateV2)
        {
            DetonateV2();
        }
        else
        {
            Detonate();
        }
        
    }

    public void SpawnDetonate()
    {
        animBomb.SetBool("isContact", true);
        StartCoroutine(DelayExplosion(delayBeforeExplosion));
    }
}
