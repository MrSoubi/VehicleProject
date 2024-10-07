using System.Collections;
using System.Collections.Generic;
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
        PlayerLifeManager otherCarLifeManager;
        collision.gameObject.TryGetComponent<PlayerLifeManager>(out otherCarLifeManager);

        if (!collision.gameObject.TryGetComponent<ImpactManager>(out otherCar) && !collision.gameObject.TryGetComponent<PlayerLifeManager>(out otherCarLifeManager))
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

            
            
            impactForce *= otherLifeManager.GetDamageMultiplier() * _baseImpactForceMultiplier;
            otherLifeManager.ApplyDamage(collision.rigidbody.velocity.magnitude * _pourcentageMultiplier);
            Debug.Log("Multiplier add" + collision.rigidbody.velocity.magnitude * _pourcentageMultiplier);
            //_playerLifeManager.GetDamageMultiplier();
            //_playerLifeManager.ApplyDamage(5);


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
