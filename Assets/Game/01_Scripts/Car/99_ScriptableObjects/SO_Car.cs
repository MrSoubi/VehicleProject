using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CarData", menuName = "Data/Car", order = 100)]
public class SO_Car : ScriptableObject
{
    public AnimationCurve powerCurve;
    public float maxSpeed;
    public float enginePower;
    public float jumpForce;
    public float airSteerForce;
    public float boostForce;
}
