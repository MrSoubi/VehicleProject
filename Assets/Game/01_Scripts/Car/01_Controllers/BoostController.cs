using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

// TODO: See Baptiste for the boost behaviour. Boost limit ? Boost max speed ? Boost quantity ?
public class BoostController : MonoBehaviour
{
    [SerializeField] Rigidbody carRigidBody;
    [SerializeField] CarController carController;
    [SerializeField] SO_Car data;
    [SerializeField] int gamepadIndex;

    public bool isBoosting;

    public float currentBoostAmount = 100;

    private void Update()
    {
        if (Gamepad.all[gamepadIndex].buttonEast.isPressed && currentBoostAmount > 0)
        {
            carRigidBody.AddForce(carRigidBody.transform.forward * data.boostForce * Time.deltaTime);
            currentBoostAmount -= data.boostConsumptionPerSecond * Time.deltaTime;
            isBoosting = true;
        }
        else
        {
            isBoosting = false;
        }
    }

    public void AdddBoost(float amount)
    {
        currentBoostAmount = Mathf.Min(currentBoostAmount + amount, data.maxBoostAmount);
    }
}
