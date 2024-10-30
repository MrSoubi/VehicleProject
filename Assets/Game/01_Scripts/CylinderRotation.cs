using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderRotation : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 1f;
    [SerializeField] Vector3 forwardAxis = Vector3.right;
    void Update()
    {
        transform.Rotate(forwardAxis, rotationSpeed * Time.deltaTime);
    }
}
