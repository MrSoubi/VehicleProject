using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class CarController : MonoBehaviour
{
    public UnityEvent OnKilled;
    public UnityEvent OnJump;
    public UnityEvent OnTakeOff;
    public UnityEvent OnLanding;
    public UnityEvent OnReverse;
    public UnityEvent OnForward;

    [SerializeField] private List<WheelController> wheels = new List<WheelController>();
    [SerializeField] private SO_Car data;
    [SerializeField] private Rigidbody rb;

    public int gamepadIndex;

    [SerializeField] float jumpDelay = 5;

    Transform spawnTransform;

    // TODO: Serialize or set these parameters in the SO_Car
    float drag;
    float flippedSince;

    bool canJump;

    Vector3 velocityOddFrame, velocityEvenFrame, lastVelocity;
    float lastSpeed, speedOddFrame, speedEvenFrame;

    private void Start()
    {
        spawnTransform = transform;

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
            rb.AddTorque(transform.right * -pitchInput * data.airSteerForce);

            SetAngularDrag();

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

                // A modifier ! Sert à redonner une bonne velo à la voiture lors de la réception
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
            return;
        }
    }

    private IEnumerator JumpReloadRoutine()
    {
        yield return new WaitForSeconds(jumpDelay);
        canJump = true;
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
        transform.position = spawnTransform.position;
        transform.rotation = spawnTransform.rotation;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        OnKilled.Invoke();
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

    public void OnWreckingBallEffect(float force)
    {
        Debug.Log("Hit");
        rb.AddForce(-transform.forward *  force, ForceMode.Impulse);
        rb.AddTorque(transform.up, ForceMode.Impulse);
    }
}
