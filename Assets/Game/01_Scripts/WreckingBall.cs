using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WreckingBall : MonoBehaviour
{
    [SerializeField] private float wreckingBallForceHorizontal;
    [SerializeField] private float wreckingBallForceUp;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CarController>() != null)
        {
            Debug.Log("Hit");
            Vector3 dir = Vector3.ProjectOnPlane((other.transform.position - transform.position), Vector3.up).normalized;
            Rigidbody otherRB = other.GetComponent<Rigidbody>();
            otherRB.AddForce(dir * wreckingBallForceHorizontal, ForceMode.Impulse);
            otherRB.AddForce(Vector3.up * wreckingBallForceUp, ForceMode.Impulse);
            otherRB.AddTorque(Vector3.zero);
        }
    }
}
