using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

// TODO: See Baptiste for the boost behaviour. Boost limit ? Boost max speed ? Boost quantity ?
public class BoostController : MonoBehaviour
{
    public UnityEvent OnBoostActivation;
    public UnityEvent OnBoostDeactivation;
    [SerializeField] Rigidbody carRigidBody;
    [SerializeField] CarController carController;
    [SerializeField] SO_Car data;
    [SerializeField] int gamepadIndex;
    [SerializeField] private S_RumbleManager _rumbleManager;

    public bool isBoosting = false;

    public float currentBoostAmount;

    private bool isButtonHeld = false;

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

    public void AddBoost(float amount)
    {
        currentBoostAmount = Mathf.Min(currentBoostAmount + amount, data.maxBoostAmount);
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
}
