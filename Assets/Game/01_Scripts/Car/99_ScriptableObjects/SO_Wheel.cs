using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WheelData", menuName = "Data/Wheel", order = 100)]
public class SO_Wheel : ScriptableObject
{
    public float suspensionRestDir;
    public float springStrength;
    public float springDamper;
    public float tireGripFactor;
    public float tireMass;
    public bool isMotorised;
    public bool isSteerable;
    public float wheelRadius;
    public float maxSuspensionDistance;
}
