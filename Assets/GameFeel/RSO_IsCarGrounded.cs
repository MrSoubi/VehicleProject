using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RSO_IsCarGrounded", menuName = "Data/IsCarGrounded")]
public class RSO_IsCarGrounded : ScriptableObject
{
    Action<bool> onValueChanged;

    private bool _value;
    public bool Value
    {
        get
        {
            return _value;
        }
        set
        {
            if (value != _value)
            {
                _value = value;
                onValueChanged?.Invoke(value);
            }
        }
    }
}

