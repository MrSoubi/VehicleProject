using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;


public class ImpactManager : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    public Vector3 velocityOddFrame, velocityEvenFrame, lastVelocity;

    public bool isVerbose;
    bool isInvicible = false;

    [SerializeField] private PlayerLifeManager _playerLifeManager;
    [SerializeField] private float _baseImpactForceMultiplier;
    [SerializeField] private float _pourcentageMultiplier;

    private float lastSpeed, speedOddFrame, speedEvenFrame;

    private void Start()
    {
        velocityOddFrame = rb.velocity;
        velocityEvenFrame = rb.velocity;
        lastVelocity = rb.velocity;

        speedOddFrame = rb.velocity.magnitude;
        speedEvenFrame = rb.velocity.magnitude;
        lastSpeed = rb.velocity.magnitude;
    }

    private void FixedUpdate()
    {
        if (Time.frameCount % 2 == 0)
        {
            velocityEvenFrame = rb.velocity;
            lastVelocity = velocityOddFrame;

            speedEvenFrame = rb.velocity.magnitude;
            lastSpeed = speedOddFrame;
        }
        else
        {
            velocityOddFrame = rb.velocity;
            lastVelocity = velocityEvenFrame;

            speedOddFrame = rb.velocity.magnitude;
            lastSpeed = speedEvenFrame;

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        ImpactManager otherCar;
        PlayerLifeManager otherCarLifeManager;
        collision.gameObject.TryGetComponent<PlayerLifeManager>(out otherCarLifeManager);

        if (!collision.gameObject.TryGetComponent<ImpactManager>(out otherCar))
        {
            return;
        }

        if (isInvicible)
        {
            return;
        }

        isInvicible = true;

        HandleImpact(otherCar, collision, otherCarLifeManager);
    }

    private void HandleImpact(ImpactManager otherCar, Collision collision, PlayerLifeManager otherLifeManager)
    {
        Vector3 impactVelocity = lastVelocity + otherCar.lastVelocity;

        float score_A = Vector3.Dot(impactVelocity, lastVelocity);
        float score_B = Vector3.Dot(impactVelocity, otherCar.lastVelocity);

        bool hasAdvantage = score_A > score_B;

        StartCoroutine(nameof(InvicibilityRoutine));
        
        if (!hasAdvantage)
        {
            Vector3 impactForce = rb.velocity - lastVelocity;
            _playerLifeManager.ApplyDamage(impactForce.magnitude);

            impactForce *= _playerLifeManager.GetDamageMultiplier();

            rb.AddForce(impactForce, ForceMode.Impulse);
        }
    }

    private IEnumerator InvicibilityRoutine()
    {
        yield return new WaitForSeconds(1f);
        isInvicible = false;
    }

    public float GetPourcentageMultiplier()
    {
        return _pourcentageMultiplier;
    }
}