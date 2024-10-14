using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class WheelController : MonoBehaviour
{
    [SerializeField] private Rigidbody carRigidBody;
    [SerializeField] private Transform carTransform;
    [SerializeField] private SO_Car carData;
    [SerializeField] private SO_Wheel data;
    [SerializeField] private GameObject mesh;
    public int gamepadIndex;

    private float _gripFactor => data.tireGripFactor;
    private float _driftGripFactor => data.tireDriftGripFactor;
    private float _currentGripFactor;
    
    private bool _isThrottle = false;
    private bool _isReverse = false;
    private bool _isDrifting => _isReverse && _isThrottle;

    float steerInput = 0.0f;
    float accelInput => accelValue - reverseValue;

    float accelValue = 0f;
    float reverseValue = 0f;

    private void Awake(){
        _currentGripFactor = _gripFactor;
    }
    private void LateUpdate()
    {
        // Mesh rotation around X axis depending on the car velocity
        // Bug with the rotation, something to do with the forward of the velocity and of the wheel.
        mesh.transform.Rotate(Mathf.Rad2Deg * carRigidBody.velocity.magnitude / data.wheelRadius * Time.deltaTime, 0, 0);
    }

    // Update is called once per frame
    void FixedUpdate(){
        transform.rotation = carTransform.rotation;
        float steeringAngle = data.steeringSpeedFactor.Evaluate(carRigidBody.velocity.magnitude / carData.maxSpeed) * data.steeringInputFactor.Evaluate(Mathf.Abs(steerInput)) * Mathf.Sign(steerInput) * data.maxSteeringAngle;
        transform.Rotate(transform.up, steeringAngle);

        LayerMask mask = LayerMask.GetMask("Ground");
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, data.maxSuspensionDistance + data.wheelRadius, mask))
        {
            Suspension(hit);
            Steering(hit);
            if (data.isMotorised)
            {
                Acceleration(hit);
            }
        }
        else
        {
            mesh.transform.position = transform.position;
            suspensionForce = Vector3.zero;
            steeringForce = Vector3.zero;
            accelerationForce = Vector3.zero;
        }

        Debug.Log("current gripFactor" + _currentGripFactor);
        Debug.Log("current isDrifting" + _isDrifting);
    }

    Vector3 suspensionForce = Vector3.zero;
    public void Suspension(RaycastHit tireRay){
        Vector3 springDir = transform.up;
        Vector3 tireWorldVel = carRigidBody.GetPointVelocity(transform.position);
        float offset = data.suspensionRestDir - tireRay.distance;
        float vel = Vector3.Dot(springDir, tireWorldVel);
        float force = (offset * data.springStrength) - (vel * data.springDamper);
        carRigidBody.AddForceAtPosition(springDir * force, transform.position);

        mesh.transform.position = transform.position + new Vector3(0, -tireRay.distance + data.wheelRadius, 0);
        suspensionForce = springDir * force;
    }

    Vector3 steeringForce = Vector3.zero;
    public void Steering(RaycastHit tireRay){
        Vector3 steeringDir = transform.right;
        Vector3 tireWorldVel = carRigidBody.GetPointVelocity(transform.position);
        float steeringVel = Vector3.Dot(steeringDir, tireWorldVel);
        float desiredVelChange = -steeringVel * _currentGripFactor;
        float desiredAccel = desiredVelChange / Time.fixedDeltaTime;
        carRigidBody.AddForceAtPosition(steeringDir * data.tireMass * desiredAccel, transform.position);

        steeringForce = steeringDir * data.tireMass * desiredAccel;
    }

    Vector3 accelerationForce = Vector3.zero;
    public void Acceleration(RaycastHit tireRay)
    {
        Vector3 accelDir = transform.forward;
        if (Mathf.Abs(accelInput) > 0.0f){

            float carSpeed = Vector3.Dot(carTransform.forward, carRigidBody.velocity);
            float normalizedSpeed = Mathf.Clamp01(Mathf.Abs(carSpeed) / carData.maxSpeed);

            float availableTorque = carData.powerCurve.Evaluate(normalizedSpeed) * accelInput;

            carRigidBody.AddForceAtPosition(accelDir * availableTorque * carData.enginePower, transform.position);
            accelerationForce = accelDir * availableTorque * carData.enginePower;
        }
    }

    public bool isGrounded()
    {
        LayerMask mask = LayerMask.GetMask("Ground");
        RaycastHit hit;
        return Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, data.maxSuspensionDistance + data.wheelRadius, mask);
    }

    public bool debug = false;
    public float debugVectorFactor = 1;
    private void OnDrawGizmos()
    {
        if (debug)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawSphere(transform.position, data.wheelRadius);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + accelerationForce * debugVectorFactor);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + steeringForce * debugVectorFactor);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + suspensionForce * debugVectorFactor);
        }
    }

    public void Steer(InputAction.CallbackContext context)
    {
        if(data.isSteerable)
        {
            steerInput = context.ReadValue<float>();
        }
    }

    public void OnSteerCancel(InputAction.CallbackContext context)
    {
        steerInput = 0f;
    }

    public void Throttle(InputAction.CallbackContext context)
    {
        accelValue = context.ReadValue<float>();
        _isThrottle = true;
    }

    public void Reverse(InputAction.CallbackContext context)
    {
        
        
        reverseValue = context.ReadValue<float>();
        _isReverse = true;


        if (_isDrifting == true){
            _currentGripFactor = SetDriftGriptFactor();
        }
    }

    public void OnThrottleCancel(InputAction.CallbackContext context)
    {
        _isThrottle = false;
        accelValue = 0f;
    }

    public void OnReverseCancel(InputAction.CallbackContext context)
    {
        if (_isDrifting == true){
            _currentGripFactor = Mathf.Lerp(SetDriftGriptFactor(), SetGripFactor(), 1f);
        }
        else{
            _currentGripFactor = SetGripFactor();
        }
        _isReverse = false;
        reverseValue = 0f;
    }

    private float SetGripFactor(){
        return _gripFactor;
    }

    private float SetDriftGriptFactor(){
        return _driftGripFactor;
    }

    private float SetFactor(){
        if (_isDrifting == true)
        {
            return SetDriftGriptFactor();
        }
        else{
            return SetGripFactor();
        }
    }

    
}
