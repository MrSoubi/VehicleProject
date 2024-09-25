using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_CarRotation : MonoBehaviour
{
    [SerializeField]private float rotationSpeed = 50f;
    [SerializeField]private List<GameObject> cars = new List<GameObject>();

    void FixedUpdate()
    {
        foreach (GameObject car in cars) { 
        car.transform.Rotate(Vector3.up, rotationSpeed * Time.fixedDeltaTime);
        }
    }
}
