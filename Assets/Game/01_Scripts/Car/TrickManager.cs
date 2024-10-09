using UnityEngine;

public class TrickManager : MonoBehaviour
{
    [SerializeField] Rigidbody carRigidbody;
    [SerializeField] CarController carController;

    public float xRotation, yRotation, zRotation;
    Quaternion lastRotation;

    private void Start()
    {
        carController.OnJump.AddListener(HandleTakeOff);
        carController.OnLanding.AddListener(HandleLanding);
    }

    private void Update()
    {
        xRotation += (carRigidbody.rotation * Quaternion.Inverse(lastRotation)).x;
        yRotation += (carRigidbody.rotation * Quaternion.Inverse(lastRotation)).y;
        zRotation += (carRigidbody.rotation * Quaternion.Inverse(lastRotation)).z;

        lastRotation = carRigidbody.rotation;
    }

    private void HandleTakeOff()
    {
        xRotation = 0;
        yRotation = 0;
        zRotation = 0;
    }

    private void HandleLanding()
    {
        Debug.Log(Mathf.RoundToInt(xRotation * 100) + " | " + Mathf.RoundToInt(yRotation * 100) + " | " + Mathf.RoundToInt(zRotation * 100));

        xRotation = 0;
        yRotation = 0;
        zRotation = 0;
    }
}