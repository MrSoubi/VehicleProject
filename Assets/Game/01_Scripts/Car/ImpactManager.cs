using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;


public enum KillBy
{
    P1,
    P2,
    P3,
    P4,
    Self,
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
    [SerializeField] PlayersData _playersData;

    private int _playerId => _carInputEvent.PlayerID;
    private KillBy _playerName => (KillBy)_playerId;


    int _otherPlayerId = (int)KillBy.Self;

    [SerializeField][Range(0, 1)] private float _advantageMultiplier; //Le multiplicateur pour le joueur qui a l avantage dans la collision

    public Vector3 velocityOddFrame, velocityEvenFrame, lastVelocity;
    bool isInvicible = false;

    private float lastSpeed, _speedOddFrame, _speedEvenFrame;

    public UnityEvent OnImpactAsAVictim;
    public UnityEvent OnImpactWithAdvantage;

    private Coroutine _registerEnnemyCoroutine;
    private bool _isRegisting = false;

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
        if (_registerEnnemyCoroutine != null)
        {
            StopRegisterCoroutine();

        }

        _otherPlayerId = otherCar._playerId;

        RegisterEnnemyId();

        Debug.Log($"Player ID: {_playerId} and PlayerName {_playerName}" );
        Debug.Log($"PlayerEnnemy ID: {_otherPlayerId} and PlayerEnnemyName {_playersData.players.FirstOrDefault(x => x.Value.playerId == _otherPlayerId).Value.playerId}");


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

    private void RegisterEnnemyId()
    {
        while (_isRegisting == false)
        {
            _isRegisting = true;
            _registerEnnemyCoroutine = StartCoroutine(WaitToRegisterEnnemy());
        }
    }
    private IEnumerator WaitToRegisterEnnemy()
    {
        

        yield return new WaitForSeconds(10f);
        _registerEnnemyCoroutine = null;
        _otherPlayerId = (int)KillBy.Self;
        _isRegisting = false;
    } 

    private void StopRegisterCoroutine()
    {
        StopCoroutine(_registerEnnemyCoroutine);
        _registerEnnemyCoroutine = null;
        _isRegisting = false;


    }

    public void SelectNamesForPlayer()
    {
        Debug.Log("IsRegistering");
        switch (_otherPlayerId)
        {
            case 0:
                RegisterEnnemy(KillBy.P1);
                break;
            case 1:
                RegisterEnnemy(KillBy.P2);
                break;
            case 2:
                RegisterEnnemy(KillBy.P3);
                break;
            case 3:
                RegisterEnnemy(KillBy.P4);
                break;
            case 4:
                RegisterEnnemy(KillBy.Self);
                break;
            default:
                break;
        }

    }

    private void RegisterEnnemy(KillBy ennemyName)
    {
        _playersData.players.FirstOrDefault(x => x.Value.playerId == _playerId).Value.listKilledBy.Add(ennemyName);

        if (_otherPlayerId < (int)KillBy.Self)
        {
            _playersData.players.FirstOrDefault(x => x.Value.playerId == _otherPlayerId).Value.listYouKilled.Add(_playerName);
            _otherPlayerId = (int)KillBy.Self;

        }
        else
        {
            _otherPlayerId = (int)KillBy.Self;
        }


    }


}