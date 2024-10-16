using System.Collections;
using UnityEngine;

public class ImpactManager : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] private PlayerLifeManager _playerLifeManager;
    [SerializeField] private S_RumbleManager _rumbleManager;
    [SerializeField] private float _baseImpactForceMultiplier;
    [SerializeField] private float _pourcentageMultiplier;
    [SerializeField][Range(0, 0.1f)] private float _verticalBumpForce;

    [SerializeField][Range(0, 1)] private float _advantageMultiplier; //Le multiplicateur pour le joueur qui a l avantage dans la collision

    public Vector3 velocityOddFrame, velocityEvenFrame, lastVelocity;
    bool isInvicible = false;

    private float lastSpeed, _speedOddFrame, _speedEvenFrame;
    


    private void Start()
    {
        velocityOddFrame = rb.velocity;
        velocityEvenFrame = rb.velocity;
        lastVelocity = rb.velocity;

        _speedOddFrame = rb.velocity.magnitude;
        _speedEvenFrame = rb.velocity.magnitude;
        lastSpeed = rb.velocity.magnitude;
    }

    private void FixedUpdate()
    {
        if (Time.frameCount % 2 == 0)
        {
            velocityEvenFrame = rb.velocity;
            lastVelocity = velocityOddFrame;

            _speedEvenFrame = rb.velocity.magnitude;
            lastSpeed = _speedOddFrame;
        }
        else
        {
            velocityOddFrame = rb.velocity;
            lastVelocity = velocityEvenFrame;

            _speedOddFrame = rb.velocity.magnitude;
            lastSpeed = _speedEvenFrame;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        ImpactManager otherCar;

        float ImpactVibrationValue = lastSpeed;
        _rumbleManager.InvokeImpactVibration(ImpactVibrationValue / 10); //for test

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
        


        // Determination de l'avantage
        // On compare l'alignement entre la vélocité de chaque voiture avant l'impact à la direction de la somme de ces vélocités
        // La voiture dont la vélocité est la plus alignée à la somme est celle qui a l'avantage.
        Vector3 impactVelocity = lastVelocity + otherCar.lastVelocity;

        float ImpactVibrationValue = otherCar.lastSpeed;
        _rumbleManager.InvokeImpactVibration(ImpactVibrationValue / 10);

        float LastSpeedOtherCar = otherCar.lastSpeed; //On recupere la derniere vitesse de l'autre voiture

        float score_A = Vector3.Dot(impactVelocity, lastVelocity);
        float score_B = Vector3.Dot(impactVelocity, otherCar.lastVelocity);

        bool hasAdvantage = score_A > score_B;

        // On lance une invincibilité après chaque impact
        StartCoroutine(nameof(InvicibilityRoutine));

        // Sans avantage, on prend des dégâts et une force d'impact sur le RB
        //if (!hasAdvantage)
        //{
        //    Vector3 impactForce = rb.velocity - lastVelocity;

        //    // On applique des dégâts équivalents à la force de l'impact
        //    // La force d'impact est calculée en retirant la vélocité de la dernière frame à la vélocité actuelle
        //    _playerLifeManager.ApplyDamage(impactForce.magnitude);

        //    // La force d'impact appliquée au RB est multipliée par le pourcentage de dégâts
        //    impactForce *= _playerLifeManager.GetDamageMultiplier() * 4000;

        //    rb.AddForce(impactForce, ForceMode.Impulse);

        //    // Ajout d'une légère force vers le haut
        //    rb.AddForce(Vector3.up * impactForce.magnitude / 10, ForceMode.Impulse);

        //    // Ajout d'un léger torque
        //    rb.AddTorque(impactForce / 20, ForceMode.Impulse);
        //}

        if (!hasAdvantage)
        {
            Vector3 impactForce = rb.velocity - lastVelocity;
            //Vector3 impactForce = rb.velocity - lastVelocity + Vector3.up;

            impactForce.y += _verticalBumpForce * LastSpeedOtherCar;

             impactForce *= _playerLifeManager.GetDamageMultiplier() * _baseImpactForceMultiplier;

            _playerLifeManager.ApplyDamage(Mathf.Abs(lastSpeed - LastSpeedOtherCar) * _pourcentageMultiplier);

            //Debug.Log("Multiplier add to other car" + Mathf.Abs(lastSpeed - LastSpeedOtherCar) * _pourcentageMultiplier);

            //impactForce.y += _verticalBumpForce * LastSpeedOtherCar;

            rb.AddForce(impactForce, ForceMode.Impulse);

            rb.AddTorque(impactForce / 20, ForceMode.Impulse);
        }


        // Avec l'avantage on ne subit rien du tout
    }

    private IEnumerator InvicibilityRoutine()
    {
        yield return new WaitForSeconds(1f);
        isInvicible = false;
    }
}