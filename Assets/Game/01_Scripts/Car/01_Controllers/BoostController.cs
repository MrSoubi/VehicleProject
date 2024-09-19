using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostController : MonoBehaviour
{
    [SerializeField] Rigidbody carRigidBody;
    [SerializeField] CarController carController;
    [SerializeField] SO_Car data;

    public void Boost()
    {
        carRigidBody.AddForce(carRigidBody.transform.forward * data.boostForce * Time.deltaTime);
    }
}
