using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MultipleEventChannel", menuName = "ScriptableObjects/MultipleEventChannel", order = 4)]
public class MultipleEventChannel : ScriptableObject
{
    public delegate void IntEvent(int value);
    public delegate void StringEvent(string message);
    public delegate void FloatEvent(float value);

    public delegate int InReturntEvent();
    public delegate string StringReturnEvent();
    public delegate float FloatReturnEvent();

    public IntEvent OnIntEventRaised;
    public StringEvent OnStringEventRaised;
    public FloatEvent OnFloatEventRaised;

    public InReturntEvent OnIntEventReturnRaised;
    public StringReturnEvent OnStringEventReturnRaised;
    public FloatReturnEvent OnFloatEventReturnRaised;

    public int RaiseIntEventReturn()
    {
        if (OnIntEventReturnRaised != null)
        {
            return OnIntEventReturnRaised.Invoke();
        }
        else
        {
            return 0;
        }
    }
    public string RaiseStringEventReturn()
    {
        if (OnStringEventReturnRaised != null)
        {
            return OnStringEventReturnRaised.Invoke();
        }
        else
        {
            return null;
        }
    }
    public float RaiseFloatEventReturn()
    {
        if (OnFloatEventReturnRaised != null)
        {
            return OnFloatEventReturnRaised.Invoke();
        }
        else
        {
            return 0;
        }
    }


    public void RaiseIntEvent(int value)
    {
        OnIntEventRaised?.Invoke(value);
    }
    public void RaiseStringEvent(string message)
    {
        OnStringEventRaised?.Invoke(message);
    }

    public void RaiseFloatEvent(float value)
    {
        OnFloatEventRaised?.Invoke(value);
    }
}
