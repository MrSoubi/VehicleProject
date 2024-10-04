using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotateWasteland : MonoBehaviour
{
    
    public bool srotX;
    public float srotXSpeed = 50f;
    public bool srotY;
    public float srotYSpeed = 50f;
    public bool srotZ;
    public float srotZSpeed = 50f;

    // Update is called once per frame
    void Update()
    {
        if (srotX == true)
        {
            transform.Rotate(Vector3.left * Time.deltaTime * srotXSpeed);
        }
        if (srotY == true)
        {
            transform.Rotate(Vector3.up * Time.deltaTime * srotYSpeed);
        }

        if (srotZ == true)
        {
            transform.Rotate(Vector3.back * Time.deltaTime * srotZSpeed);
        }

    }

  
}
