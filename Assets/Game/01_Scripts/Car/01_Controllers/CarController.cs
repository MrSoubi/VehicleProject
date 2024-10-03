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
    public UnityEvent OnLanding;

    [SerializeField] private List<WheelController> wheels = new List<WheelController>();
    [SerializeField] private SO_Car data;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private CinemachineVirtualCamera vCamera;
    public int gamepadIndex;

    Transform spawnTransform;

    //private PlayerInput _playerInput;

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

    float steerInput, pitchInput;
    bool desiredJump;

    private void Update()
    {
        //steerInput = Gamepad.all[gamepadIndex].leftStick.x.value;
        //pitchInput = Gamepad.all[gamepadIndex].leftStick.y.value;
        //desiredJump |= Gamepad.all[gamepadIndex].buttonSouth.wasPressedThisFrame;
        
    }

    int framesSinceLastGrounded;

    private void FixedUpdate()
    {
        rb.maxAngularVelocity = data.maxAngularVelocity;

        // On air (or flipped)
        if (!IsGrounded())
        {
            framesSinceLastGrounded++;

            rb.drag = 0.1f;

            // Air control
            rb.AddTorque(transform.up * steerInput * data.airSteerForce * 10);
            rb.AddTorque(transform.right * -pitchInput * data.airSteerForce);

            //// Angular drag setting depending on playerInput
            //if (Mathf.Abs(steerInput) == 0 && Mathf.Abs(pitchInput) == 0)
            //{
            //    rb.angularDrag = data.angularDrag_NoInput;
            //}
            //else
            //{
            //    rb.angularDrag = data.angularDrag_Input;
            //}
            SetAngularDrag();

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
            
            // On landing
            if (framesSinceLastGrounded > 0)
            {
                rb.drag = drag;
                vCamera.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTransposer>().m_YawDamping = 4;

                OnLanding.Invoke();
            }

            framesSinceLastGrounded = 0;

            // Jump
            if (desiredJump)
            {
                //rb.AddForce(transform.up * data.jumpForce, ForceMode.Impulse);

                //OnJump.Invoke();
            }

            
        }

        // Recover the car if it's stuck for too long
        if (flippedSince > 2.0f)
        {
            Recover();
        }

        //desiredJump = false;
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

        OnKilled.Invoke();
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

    

    //public PlayerInput GetPlayerInput()
    //{
    //    return _playerInput;
    //}

    public void Jump(InputAction.CallbackContext context)
    {
        if (IsGrounded() == true && context.performed)
        {

            rb.AddForce(transform.up * data.jumpForce, ForceMode.Impulse);

            OnJump.Invoke();
        }
    }

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
}
