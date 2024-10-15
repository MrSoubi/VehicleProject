using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Haptics;

public class S_RumbleManager : MonoBehaviour
{
    public delegate void VibrationEventHandler();

    public event VibrationEventHandler OnLightVibration;
    public event VibrationEventHandler OnMediumVibration;
    public event VibrationEventHandler OnStrongVibration;
    public event VibrationEventHandler OnTestVibration;

    [SerializeField] private PlayerInput _playerInput;
    private Gamepad _gamepad;

    [FoldoutGroup("Curve Vibration")]
    [SerializeField] private AnimationCurve _testVibrationCurve;
    [FoldoutGroup("Curve Vibration")]
    [SerializeField] private AnimationCurve _lightVibrationCurve;
    [FoldoutGroup("Curve Vibration")]
    [SerializeField] private AnimationCurve _mediumVibrationCurve;
    [FoldoutGroup("Curve Vibration")]
    [SerializeField] private AnimationCurve _strongVibrationCurve;

    [FoldoutGroup("Vibration Duration")]
    [SerializeField] private float _testVibrationDuration;
    [FoldoutGroup("Vibration Duration")]
    [SerializeField] private float _lightVibrationDuration;
    [FoldoutGroup("Vibration Duration")]
    [SerializeField] private float _mediumVibrationDuration;
    [FoldoutGroup("Vibration Duration")]
    [SerializeField] private float _strongVibrationDuration;

    [FoldoutGroup("Vibration Power")]
    [SerializeField] private float _testVibrationPower;
    [FoldoutGroup("Vibration Power")]
    [SerializeField] private float _lightVibrationPower;
    [FoldoutGroup("Vibration Power")]
    [SerializeField] private float _mediumVibrationPower;
    [FoldoutGroup("Vibration Power")]
    [SerializeField] private float _strongVibrationPower;


    [SerializeField]
    private bool _activateVibration;

    private void Awake()
    {

        if (_activateVibration == true)
        {
            OnTestVibration += TriggerTestVibration;
            OnLightVibration += TriggerLightVibration;
            OnMediumVibration += TriggerMediumVibration;
            OnStrongVibration += TriggerStrongVibration;
        }

        
    }

    private void OnEnable()
    {

    }

    void Start()
    {

        GetPlayerInput();
    }
    private void Update()
    {

    }

    public void Init(PlayerInput playerInput)
    {
        _playerInput = playerInput;
    }
    public void GetPlayerInput()
    {
        Debug.Log("second");
        if (_playerInput.devices.Count > 0)
        {
            Debug.Log("second");
            _gamepad = _playerInput.devices[0] as Gamepad;
        }
        else
        {
            _gamepad = _playerInput.devices[0] as Gamepad;
            Debug.Log("second");

        }
    }

    private void TriggerTestVibration()
    {
        StartVibration(_testVibrationCurve, _testVibrationDuration, _testVibrationPower);
    }
    private void TriggerLightVibration()
    {
        StartVibration(_lightVibrationCurve, _lightVibrationDuration, _lightVibrationPower);
    }


    private void TriggerMediumVibration()
    {
        StartVibration(_mediumVibrationCurve, _mediumVibrationDuration, _mediumVibrationPower);
    }

    private void TriggerStrongVibration()
    {
        StartVibration(_strongVibrationCurve, _strongVibrationDuration, _strongVibrationPower);
    }

    private void StartVibration(AnimationCurve curve, float duration, float intensityMultiplier)
    {
        //Gamepad gamepad = Gamepad.current;
        //if (gamepad != null && gamepad is IDualMotorRumble rumbleDevice)
        //{
        //    StartCoroutine(ApplyVibrationWithCurve(rumbleDevice, curve, duration, intensityMultiplier));
        //}

        if (_gamepad != null && _gamepad is IDualMotorRumble rumbleDevice)
        {
            StartCoroutine(ApplyVibrationWithCurve(rumbleDevice, curve, duration, intensityMultiplier));
        }
    }

    private IEnumerator ApplyVibrationWithCurve(IDualMotorRumble rumbleDevice, AnimationCurve curve, float duration, float intensityMultiplier)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float curveValue = curve.Evaluate(elapsed / duration);
            float leftIntensity = curveValue * intensityMultiplier;
            float rightIntensity = curveValue * intensityMultiplier;

            rumbleDevice.SetMotorSpeeds(leftIntensity, rightIntensity);

            elapsed += Time.deltaTime;
            yield return null;
        }

        rumbleDevice.ResetHaptics();
    }

    private void OnDisable()
    {
        OnTestVibration -= TriggerTestVibration;
        OnLightVibration -= TriggerLightVibration;
        OnMediumVibration -= TriggerMediumVibration;
        OnStrongVibration -= TriggerStrongVibration;
    }

    public void InvokeTestVibration() => OnTestVibration?.Invoke();
    public void InvokeLightVibration() => OnLightVibration?.Invoke();
    public void InvokeMediumVibration() => OnMediumVibration?.Invoke();
    public void InvokeStrongVibration() => OnStrongVibration?.Invoke();
}
