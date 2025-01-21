using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CameraShakeDefinition", menuName = "Data/CameraShakeDefinition")]
public class CameraShakeDefinition : ScriptableObject
{
    [Header("Réglages du Shake")]
    [Tooltip("Durée de l'effet de tremblement (en secondes).")]
    public float duration = 0.5f;

    [Tooltip("Axe de tremblement (ex: (1,1,0) pour secouer en X et Y uniquement).")]
    public Vector3 axis = Vector3.one;

    [Tooltip("Vitesse à laquelle le tremblement s'atténue (relâche).")]
    public float release = 1.0f;
}