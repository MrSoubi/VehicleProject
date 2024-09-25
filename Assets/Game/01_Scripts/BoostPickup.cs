using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostPickup : MonoBehaviour
{
    public float boostAmount = 25;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<BoostController>() != null)
        {
            other.GetComponent<BoostController>().AdddBoost(boostAmount);
            Destroy(gameObject);
        }
    }
}
