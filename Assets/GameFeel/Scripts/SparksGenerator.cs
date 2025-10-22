using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparksGenerator : MonoBehaviour
{
    [SerializeField] GameObject prefab_Sparks;

    public RSE_MetalCollision metalCollision;

    private void OnEnable()
    {
        metalCollision.trigger += GenerateSparks;
    }

    private void OnDisable()
    {
        metalCollision.trigger -= GenerateSparks;
    }

    void GenerateSparks(Vector3 position, Vector3 velocity, Quaternion orientation)
    {
        GameObject sparks = Instantiate(prefab_Sparks, position, orientation);
        sparks.GetComponent<Rigidbody>().linearVelocity = velocity;
    }
}
