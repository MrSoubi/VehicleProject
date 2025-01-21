using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiresTrails : MonoBehaviour
{
    [SerializeField] private TrailRenderer trail;
    [SerializeField] private Rigidbody carRB;
    [SerializeField] private float driftLimit;

    bool shouldDrawLines = true;

    [SerializeField] CarController carController;

    private void Start()
    {

    }
    private void Update()
    {
       if(shouldDrawLines && Vector3.Dot(carRB.linearVelocity.normalized, transform.forward) < driftLimit)
        {
            trail.emitting = true;
        }
        else
        {
            trail.emitting = false;
        }
    }

    public void ActiveVFXTireTrail()
    {
        shouldDrawLines = true;
    }

    public void DesactivateVFXTireTrail()
    {
        shouldDrawLines = false;
    }
}
