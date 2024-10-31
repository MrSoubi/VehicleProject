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
    [SerializeField] private CarColorManager _carColorManager;
    public int PlayerID { get; private set; }

    private void Awake()
    {

        if (_isForTest == true)
        {
            Initialize(GetComponent<PlayerInput>(),0);
        }
    }
   

    public void Initialize(PlayerInput playerInput, int playerID)
    {

        _playerInput = playerInput;
        PlayerID = playerID;

        _carColorManager.SetColor(playerID);

        _playerInput.actions["Jump"].performed += _carController.Jump;
        _playerInput.actions["Steer"].performed += _carController.SteerInAir;
        _playerInput.actions["Steer"].canceled += _carController.OnSteerCanceled;
        _playerInput.actions["Pitch"].performed += _carController.PitchInAir;
        _playerInput.actions["Pitch"].canceled += _carController.OnPitchCanceled;
        _playerInput.actions["Reverse"].performed += _carController.Reverse;
        _playerInput.actions["Reverse"].canceled += _carController.OnReverseCancel;

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

    public PlayerInput GetPlayerInput()
    {
        return _playerInput;
    }

    public void PlayerInputDisable()
    {
        _playerInput.DeactivateInput();
    }
    
}
