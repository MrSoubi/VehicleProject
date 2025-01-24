using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using Unity.VisualScripting;

public class CarController : MonoBehaviour
{
    [SerializeField] private List<WheelController> wheels = new List<WheelController>();
    [SerializeField] private SO_Car data;
    [SerializeField] private Rigidbody rb;

    public RSO_IsCarGrounded RSO_IsCarGrounded;
    public RSE_MetalCollision metalCollision;
    public RSE_CarLanding carLanding;
    public RSE_BoostActivated boostActivated;
    public RSO_CarBoosting carBoosting;

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

        drag = rb.linearDamping;

        velocityOddFrame = rb.linearVelocity;
        velocityEvenFrame = rb.linearVelocity;
        lastVelocity = rb.linearVelocity;

        speedOddFrame = rb.linearVelocity.magnitude;
        speedEvenFrame = rb.linearVelocity.magnitude;
        lastSpeed = rb.linearVelocity.magnitude;
    }

    float steerInput, pitchInput;
    int framesSinceLastGrounded;
    int framesSinceGoingReverse;

    bool boostInput;
    public float boostStrength = 500;

    private void Update()
    {
        Gamepad gamepad = Gamepad.all[0];
        if (gamepad == null)
            return;

        if (!boostInput && gamepad.bButton.ReadValue() == 1)
        {
            boostActivated.trigger?.Invoke();
        }

        boostInput = gamepad.bButton.ReadValue() == 1;
        carBoosting.Value = boostInput;
    }

    private void FixedUpdate()
    {
        // TODO : change to a different frame count !!!
        if (Time.frameCount % 2 == 0)
        {
            velocityEvenFrame = rb.linearVelocity;
            lastVelocity = velocityOddFrame;

            speedEvenFrame = rb.linearVelocity.magnitude;
            lastSpeed = speedOddFrame;
        }
        else
        {
            velocityOddFrame = rb.linearVelocity;
            lastVelocity = velocityEvenFrame;

            speedOddFrame = rb.linearVelocity.magnitude;
            lastSpeed = speedEvenFrame;

        }

        SetAngularDrag();
        rb.maxAngularVelocity = data.maxAngularVelocity;

        // On air (or flipped)
        if (!IsGrounded())
        {
            framesSinceLastGrounded++;

            rb.linearDamping = 0.1f;

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
                carLanding.trigger?.Invoke(-lastVelocity.y, framesSinceLastGrounded);

                rb.linearDamping = drag;

                if (!canJump)
                {
                    StartCoroutine(JumpReloadRoutine());
                }
            }

            framesSinceLastGrounded = 0;

            if (IsGoingInReverse())
            {
                framesSinceGoingReverse++;
            }
            else
            {
                framesSinceGoingReverse = 0;
            }


        }

        // Recover the car if it's stuck for too long
        if (flippedSince > 0.5f)
        {
            canRecover = true;
        }
        //Debug.Log(rb.angularDrag);

        if (boostInput)
        {
            rb.AddForce(transform.forward * boostStrength);
        }
    }

    bool canRecover;

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Props")) return;

        Quaternion orientation = Quaternion.FromToRotation(transform.position, collision.contacts[0].point);
        metalCollision.trigger?.Invoke(collision.contacts[0].point, rb.velocity, orientation);
    }

    Vector3 GetFloorNormal()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, -transform.up, out hit);

        return hit.normal;
    }
    private bool IsGoingInReverse()
    {
        return Vector3.Dot(rb.linearVelocity, transform.forward) < 0 && reverseValue > 0;
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

        RSO_IsCarGrounded.Value = result;

        return result;
    }

    public void SetAngularDrag()
    {
        if (IsGrounded()){
            rb.angularDamping = 0;
        }else{
            // Angular drag setting depending on playerInput
            if (Mathf.Abs(steerInput) == 0 && Mathf.Abs(pitchInput) == 0)
            {
                rb.angularDamping = data.angularDrag_NoInput;
            }
            else
            {
                rb.angularDamping = data.angularDrag_Input;
            }
        }
    }

    public bool IsFlipped()
    {
        return Vector3.Dot(transform.up, Vector3.down) > 0.9f;
    }

    public float GetSpeedRatio()
    {
        float speed = rb.linearVelocity.magnitude / data.maxSpeed;
        float value = data.powerCurve.Evaluate(speed);

        return value;
    }

    public void Kill()
    {
        transform.position = spawnPosition;

        transform.rotation = spawnRotation;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        Invoke(nameof(ResetTeleport), 0.5f);
    }
    private void ResetTeleport()
    {
        HasTeleported = false;
    }

    private void StopTheCar()
    {     
        rb.linearVelocity = Vector3.zero;
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
        Gizmos.DrawLine(transform.position + Vector3.up, transform.position + Vector3.up + rb.linearVelocity);
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
