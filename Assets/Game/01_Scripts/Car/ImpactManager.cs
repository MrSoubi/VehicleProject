using System.Collections;
using UnityEngine;

public class ImpactManager : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] private PlayerLifeManager _playerLifeManager;

    public Vector3 velocityOddFrame, velocityEvenFrame, lastVelocity;
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
        // Determination de l'avantage
        // On compare l'alignement entre la vélocité de chaque voiture avant l'impact à la direction de la somme de ces vélocités
        // La voiture dont la vélocité est la plus alignée à la somme est celle qui a l'avantage.
        Vector3 impactVelocity = lastVelocity + otherCar.lastVelocity;

        float score_A = Vector3.Dot(impactVelocity, lastVelocity);
        float score_B = Vector3.Dot(impactVelocity, otherCar.lastVelocity);

        bool hasAdvantage = score_A > score_B;

        // On lance une invincibilité après chaque impact
        StartCoroutine(nameof(InvicibilityRoutine));
        
        // Sans avantage, on prend des dégâts et une force d'impact sur le RB
        if (!hasAdvantage)
        {
            Vector3 impactForce = rb.velocity - lastVelocity;

            // On applique des dégâts équivalents à la force de l'impact
            // La force d'impact est calculée en retirant la vélocité de la dernière frame à la vélocité actuelle
            _playerLifeManager.ApplyDamage(impactForce.magnitude);

            // La force d'impact appliquée au RB est multipliée par le pourcentage de dégâts
            impactForce *= _playerLifeManager.GetDamageMultiplier() * 4000;

            rb.AddForce(impactForce, ForceMode.Impulse);

            // Ajout d'une légère force vers le haut
            rb.AddForce(Vector3.up * impactForce.magnitude / 10, ForceMode.Impulse);

            // Ajout d'un léger torque
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