using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceBumper : MonoBehaviour
{
    [SerializeField] private float bounceForce;
    [SerializeField] private float bounceRadius;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<CarController>() != null)
        {
            Rigidbody otherRB = collision.rigidbody;
             float forceBumper = otherRB.velocity.magnitude;

            otherRB.AddExplosionForce(bounceForce * forceBumper, transform.position, bounceRadius);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, bounceRadius);
    }
}
