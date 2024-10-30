using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WreckingBall : MonoBehaviour
{
    [SerializeField] private float wreckingBallForceHorizontal;
    [SerializeField] private float wreckingBallForceUp;
    [SerializeField] private List<Transform> ballPointDirection;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float ballSpeed;

    private Transform target;
    private int targetIndex;

    private void Start()
    {
        targetIndex = 0;
        target = ballPointDirection[targetIndex];
    }

    private void Update()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        rb.velocity = direction * ballSpeed;

        for (int i = 0; i < ballPointDirection.Count; i++)
        {
            if(Vector3.Distance(target.position, transform.position) <= 0.1f)
            {
                if(targetIndex >= ballPointDirection.Count)
                {
                    targetIndex = 0;
                    target = ballPointDirection[targetIndex];
                }
                else
                {
                    targetIndex++;
                    target = ballPointDirection[targetIndex];
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CarController>() != null)
        {
            Vector3 dir = Vector3.ProjectOnPlane((other.transform.position - transform.position), Vector3.up).normalized;
            Rigidbody otherRB = other.GetComponent<Rigidbody>();
            otherRB.AddForce(dir * wreckingBallForceHorizontal, ForceMode.Impulse);
            otherRB.AddForce(Vector3.up * wreckingBallForceUp, ForceMode.Impulse);
            otherRB.AddTorque(Vector3.zero);
        }
    }
}
