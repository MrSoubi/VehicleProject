using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] private float rocketForceHorizontal;
    [SerializeField] private float rocketForceUp;

    [SerializeField] private Transform startPosition;
    [SerializeField] private Transform endPosition;
    [SerializeField] private float duration;

    private void Update()
    {
        RocketMovement();
    }
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
        float timeDuration = Time.time / duration;
        transform.position = Vector3.Lerp(startPosition.position, endPosition.position, timeDuration);

        if(Vector3.Distance(transform.position, endPosition.position) <= 0.1f)
        {
            Destroy(gameObject);
        }
    }
}
