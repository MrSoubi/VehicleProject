using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CarData", menuName = "Data/Car", order = 100)]
public class SO_Car : ScriptableObject
{
    [FoldoutGroup("Engine")]
    public AnimationCurve powerCurve;
    [FoldoutGroup("Engine")]
    public float maxSpeed;
    [FoldoutGroup("Engine")]
    public float enginePower;
    [FoldoutGroup("Engine")]
    public float jumpForce;

    [FoldoutGroup("Air control")]
    [Title("Forces")]
    [LabelText("Steering force")]
    public float airSteerForce;

    [FoldoutGroup("Air control")]
    [LabelText("Max angular velocity")]
    public float maxAngularVelocity;

    [FoldoutGroup("Air control")]
    [Title("Air Resistance")]
    [LabelText("During free fall")]
    public float angularDrag_NoInput;

    [FoldoutGroup("Air control")]
    [LabelText("During control")]
    public float angularDrag_Input;

    [FoldoutGroup("Boost")]
    [LabelText("Strength")]
    public float boostForce;

    [FoldoutGroup("Boost")]
    [LabelText("Max capacity")]
    public float maxBoostAmount;

    [FoldoutGroup("Boost")]
    [LabelText("Consumption /sec")]
    public float boostConsumptionPerSecond;
}
