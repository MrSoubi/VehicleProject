using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VFXBoost : MonoBehaviour
{
    [SerializeField] private List<ParticleSystem> boost;
    [SerializeField] private float emissionRate;
    [SerializeField] private BoostController boostController;


    private void Start()
    {
        for (int i = 0; i < boost.Count; i++)
        {
            boost[i].Play();
        }
    }

    private void Update()
    {
        if (boostController.isBoosting)
        {
            for (int i = 0; i < boost.Count; i++)
            {
                var em = boost[i].emission;
                em.rateOverTime = emissionRate;
            }
        }
        else
        {
            for (int i = 0; i < boost.Count; i++)
            {
                var em = boost[i].emission;
                em.rateOverTime = 0;
            }
        }
    }

}
