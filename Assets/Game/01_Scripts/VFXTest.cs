using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class VFXTest : MonoBehaviour
{
    [SerializeField] private List<ParticleSystem> mud;
    [SerializeField] private int gamepadIndex;
    [SerializeField] private Rigidbody rb;
    public float emissionFactor;

    private void Start()
    {
        for (int i = 0; i < mud.Count; i++)
        {
            mud[i].Play();
        }
    }

    void Update()
    {
        for (int i = 0; i < mud.Count; i++)
        {
            var em = mud[i].emission;
            em.rateOverTime = rb.velocity.magnitude * emissionFactor;
        }
    }
}
