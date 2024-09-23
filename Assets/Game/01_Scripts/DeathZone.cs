using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CarController>() != null)
        {
            other.GetComponent<CarController>().Kill();
        }
    }
}
