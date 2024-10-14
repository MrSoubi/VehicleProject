using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WreckingBall : MonoBehaviour
{
    [SerializeField] private float wreckingBallForce;
    private void OnTriggerEnter(Collider other)
    {
        CarController car;

        if (other.TryGetComponent<CarController>(out car))
        {
            car.OnWreckingBallEffect(wreckingBallForce);
        }
    }
}
