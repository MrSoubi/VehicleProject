using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    [SerializeField] Rigidbody carRigidBody;
    [SerializeField] float baseDamage;

    public Vector3 GetVelocity()
    {
        return carRigidBody.velocity;
    }
}
