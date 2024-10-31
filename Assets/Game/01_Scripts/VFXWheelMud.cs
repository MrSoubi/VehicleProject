using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class VFXWheelMud : MonoBehaviour
{
    [SerializeField] private List<ParticleSystem> mud;
    [SerializeField] private Rigidbody rb;

    [SerializeField] CarController carController;

    public float emissionFactor;

    private void Start()
    {
        for (int i = 0; i < mud.Count; i++)
        {
            mud[i].Play();
        }

        carController.OnLanding.AddListener(ActiveVFXMud);
        carController.OnJump.AddListener(DeactivateVFXMud);
        carController.OnTakeOff.AddListener(DeactivateVFXMud);
    }


    public void ActiveVFXMud()
    {
        for (int i = 0; i < mud.Count; i++)
        {
            var em = mud[i].emission;
            em.rateOverTime = rb.velocity.magnitude * emissionFactor;
        }
    }

    public void DeactivateVFXMud()
    {
        for (int i = 0; i < mud.Count; i++)
        {
            var em = mud[i].emission;
            em.rateOverTime = 0;
        }
    }
}
