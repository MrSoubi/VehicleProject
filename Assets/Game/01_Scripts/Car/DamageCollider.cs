using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    [SerializeField] float damageFactor = 1.0f;
    [SerializeField] PlayerLifeManager lifeManager;
    [SerializeField] Rigidbody carRigidBody;

    private void OnTriggerEnter(Collider other)
    {
        AttackCollider attacker;

        if (other.TryGetComponent<AttackCollider>(out attacker))
        {
            lifeManager.ApplyDamage(damageFactor * attacker.GetVelocity().magnitude);
            carRigidBody.AddForce(Vector3.up * damageFactor * 2, ForceMode.Impulse);
            carRigidBody.AddForce(attacker.GetVelocity() * damageFactor, ForceMode.Impulse);
        }
    }
}
