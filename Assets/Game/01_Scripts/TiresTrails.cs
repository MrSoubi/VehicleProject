using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiresTrails : MonoBehaviour
{
    [SerializeField] private TrailRenderer trail;
    [SerializeField] private Rigidbody carRB;
    [SerializeField] private float driftLimit;
    private void Update()
    {
       if(Vector3.Dot(carRB.velocity.normalized, transform.forward) < driftLimit)
        {
            trail.emitting = true;
        }
        else
        {
            trail.emitting = false;
        }
    }
}
