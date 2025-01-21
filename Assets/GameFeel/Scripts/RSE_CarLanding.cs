using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RSE_CarLanding", menuName = "Data/CarLanding")]
public class RSE_CarLanding : ScriptableObject
{
    public Action<float, float> trigger;
}
