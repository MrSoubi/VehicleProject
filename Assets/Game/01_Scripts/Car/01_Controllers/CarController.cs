using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class CarController : MonoBehaviour
{
    public UnityEvent OnDeath;
    public UnityEvent OnKilled;
    public UnityEvent OnJump;
    public UnityEvent OnTakeOff;
    public UnityEvent OnLanding;
    public UnityEvent OnReverse;
    public UnityEvent OnForward;
    public UnityEvent OnJumpEnabled;
    public UnityEvent OnJumpDisabled;

    [SerializeField] private List<WheelController> wheels = new List<WheelController>();
    [SerializeField] private SO_Car data;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private S_RumbleManager _rumbleManager;
    [SerializeField] private EventChannel _deathEvent;
    [SerializeField] EventChannel _gameLoopEnd;



    public int gamepadIndex;

    [SerializeField] float jumpDelay = 5;

    Vector3 spawnPosition;
    Quaternion spawnRotation;

    // TODO: Serialize or set these parameters in the SO_Car
    float drag;
    float flippedSince;

    public bool canJump = true;
    public bool HasTeleported { get; set; }

    Vector3 velocityOddFrame, velocityEvenFrame, lastVelocity;
    float lastSpeed, speedOddFrame, speedEvenFrame;

    private void Start()
    {
        spawnPosition = transform.position;
        spawnRotation = transform.rotation;

        if (rb == null)
        {
            Debug.LogError("RigidBody not found on " + gameObject.name);
        }
        if (data == null)
        {
            Debug.LogError("Data not found on " + gameObject.name);
        }
        if (wheels.Count != 4)
        {
            Debug.LogWarning("Wheels not set correctly on " + gameObject.name);
        }

        drag = rb.drag;

        velocityOddFrame = rb.velocity;
        velocityEvenFrame = rb.velocity;
        lastVelocity = rb.velocity;

        speedOddFrame = rb.velocity.magnitude;
        speedEvenFrame = rb.velocity.magnitude;
        lastSpeed = rb.velocity.magnitude;
    }

    float steerInput, pitchInput;
    bool desiredJump;
    int framesSinceLastGrounded;
    int framesSinceGoingReverse;

    private void FixedUpdate()
    {
        // TODO : change to a different frame count !!!
        if (Time.frameCount % 2 == 0)
        {
            velocityEvenFrame = rb.velocity;
            lastVelocity = velocityOddFrame;

            speedEvenFrame = rb.velocity.magnitude;
            lastSpeed = speedOddFrame;
        }
        else
        {
            velocityOddFrame = rb.velocity;
            lastVelocity = velocityEvenFrame;

            speedOddFrame = rb.velocity.magnitude;
            lastSpeed = speedEvenFrame;

        }

        SetAngularDrag();
        rb.maxAngularVelocity = data.maxAngularVelocity;

        // On air (or flipped)
        if (!IsGrounded())
        {
            if (framesSinceLastGrounded == 0)
            {
                OnTakeOff.Invoke();
            }

            framesSinceLastGrounded++;

            rb.drag = 0.1f;

            // Air control
            rb.AddTorque(transform.up * steerInput * data.airSteerForce);
            rb.AddTorque(transform.right * pitchInput * data.airSteerForce);

            

            // Check is returned
            if (IsFlipped())
            {
                flippedSince += Time.deltaTime;
            }
            else
            {
                canRecover = false;
                flippedSince = 0.0f;
            }
        }
        else // On ground
        {
            // On landing
            if (framesSinceLastGrounded > 0)
            {
                rb.drag = drag;

                if (!canJump)
                {
                    StartCoroutine(JumpReloadRoutine());
                }

                // A modifier ! Sert � redonner une bonne velo � la voiture lors de la r�ception
                // rb.velocity = Vector3.ProjectOnPlane(transform.forward, GetFloorNormal()) * lastSpeed;

                OnLanding.Invoke();
            }

            framesSinceLastGrounded = 0;

            if (IsGoingInReverse())
            {
                if (framesSinceGoingReverse == 0)
                {
                    OnReverse.Invoke();
                }
                framesSinceGoingReverse++;
            }
            else
            {
                framesSinceGoingReverse = 0;
                OnForward.Invoke();
            }
        }

        // Recover the car if it's stuck for too long
        if (flippedSince > 0.5f)
        {
            canRecover = true;
        }
        //Debug.Log(rb.angularDrag);
    }

    bool canRecover;

    Vector3 GetFloorNormal()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, -transform.up, out hit);

        return hit.normal;
    }
    private bool IsGoingInReverse()
    {
        return Vector3.Dot(rb.velocity, transform.forward) < 0 && reverseValue > 0;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (canRecover && context.performed)
        {
            rb.AddForce(-transform.up * data.jumpForce, ForceMode.Impulse);
            canRecover = false;
            return;
        }

        if (canJump && context.performed)
        {
            rb.AddForce(transform.up * data.jumpForce, ForceMode.Impulse);
            OnJump.Invoke();
            canJump = false;
            OnJumpDisabled.Invoke();
            return;
        }
    }

    private IEnumerator JumpReloadRoutine()
    {
        yield return new WaitForSeconds(jumpDelay);
        canJump = true;
        OnJumpEnabled.Invoke();
    }

    // TODO : Make the forces applied more effective to untilt the car
    private void Recover()
    {
        rb.AddForceAtPosition((Vector3.up + Vector3.right / 2) * data.jumpForce / 2, transform.position, ForceMode.Impulse);
        flippedSince = 0.0f;
    }

    public bool IsGrounded()
    {
        bool result = false;
        foreach (WheelController wheel in wheels)
        {
            result |= wheel.isGrounded();
        }
        return result;
    }

    public void SetAngularDrag()
    {
        if (IsGrounded()){
            rb.angularDrag = 0;
        }else{
            // Angular drag setting depending on playerInput
            if (Mathf.Abs(steerInput) == 0 && Mathf.Abs(pitchInput) == 0)
            {
                rb.angularDrag = data.angularDrag_NoInput;
            }
            else
            {
                rb.angularDrag = data.angularDrag_Input;
            }
        }
    }

    public bool IsFlipped()
    {
        return Vector3.Dot(transform.up, Vector3.down) > 0.9f;
    }

    public float GetSpeedRatio()
    {
        float speed = rb.velocity.magnitude / data.maxSpeed;
        float value = data.powerCurve.Evaluate(speed);

        return value;
    }

    public void Kill()
    {
        transform.position = spawnPosition;

        //_deathEvent.onEventTriggered.Invoke();

        transform.rotation = spawnRotation;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;


        _rumbleManager.InvokeDeathVibration();

        OnKilled.Invoke();

        OnDeath.Invoke();


        Invoke(nameof(ResetTeleport), 0.5f);
    }
    private void ResetTeleport()
    {
        HasTeleported = false;
    }

    private void StopTheCar()
    {     
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.useGravity = false;
        rb.isKinematic = true;

    }
    // Still in use ?
    public void setGamepadIndex(int gamepadIndex)
    {
        this.gamepadIndex = gamepadIndex;
        foreach (WheelController wheel in wheels)
        {
            wheel.GetComponent<WheelController>().gamepadIndex = gamepadIndex;
        }
    }

    // Should not be used ! SOLID !
    public Camera GetCamera()
    {
        return GetComponentInChildren<Camera>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position + rb.centerOfMass, 0.2f);
        Gizmos.DrawLine(transform.position + Vector3.up, transform.position + Vector3.up + rb.velocity);
    }

    private void OnEnable()
    {
        _gameLoopEnd.onEventTriggered.AddListener(StopTheCar);
    }

    private void OnDisable()
    {
        _gameLoopEnd.onEventTriggered.RemoveListener(StopTheCar);
    }
    #region INPUT
    public void SteerInAir(InputAction.CallbackContext context)
    {
        steerInput = context.ReadValue<float>();
    }

    public void PitchInAir(InputAction.CallbackContext context)
    {
        pitchInput = context.ReadValue<float>();
    }
    public void OnSteerCanceled(InputAction.CallbackContext context)
    {
        steerInput = 0f;
    }
    public void OnPitchCanceled(InputAction.CallbackContext context)
    {
        pitchInput = 0f;
    }

    float reverseValue;
    public void Reverse(InputAction.CallbackContext context)
    {
        reverseValue = context.ReadValue<float>();
    }

    public void OnReverseCancel(InputAction.CallbackContext context)
    {
        reverseValue = 0f;
    }
    #endregion
}
