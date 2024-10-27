using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerLifeManager : MonoBehaviour
{
    public UnityEvent OnPercentageValueChanged;

    private float _bumpLifeMultiplierPourcentage = 0f;
    [SerializeField] private float _mutiplierByBumpPourcentage;
    [SerializeField] private int _playerLife;
    [SerializeField] private S_CarInputEvent _carInputEvent;
    [SerializeField] private PlayersData _playersData;
    [SerializeField] private EventChannel _deathEvent;
    [SerializeField] private EventChannel _gameOverEvent;

    public bool debugActive;

    public float percentage => _bumpLifeMultiplierPourcentage;

    void OnEnable()
    {
        _deathEvent.onEventTriggered.AddListener(Death);
    }

    void OnDisable()
    {
        _deathEvent.onEventTriggered.RemoveListener(Death);
    }

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

    private void Death()
    {
        _playerLife--;
        if (_playerLife <= 0)
        {
            //_gameOverEvent.onEventTriggered.Invoke();
            _playersData.players.FirstOrDefault(x => x.Value._playerInput == _carInputEvent.GetPlayerInput()).Value.isAlive = false;
        }
    }
}
