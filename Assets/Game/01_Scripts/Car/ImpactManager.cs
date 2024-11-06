using System.Collections;
using UnityEngine;
using UnityEngine.Events;


public enum KillBy
{
    P1,
    P2,
    P3,
    P4,
    Environement,
}
public class ImpactManager : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] private PlayerLifeManager _playerLifeManager;
    [SerializeField] private S_RumbleManager _rumbleManager;
    [SerializeField] private float _baseImpactForceMultiplier;
    [SerializeField] private float _pourcentageMultiplier;
    [SerializeField][Range(0, 0.1f)] private float _verticalBumpForce;
    [SerializeField] private GameObject _impactSFX;
    [SerializeField] private S_CarInputEvent _carInputEvent;
    
    public int playerId => _carInputEvent.PlayerID;

    int otherPlayerId;

    [SerializeField][Range(0, 1)] private float _advantageMultiplier; //Le multiplicateur pour le joueur qui a l avantage dans la collision

    public Vector3 velocityOddFrame, velocityEvenFrame, lastVelocity;
    bool isInvicible = false;

    private float lastSpeed, _speedOddFrame, _speedEvenFrame;

    public UnityEvent OnImpactAsAVictim;
    public UnityEvent OnImpactWithAdvantage;

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
        //otherPlayerId = otherCar.playerId;
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
        _rumbleManager.InvokeImpactVibration(ImpactVibrationValue);

        float LastSpeedOtherCar = otherCar.lastSpeed; //On recupere la derniere vitesse de l'autre voiture

        float score_A = Vector3.Dot(impactVelocity, lastVelocity);
        float score_B = Vector3.Dot(impactVelocity, otherCar.lastVelocity);

        bool hasAdvantage = score_A > score_B;

        // On lance une invincibilité après chaque impact
        StartCoroutine(nameof(InvicibilityRoutine));

        if (!hasAdvantage)
        {
            Vector3 impactForce = rb.velocity - lastVelocity;
            //Vector3 impactForce = rb.velocity - lastVelocity + Vector3.up;

            impactForce.y += _verticalBumpForce * LastSpeedOtherCar;

             impactForce *= _playerLifeManager.GetDamageMultiplier() * _baseImpactForceMultiplier;

            _playerLifeManager.ApplyDamage(Mathf.Abs(lastSpeed - LastSpeedOtherCar) * _pourcentageMultiplier);

            rb.AddForce(impactForce, ForceMode.Impulse);

            rb.AddTorque(impactForce / 20, ForceMode.Impulse);

            Instantiate(_impactSFX, collision.transform.position, Quaternion.identity);

            OnImpactAsAVictim.Invoke();
        }
        else // Avec l'avantage on ne subit rien du tout
        {
            OnImpactWithAdvantage.Invoke();
        }
        
    }

    private IEnumerator InvicibilityRoutine()
    {
        yield return new WaitForSeconds(1f);
        isInvicible = false;
    }
}