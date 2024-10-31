using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEditor;
using System;

// TODO: See Baptiste for the boost behaviour. Boost limit ? Boost max speed ? Boost quantity ?
public class BoostController : MonoBehaviour
{
    public UnityEvent OnBoostActivation;
    public UnityEvent OnBoostDeactivation;
    public UnityEvent OnBoostValueChanged;

    [SerializeField] Rigidbody carRigidBody;
    [SerializeField] CarController carController;
    [SerializeField] SO_Car data;
    [SerializeField] int gamepadIndex;
    [SerializeField] private S_RumbleManager _rumbleManager;
    [SerializeField] TrickManager trickManager;
    [SerializeField] private EventChannel _deathEvent;
    [SerializeField] ImpactManager impactManager;


    [SerializeField] float flipBonus = 10, backFlipBonus = 10, frontFlipBonus = 10, shoveItBonus = 10, impactBonus = 10;

    public bool isBoosting = false;

    private float currentBoostAmount;
    public float maxBoostAmount => data.maxBoostAmount;

    private bool isButtonHeld = false;

    private void Awake()
    {
        BoostFullRecuperation();
    }
    private void Start()
    {
        trickManager.OnBackFlipCompleted.AddListener(OnBackFlip);
        trickManager.OnFrontFlipCompleted.AddListener(OnFrontFlip);
        trickManager.OnShoveItCompleted.AddListener(OnShoveIt);
        trickManager.OnFlipCompleted.AddListener(OnFlip);
        impactManager.OnImpactWithAdvantage.AddListener(OnImpact);
    }

    private void OnImpact()
    {
        AddBoost(impactBonus);
    }

    private void OnEnable()
    {
        _deathEvent.onEventTriggered.AddListener(BoostFullRecuperation);
    }

    private void OnDisable()
    {
        _deathEvent.onEventTriggered.RemoveListener(BoostFullRecuperation);
    }

    private void Update()
    {
        if (currentBoostAmount > 0 && isButtonHeld == true)
        {
            if (!isBoosting)
            {
                OnBoostActivation.Invoke();
                _rumbleManager.InvokeBoostVibration();
                isBoosting = true;
            }

            if (carController.IsGrounded())
            {
                carRigidBody.AddForce(carRigidBody.transform.forward * data.boostForceOnGround * Time.deltaTime);
            }
            else
            {
                carRigidBody.AddForce(carRigidBody.transform.forward * data.boostForceOnAir * Time.deltaTime);
            }

            currentBoostAmount -= data.boostConsumptionPerSecond * Time.deltaTime;
            OnBoostValueChanged.Invoke();
            isBoosting = true;
        }
        else
        {
            if (isBoosting)
            {
                OnBoostDeactivation.Invoke();
                StopBoosting();

            }
        }
    }

    void OnBackFlip()
    {
        AddBoost(backFlipBonus);
    }

    void OnFrontFlip()
    {
        AddBoost(frontFlipBonus);
    }

    void OnFlip()
    {
        AddBoost(flipBonus);
    }

    void OnShoveIt()
    {
        AddBoost(shoveItBonus);
    }

    public void AddBoost(float amount)
    {
        currentBoostAmount = Mathf.Min(currentBoostAmount + amount, data.maxBoostAmount);
        OnBoostValueChanged.Invoke();
    }

    public void StartBoost(InputAction.CallbackContext context)
    {
        if (context.performed && currentBoostAmount > 0)
        {
            isButtonHeld = true;
        }
    }

    public void StopBoost(InputAction.CallbackContext context)
    {

        if (context.canceled)
        {
            isButtonHeld = false;

            if (isBoosting)
            {
                StopBoosting();
            }
        }
    }
    private void StopBoosting()
    {
        _rumbleManager.InvokeEndBoostVibration();
        OnBoostDeactivation.Invoke();
        isBoosting = false;
    }

    public void BoostFullRecuperation()
    {
        currentBoostAmount = maxBoostAmount;
    }

    public float GetCurrentBoost()
    {
        return currentBoostAmount;
    }
}
