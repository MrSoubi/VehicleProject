using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu(fileName = "WheelData", menuName = "Data/Wheel", order = 100)]
public class SO_Wheel : ScriptableObject
{
    [PropertySpace(SpaceAfter = 15, SpaceBefore = 15)]
    public bool isMotorised;

    [FoldoutGroup("Suspension")]
    [LabelText("Resting distance")]
    public float suspensionRestDir;

    [FoldoutGroup("Suspension")]
    [LabelText("Max suspension distance")]
    public float maxSuspensionDistance;

    [FoldoutGroup("Suspension")]
    [LabelText("Spring strength")]
    public float springStrength;

    [FoldoutGroup("Suspension")]
    [LabelText("Spring damper")]
    public float springDamper;

    [FoldoutGroup("Tire")]
    [LabelText("Grip factor")]
    public float tireGripFactor;
    [FoldoutGroup("Tire")]
    public float tireMass;
    [FoldoutGroup("Tire")]
    public float wheelRadius;

    [LabelText("Steering")]
    [ToggleGroup("isSteerable")]
    public bool isSteerable;

    [ToggleGroup("isSteerable")]
    public float maxSteeringAngle;
    [ToggleGroup("isSteerable")]
    public AnimationCurve steeringInputFactor;
    [ToggleGroup("isSteerable")]
    public AnimationCurve steeringSpeedFactor;
}
