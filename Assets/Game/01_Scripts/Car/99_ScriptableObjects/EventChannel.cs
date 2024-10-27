using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "EventChannel", menuName = "ScriptableObjects/EventChannel", order = 1)]
public class EventChannel : ScriptableObject
{
    public UnityEvent onEventTriggered;

    public void RaiseEvent()
    {
        if (onEventTriggered != null)
        {
            onEventTriggered.Invoke();
        }
    }
}
