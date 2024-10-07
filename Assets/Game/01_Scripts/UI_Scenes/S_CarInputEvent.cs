using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class S_CarInputEvent : MonoBehaviour
{
    private PlayerInput _playerInput;
    [SerializeField] private CarController _carController;
    [SerializeField] private WheelController[] _wheelCollider;
    [SerializeField] private BoostController _boostController;

    [SerializeField] private bool _isForTest;

    private void Awake()
    {

        if (_isForTest == true)
        {
            Initialize(GetComponent<PlayerInput>());
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize(PlayerInput playerInput)
    {

        _playerInput = playerInput;


        _playerInput.actions["Jump"].performed += _carController.Jump;
        _playerInput.actions["Steer"].performed += _carController.SteerInAir;
        _playerInput.actions["Steer"].canceled += _carController.OnSteerCanceled;
        _playerInput.actions["Pitch"].performed += _carController.PitchInAir;
        _playerInput.actions["Pitch"].canceled += _carController.OnPitchCanceled;

        foreach (WheelController wheelController in _wheelCollider)
        {
            _playerInput.actions["Steer"].performed += wheelController.Steer;
            _playerInput.actions["Steer"].canceled += wheelController.OnSteerCancel;
            _playerInput.actions["Throttle"].performed += wheelController.Throttle;
            _playerInput.actions["Reverse"].performed += wheelController.Reverse;
            _playerInput.actions["Throttle"].canceled += wheelController.OnThrottleCancel;
            _playerInput.actions["Reverse"].canceled += wheelController.OnReverseCancel;
        }

        _playerInput.actions["Boost"].performed += _boostController.StartBoost;
        _playerInput.actions["Boost"].canceled += _boostController.StopBoost;

    }

    public void RemoveAllAcions()
    {
        _playerInput.actions["Jump"].performed -= _carController.Jump;
        _playerInput.actions["Steer"].performed -= _carController.SteerInAir;
        _playerInput.actions["Steer"].canceled -= _carController.OnSteerCanceled;
        _playerInput.actions["Pitch"].performed -= _carController.PitchInAir;
        _playerInput.actions["Pitch"].canceled -= _carController.OnPitchCanceled;

        foreach (WheelController wheelController in _wheelCollider)
        {
            _playerInput.actions["Steer"].performed -= wheelController.Steer;
            _playerInput.actions["Steer"].canceled -= wheelController.OnSteerCancel;
            _playerInput.actions["Throttle"].performed -= wheelController.Throttle;
            _playerInput.actions["Reverse"].performed -= wheelController.Reverse;
            _playerInput.actions["Throttle"].canceled -= wheelController.OnThrottleCancel;
            _playerInput.actions["Reverse"].canceled -= wheelController.OnReverseCancel;
        }

        _playerInput.actions["Boost"].performed -= _boostController.StartBoost;
        _playerInput.actions["Boost"].canceled -= _boostController.StopBoost;
    }
}
