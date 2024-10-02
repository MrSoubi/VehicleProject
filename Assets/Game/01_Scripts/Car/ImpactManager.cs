using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactManager : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    public Vector3 velocityOddFrame, velocityEvenFrame, lastVelocity;

    public bool isVerbose;
    bool isInvicible = false;


    private void Start()
    {
        velocityOddFrame = rb.velocity;
        velocityEvenFrame = rb.velocity;
        lastVelocity = rb.velocity;
    }

    private void FixedUpdate()
    {
        if (Time.frameCount % 2 == 0)
        {
            velocityEvenFrame = rb.velocity;
            lastVelocity = velocityOddFrame;
        }
        else
        {
            velocityOddFrame = rb.velocity;
            lastVelocity = velocityEvenFrame;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        ImpactManager otherCar;

        if (!collision.gameObject.TryGetComponent<ImpactManager>(out otherCar))
        {
            return;
        }

        if (isInvicible)
        {
            return;
        }

        isInvicible = true;

        HandleImpact(otherCar, collision);
    }

    private void HandleImpact(ImpactManager otherCar, Collision collision)
    {
        Vector3 impactVelocity = lastVelocity + otherCar.lastVelocity;

        float score_A = Vector3.Dot(impactVelocity, lastVelocity);
        float score_B = Vector3.Dot(impactVelocity, otherCar.lastVelocity);

        bool hasAdvantage = score_A > score_B;

        StartCoroutine(nameof(InvicibilityRoutine));

        if (!hasAdvantage)
        {
            Vector3 impactForce = rb.velocity - lastVelocity;

            impactForce *= 4000;

            rb.AddForce(impactForce, ForceMode.Impulse);
        }

        if (hasAdvantage)
        {
            Debug.Log(gameObject);
        }
    }

    private IEnumerator InvicibilityRoutine()
    {
        yield return new WaitForSeconds(1f);
        isInvicible = false;
    }
}
