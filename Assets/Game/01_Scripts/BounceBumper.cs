using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceBumper : MonoBehaviour
{
    [SerializeField] private float bounceForce;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<CarController>() != null)
        {
            Rigidbody otherRB = collision.rigidbody;

            otherRB.AddExplosionForce(bounceForce, collision.contacts[0].point, 5);
        }
    }
}
