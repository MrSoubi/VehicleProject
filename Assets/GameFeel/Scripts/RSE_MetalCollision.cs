using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RSE_MetalCollision", menuName = "Data/MetalCollision")]
public class RSE_MetalCollision : ScriptableObject
{
    public Action<Vector3, Vector3, Quaternion> trigger;
}
