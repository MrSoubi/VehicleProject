using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float speedX, speedY, speedZ;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Time.deltaTime * speedX, Time.deltaTime * speedY, Time.deltaTime * speedZ);
    }
}
