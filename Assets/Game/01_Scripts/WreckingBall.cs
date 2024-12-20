using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WreckingBall : MonoBehaviour
{
    [SerializeField] private float wreckingBallForceHorizontal;
    [SerializeField] private float wreckingBallForceUp;
    [SerializeField] private List<Transform> ballPointDirection;
    [SerializeField] private float ballSpeed;

    [SerializeField] LayerMask layerMask;

    private Transform target, lastTarget;
    private int targetIndex;

    private void Start()
    {
        targetIndex = 0;
        target = ballPointDirection[targetIndex];
        lastTarget = target;
        targetIndex++;
        target = ballPointDirection[targetIndex];
        transform.position = target.position;
        SetPositionOnGround();
    }
    Vector3 midPoint;
    public float magicNumber = 10;
    private void Update()
    {
        Vector3 direction = (target.position - transform.position).normalized.ProjectOntoPlane(GetFloorNormal());

        midPoint = lastTarget.position + (target.position - lastTarget.position) / 2;
        float distanceToCenter = (transform.position - midPoint).magnitude;
        float speedFactor = (magicNumber/distanceToCenter);

        transform.position += ballSpeed * speedFactor * Time.deltaTime * direction;

        if ((transform.position - target.position).magnitude < 15)
        {
            targetIndex = (targetIndex + 1) % ballPointDirection.Count;
            lastTarget = target;
            target = ballPointDirection[targetIndex];
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

    void SetPositionOnGround()
    {
        transform.position = GetFloorPosition() + GetFloorNormal() * transform.localScale.x / 2;
    }

    Vector3 GetFloorNormal()
    {
        RaycastHit hit;

        Physics.Raycast(transform.position, -transform.up, out hit, Mathf.Infinity, layerMask);

        return hit.normal;
    }

    Vector3 GetFloorPosition()
    {
        RaycastHit hit;

        Physics.Raycast(transform.position, -transform.up, out hit, Mathf.Infinity, layerMask);

        return hit.point;
    }
}
