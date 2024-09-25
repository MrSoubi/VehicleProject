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

    [SerializeField] float maxBoostAmount = 100;
    public float currentBoostAmount = 100;
    [SerializeField] float boostConsumptionPerSecond = 25;

    private void Update()
    {
        if (Gamepad.all[gamepadIndex].buttonEast.isPressed && currentBoostAmount > 0)
        {
            carRigidBody.AddForce(carRigidBody.transform.forward * data.boostForce * Time.deltaTime);
            currentBoostAmount -= boostConsumptionPerSecond * Time.deltaTime;
        }
    }

    public void AdddBoost(float amount)
    {
        currentBoostAmount = Mathf.Min(currentBoostAmount + amount, maxBoostAmount);
    }
}
