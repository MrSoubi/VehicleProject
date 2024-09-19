using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    [SerializeField] private List<WheelController> wheels = new List<WheelController>();

    [SerializeField] private SO_Car data;
    //[SerializeField] private SO_PlayerInput input;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private CinemachineVirtualCamera vCamera;
    [SerializeField] private int playerIndex;

    float drag;
    float returnedSince;

    private void Start()
    {
        drag = rb.drag;
    }

    private void Update()
    {
        // On air (or tilted)
        if (!IsGrounded())
        {
            rb.drag = 0.1f;

            // Air control
            float steerInput = Gamepad.all[playerIndex].leftStick.x.value;
            float pitchInput = Gamepad.all[playerIndex].leftStick.y.value;

            rb.AddTorque(transform.up * steerInput * data.airSteerForce);
            rb.AddTorque(transform.right * -pitchInput * data.airSteerForce);

            // Check is returned
            if (IsReturned())
            {
                returnedSince += Time.deltaTime;
            }
            else
            {
                returnedSince = 0.0f;
            }

            vCamera.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTransposer>().m_YawDamping = 20;
        }
        else // On ground
        {
            rb.drag = drag;

            // Jump
            if (Gamepad.all[playerIndex].buttonSouth.wasPressedThisFrame)
            {
                rb.AddForce(transform.up * data.jumpForce, ForceMode.Impulse);
            }

            vCamera.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTransposer>().m_YawDamping = 4;
        }

        // Boost
        if (Gamepad.all[playerIndex].buttonEast.isPressed)
        {
            rb.AddForce(transform.forward * data.boostForce * Time.deltaTime);
        }

        // Apply force to tilt the car if it's stuck for too long
        if (returnedSince > 2.0f)
        {
            rb.AddForceAtPosition((Vector3.up + Vector3.right / 2) * data.jumpForce /2, transform.position, ForceMode.Impulse);
            returnedSince = 0.0f;
        }
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

    public bool IsReturned()
    {
        return Vector3.Dot(transform.up, Vector3.down) > 0.9f;
    }
}
