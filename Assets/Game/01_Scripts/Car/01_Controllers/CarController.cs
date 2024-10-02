using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    [SerializeField] private List<WheelController> wheels = new List<WheelController>();
    [SerializeField] private SO_Car data;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private CinemachineVirtualCamera vCamera;
    public int gamepadIndex;

    Transform spawnTransform;

    // TODO: Serialize or set these parameters in the SO_Car
    float drag;
    float flippedSince;

    private void Start()
    {
        spawnTransform = transform;

        if (rb == null)
        {
            Debug.LogError("RigidBody not found on " + gameObject.name);
        }
        if (vCamera == null)
        {
            Debug.LogError("Virtual Camera not found on " + gameObject.name);
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
    }

    // TODO : Passer en FixedUpdate et sortir les inputs d'ici
    private void Update()
    {
        rb.maxAngularVelocity = data.maxAngularVelocity;

        // On air (or flipped)
        if (!IsGrounded())
        {
            rb.drag = 0.1f;

            // Air control
            float steerInput = Gamepad.all[gamepadIndex].leftStick.x.value;
            float pitchInput = Gamepad.all[gamepadIndex].leftStick.y.value;

            rb.AddTorque(transform.up * steerInput * data.airSteerForce);
            rb.AddTorque(transform.right * -pitchInput * data.airSteerForce);

            if (Mathf.Abs(steerInput) == 0 && Mathf.Abs(pitchInput) == 0)
            {
                rb.angularDrag = data.angularDrag_NoInput;
            }
            else
            {
                rb.angularDrag = data.angularDrag_Input;
            }

            // Check is returned
            if (IsFlipped())
            {
                flippedSince += Time.deltaTime;
            }
            else
            {
                flippedSince = 0.0f;
            }

            vCamera.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTransposer>().m_YawDamping = 20;
        }
        else // On ground
        {
            rb.drag = drag;

            // Jump
            if (Gamepad.all[gamepadIndex].buttonSouth.wasPressedThisFrame)
            {
                rb.AddForce(transform.up * data.jumpForce, ForceMode.Impulse);
            }

            vCamera.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTransposer>().m_YawDamping = 4;
        }

        // Recover the car if it's stuck for too long
        if (flippedSince > 2.0f)
        {
            Recover();
        }
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
    }

    public void setGamepadIndex(int gamepadIndex)
    {
        this.gamepadIndex = gamepadIndex;
        foreach (WheelController wheel in wheels)
        {
            wheel.GetComponent<WheelController>().gamepadIndex = gamepadIndex;
        }
    }

    public Camera GetCamera()
    {
        return GetComponentInChildren<Camera>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position + rb.centerOfMass, 0.2f);
    }
}
