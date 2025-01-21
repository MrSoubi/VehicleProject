using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RSE_BoostActivated", menuName = "Data/BoostActivated")]
public class RSE_BoostActivated : ScriptableObject
{
    public Action trigger;
}
