using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WreckingBall : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        CarController car;

        if (other.TryGetComponent<CarController>(out car))
        {
            car.OnWreckingBallEffect();
        }
    }
}
