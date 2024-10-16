using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXSpeedlines : MonoBehaviour
{
    [SerializeField] private ParticleSystem speedlines;
    [SerializeField] private float emissionRate;
    [SerializeField] private BoostController boostController;

    void Start()
    {
        speedlines.Play();
    }


    void Update()
    {
        if (boostController.isBoosting)
        {
            var em = speedlines.emission;
            em.rateOverTime = emissionRate;
        }
        else
        {
            var em = speedlines.emission;
            em.rateOverTime = 0;
        }
    }
}
