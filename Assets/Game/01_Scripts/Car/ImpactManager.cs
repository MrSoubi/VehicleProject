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
    [SerializeField] [Range(0, 1)]private float _advantageMultiplier; //Le multiplicateur pour le joueur qui a l avantage dans la collision

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
        float OtherCarSpeed = collision.rigidbody.velocity.magnitude;
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

        HandleImpact(otherCar, collision, otherCarLifeManager, OtherCarSpeed);
    }

    private void HandleImpact(ImpactManager otherCar, Collision collision, PlayerLifeManager otherLifeManager, float OtherCarSpeed)
    {
        Vector3 impactVelocity = lastVelocity + otherCar.lastVelocity;

        float score_A = Vector3.Dot(impactVelocity, lastVelocity);
        float score_B = Vector3.Dot(impactVelocity, otherCar.lastVelocity);
        Debug.Log("scoreA : " + score_A);
        Debug.Log("score_B : " + score_B);
        bool hasAdvantage = score_A > score_B;

        StartCoroutine(nameof(InvicibilityRoutine));
        
        if (!hasAdvantage)
        {
            Vector3 impactForce = rb.velocity - lastVelocity;

            Debug.Log("Impact Velocity : " +impactVelocity);
            Debug.Log("Impact force : " + impactForce);

            //float OtherCarSpeed= collision.rigidbody.velocity.magnitude;
            //OtherCarSpeed = 
            Debug.Log(rb.velocity.magnitude);

            impactForce *= _playerLifeManager.GetDamageMultiplier() * _baseImpactForceMultiplier;
            _playerLifeManager.ApplyDamage(OtherCarSpeed * _pourcentageMultiplier);
            //Debug.Log("Multiplier add to other car" + OtherCarSpeed * _pourcentageMultiplier);


            


            rb.AddForce(impactForce, ForceMode.Impulse);
            
        }

        if (hasAdvantage)
        {
            Vector3 impactForce = rb.velocity - lastVelocity;
            //Debug.Log(rb.velocity.magnitude);
            //float OtherCarSpeed = collision.rigidbody.velocity.magnitude;
            //Debug.Log(OtherCarSpeed);
            impactForce *= _playerLifeManager.GetDamageMultiplier() * _baseImpactForceMultiplier * _advantageMultiplier;
            _playerLifeManager.ApplyDamage(OtherCarSpeed * _pourcentageMultiplier * _advantageMultiplier);

            rb.AddForce(impactForce, ForceMode.Impulse);

            //Debug.Log("Multiplier add" + OtherCarSpeed * _pourcentageMultiplier * _advantageMultiplier);

            Debug.Log(gameObject);
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
