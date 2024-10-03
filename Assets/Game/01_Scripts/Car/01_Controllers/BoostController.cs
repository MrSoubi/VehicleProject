using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// TODO: See Baptiste for the boost behaviour. Boost limit ? Boost max speed ? Boost quantity ?
public class BoostController : MonoBehaviour
{
    [SerializeField] Rigidbody carRigidBody;
    [SerializeField] CarController carController;
    [SerializeField] SO_Car data;
    [SerializeField] int gamepadIndex;

    public float currentBoostAmount = 100;
    private bool isBoosting = false;

    private void Update()
    {
        if (currentBoostAmount > 0 && isBoosting == true)
        {
            carRigidBody.AddForce(carRigidBody.transform.forward * data.boostForce * Time.deltaTime);
            currentBoostAmount -= data.boostConsumptionPerSecond * Time.deltaTime;
        }
    }

    public void AdddBoost(float amount)
    {
        currentBoostAmount = Mathf.Min(currentBoostAmount + amount, data.maxBoostAmount);
    }

    public void StartBoost(InputAction.CallbackContext context)
    {
        if (context.performed && currentBoostAmount > 0)
        {
            isBoosting = true;
        }
    }

    public void StopBoost(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            isBoosting = false;
        }
    }
}
