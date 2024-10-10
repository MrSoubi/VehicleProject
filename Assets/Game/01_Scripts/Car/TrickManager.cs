using UnityEngine;

public class TrickManager : MonoBehaviour
{
    [SerializeField] Rigidbody carRigidbody;
    [SerializeField] CarController carController;

    public float xRotation, yRotation, zRotation;
    Quaternion lastRotation;
    Vector3 lastForward;
    Vector3 lastUpward;
    Vector3 lastSideward;

    private void Start()
    {
        carController.OnJump.AddListener(HandleTakeOff);
        carController.OnLanding.AddListener(HandleLanding);

        lastForward = transform.forward;
        lastUpward = transform.up;
        lastSideward = transform.right;
    }

    private void Update()
    {
        // x rotation (hardflip)
        Vector3 xUpward = Vector3.ProjectOnPlane(transform.up, transform.right);
        Vector3 xLastUpward = Vector3.ProjectOnPlane(lastUpward, transform.right);
        float xDelta = Vector3.SignedAngle(xLastUpward, xUpward, transform.right);

        // y rotation (pop shove it)
        Vector3 ySideward = Vector3.ProjectOnPlane(transform.right, transform.up);
        Vector3 yLastSideward = Vector3.ProjectOnPlane(lastSideward, transform.up);
        float yDelta = Vector3.SignedAngle(yLastSideward, ySideward, transform.up);

        // z rotation (kickflip)
        Vector3 zUpward = Vector3.ProjectOnPlane(transform.up, transform.forward);
        Vector3 zLastUpward = Vector3.ProjectOnPlane(lastUpward, transform.forward);
        float zDelta = Vector3.SignedAngle(zLastUpward, zUpward, transform.forward);

        xRotation += xDelta;
        yRotation += yDelta;
        zRotation += zDelta;

        lastForward = transform.forward;
        lastUpward = transform.up;
        lastSideward = transform.right;
    }

    private void HandleTakeOff()
    {
        xRotation = 0;
        yRotation = 0;
        zRotation = 0;
    }

    private void HandleLanding()
    {
        Debug.Log("hardflip: " + Mathf.RoundToInt(xRotation) + " | " + "shoveit: " + Mathf.RoundToInt(yRotation) + " | " + Mathf.RoundToInt(zRotation));

        xRotation = 0;
        yRotation = 0;
        zRotation = 0;
    }
}