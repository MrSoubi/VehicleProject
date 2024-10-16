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
    public delegate void ImapctVibrationEventHandler(float ImpactForce);

    public event VibrationEventHandler OnTestVibration;
    public event VibrationEventHandler OnBoostVibration;
    public event ImapctVibrationEventHandler OnImpactVibration;
    public event VibrationEventHandler OnDeathVibration;

    [SerializeField] private PlayerInput _playerInput;
    private Gamepad _gamepad;

    [FoldoutGroup("Curve Vibration")]
    [SerializeField] private AnimationCurve _testVibrationCurve;
    [FoldoutGroup("Curve Vibration")]
    [SerializeField] private AnimationCurve _boostVibrationCurve;
    [FoldoutGroup("Curve Vibration")]
    [SerializeField] private AnimationCurve _impactVibrationCurve;
    [FoldoutGroup("Curve Vibration")]
    [SerializeField] private AnimationCurve _deathVibrationCurve;

    [FoldoutGroup("Vibration Duration")]
    [SerializeField] private float _testVibrationDuration;
    [FoldoutGroup("Vibration Duration")]
    [SerializeField] private float _boostVibrationDuration;
    [FoldoutGroup("Vibration Duration")]
    [SerializeField] private float _impactVibrationDuration;
    [FoldoutGroup("Vibration Duration")]
    [SerializeField] private float _deathVibrationDuration;

    [FoldoutGroup("Vibration Power")]
    [Title("Test")]
    [SerializeField] private float _leftTestVibrationPower;
    [FoldoutGroup("Vibration Power")]
    [SerializeField] private float _rightTestVibrationPower;

    [FoldoutGroup("Vibration Power")]
    [Title("Boost")]
    [SerializeField] private float _leftBoostVibrationPower;
    [FoldoutGroup("Vibration Power")]
    [SerializeField] private float _rightBoostVibrationPower;

    //[FoldoutGroup("Vibration Power")]
    //[Title("Impact")]
    //[SerializeField] private float _leftImpactVibrationPower;
    //[FoldoutGroup("Vibration Power")]
    //[SerializeField] private float _rightImpactVibrationPower;

    [FoldoutGroup("Vibration Power")]
    [Title("Death")]
    [SerializeField] private float _leftDeathVibrationPower;
    [FoldoutGroup("Vibration Power")]
    [SerializeField] private float _rightDeathVibrationPower;


    [SerializeField]
    private bool _activateVibration;

    private void Awake()
    {

        if (_activateVibration == true)
        {
            OnTestVibration += TriggerTestVibration;
            OnBoostVibration += TriggerBoostVibration;
            OnImpactVibration += TriggerImpactVibration;
            OnDeathVibration += TriggerDeathVibration;
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
        if (_playerInput.devices.Count > 0)
        {
            _gamepad = _playerInput.devices[0] as Gamepad;
        }
        else
        {
            _gamepad = _playerInput.devices[0] as Gamepad;
        }
    }

    private void TriggerTestVibration()
    {
        StartVibration(_testVibrationCurve, _testVibrationDuration, _leftTestVibrationPower, _rightTestVibrationPower);
    }
    private void TriggerBoostVibration()
    {
        StartVibration(_boostVibrationCurve, _boostVibrationDuration, _leftBoostVibrationPower, _rightBoostVibrationPower);
    }


    private void TriggerImpactVibration(float ImpactForce)
    {
        StartVibration(_impactVibrationCurve, _impactVibrationDuration, ImpactForce, ImpactForce);
    }

    private void TriggerDeathVibration()
    {
        StartVibration(_deathVibrationCurve, _deathVibrationDuration, _leftDeathVibrationPower, _rightDeathVibrationPower);
    }

    private void StartVibration(AnimationCurve curve, float duration, float leftIntensityMultiplier, float rightIntensityMultiplier)
    {
        //Gamepad gamepad = Gamepad.current;
        //if (gamepad != null && gamepad is IDualMotorRumble rumbleDevice)
        //{
        //    StartCoroutine(ApplyVibrationWithCurve(rumbleDevice, curve, duration, intensityMultiplier));
        //}

        if (_gamepad != null && _gamepad is IDualMotorRumble rumbleDevice)
        {
            StartCoroutine(ApplyVibrationWithCurve(rumbleDevice, curve, duration, leftIntensityMultiplier, rightIntensityMultiplier));
        }
    }

    private IEnumerator ApplyVibrationWithCurve(IDualMotorRumble rumbleDevice, AnimationCurve curve, float duration, float leftIntensityMultiplier, float rightIntensityMultiplier)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float curveValue = curve.Evaluate(elapsed / duration);
            float leftIntensity = curveValue * leftIntensityMultiplier;
            float rightIntensity = curveValue * rightIntensityMultiplier;

            rumbleDevice.SetMotorSpeeds(leftIntensity, rightIntensity);
            elapsed += Time.deltaTime;
            yield return null;
        }

        rumbleDevice.ResetHaptics();
    }

    private void OnDisable()
    {
        OnTestVibration -= TriggerTestVibration;
        OnBoostVibration -= TriggerBoostVibration;
        OnImpactVibration -= TriggerImpactVibration;
        OnDeathVibration -= TriggerDeathVibration;
    }

    public void InvokeTestVibration() => OnTestVibration?.Invoke();
    public void InvokeBoostVibration() => OnBoostVibration?.Invoke();
    public void InvokeImpactVibration(float ImpactForce) => OnImpactVibration?.Invoke(ImpactForce);
    public void InvokeDeathVibration() => OnDeathVibration?.Invoke();
}
