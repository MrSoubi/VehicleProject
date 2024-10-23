using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerLifeManager : MonoBehaviour
{
    public UnityEvent OnPercentageValueChanged;

    private float _bumpLifeMultiplierPourcentage = 0f;
    [SerializeField] private float _mutiplierByBumpPourcentage;
    public bool debugActive;

    public float percentage => _bumpLifeMultiplierPourcentage;

    public void ApplyDamage(float amount)
    {
        if (debugActive)
        {
            Debug.Log("PourcentageMultiplier before: " + Mathf.RoundToInt(_bumpLifeMultiplierPourcentage));
        }

        _bumpLifeMultiplierPourcentage += amount;
        OnPercentageValueChanged.Invoke();

        if (debugActive)
        {
            Debug.Log("PourcentageMultiplier after: " + Mathf.RoundToInt(_bumpLifeMultiplierPourcentage));

        }

    }

    public float GetDamageMultiplier()
    {
        return 1.0f + (_bumpLifeMultiplierPourcentage * _mutiplierByBumpPourcentage);
    }
}
