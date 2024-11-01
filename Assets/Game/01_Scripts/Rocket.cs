using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Rocket : MonoBehaviour
{
    [SerializeField] private float rocketForceHorizontal;
    [SerializeField] private float rocketForceUp;

    [SerializeField] private float endPosition;
    [SerializeField] private float moveDuration;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CarController>() != null)
        {
            Vector3 dir = Vector3.ProjectOnPlane((other.transform.position - transform.position), Vector3.up).normalized;
            Rigidbody otherRB = other.GetComponent<Rigidbody>();
            otherRB.AddForce(dir * rocketForceHorizontal, ForceMode.Impulse);
            otherRB.AddForce(Vector3.up * rocketForceUp, ForceMode.Impulse);
            otherRB.AddTorque(Vector3.zero);
        }
    }

    public void RocketMovement()
    {
        transform.DOMoveY(endPosition, moveDuration).OnComplete(DestroyRocket);
    }

    void DestroyRocket()
    {
        Destroy(gameObject);
    }
}
