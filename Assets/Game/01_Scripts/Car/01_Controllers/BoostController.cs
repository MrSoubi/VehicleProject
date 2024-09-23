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

    private void Update()
    {
        if (Gamepad.all[gamepadIndex].buttonEast.isPressed)
        {
            carRigidBody.AddForce(carRigidBody.transform.forward * data.boostForce * Time.deltaTime);
        }
    }
}
