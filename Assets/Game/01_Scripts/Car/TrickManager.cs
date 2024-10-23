using CartoonFX;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TrickManager : MonoBehaviour
{
    public UnityEvent OnFlipCompleted, OnShoveItCompleted, OnFrontFlipCompleted, OnBackFlipCompleted;

    [SerializeField] Rigidbody carRigidbody;
    [SerializeField] CarController carController;

    [SerializeField] GameObject VFX;

    [SerializeField] float minXRotation = 360, minYRotation = 180, minZRotation = 180;

    float xRotation, yRotation, zRotation;

    Vector3 lastForward;
    Vector3 lastUpward;
    Vector3 lastSideward;

    enum BaseTricks
    {
        BACKFLIP,
        FRONTFLIP,
        SHOVEIT,
        FLIP
    }

    List<BaseTricks> currentTricks;

    private void Start()
    {
        carController.OnJump.AddListener(HandleTakeOff);
        carController.OnLanding.AddListener(HandleLanding);

        lastForward = transform.forward;
        lastUpward = transform.up;
        lastSideward = transform.right;

        currentTricks = new List<BaseTricks>();
    }

    private void Update()
    {
        if (!canDoTricks) return;

        HandleRotation();

        HandleTricks();

        lastForward = transform.forward;
        lastUpward = transform.up;
        lastSideward = transform.right;
    }

    private void HandleRotation()
    {
        // x rotation (front/back flip)
        Vector3 xUpward = Vector3.ProjectOnPlane(transform.up, transform.right);
        Vector3 xLastUpward = Vector3.ProjectOnPlane(lastUpward, transform.right);
        float xDelta = Vector3.SignedAngle(xLastUpward, xUpward, transform.right);

        // y rotation (shove it)
        Vector3 ySideward = Vector3.ProjectOnPlane(transform.right, transform.up);
        Vector3 yLastSideward = Vector3.ProjectOnPlane(lastSideward, transform.up);
        float yDelta = Vector3.SignedAngle(yLastSideward, ySideward, transform.up);

        // z rotation (flip)
        Vector3 zUpward = Vector3.ProjectOnPlane(transform.up, transform.forward);
        Vector3 zLastUpward = Vector3.ProjectOnPlane(lastUpward, transform.forward);
        float zDelta = Vector3.SignedAngle(zLastUpward, zUpward, transform.forward);

        xRotation += xDelta;
        yRotation += yDelta;
        zRotation += zDelta;
    }

    bool canDoTricks = false;
    private void HandleTakeOff()
    {
        canDoTricks = true;

        xRotation = 0;
        yRotation = 0;
        zRotation = 0;
    }

    private void HandleLanding()
    {
        canDoTricks = false;

        currentTricks.Clear();

        xRotation = 0;
        yRotation = 0;
        zRotation = 0;
    }

    private void PrintTricks()
    {
        foreach (var trick in currentTricks)
        {
            Debug.Log(trick);
        }
    }

    private void HandleTricks()
    {
        if (Mathf.Abs(xRotation) > minXRotation)
        {
            if (xRotation > minXRotation)
            {
                xRotation -= minXRotation;
                currentTricks.Add(BaseTricks.FRONTFLIP);

                OnFrontFlipCompleted.Invoke();

                GameObject localVFX = Instantiate(VFX, transform.position + Vector3.up * 3, Quaternion.identity);
                localVFX.GetComponent<CFXR_ParticleText>().UpdateText("FRONTFLIP");
            }

            if (xRotation < -minXRotation)
            {
                xRotation += minXRotation;
                currentTricks.Add(BaseTricks.BACKFLIP);

                OnBackFlipCompleted.Invoke();

                GameObject localVFX = Instantiate(VFX, transform.position + Vector3.up * 3, Quaternion.identity);
                localVFX.GetComponent<CFXR_ParticleText>().UpdateText("BACKFLIP");
            }
        }

        if (Mathf.Abs(yRotation) > minYRotation)
        {
            if (yRotation > minYRotation)
            {
                yRotation -= minYRotation;
                currentTricks.Add(BaseTricks.SHOVEIT);

                OnShoveItCompleted.Invoke();

                VFX.GetComponent<CFXR_ParticleText>().UpdateText("SHOVE IT");

                GameObject localVFX = Instantiate(VFX, transform.position + Vector3.up * 3, Quaternion.identity);
                localVFX.GetComponent<CFXR_ParticleText>().UpdateText("SHOVE IT");
            }

            if (yRotation < -minYRotation)
            {
                yRotation += minYRotation;
                currentTricks.Add(BaseTricks.SHOVEIT);

                OnShoveItCompleted.Invoke();

                VFX.GetComponent<CFXR_ParticleText>().UpdateText("SHOVE IT");
                GameObject localVFX = Instantiate(VFX, transform.position + Vector3.up * 3, Quaternion.identity);
                localVFX.GetComponent<CFXR_ParticleText>().UpdateText("SHOVE IT");
            }
        }

        if (Mathf.Abs(zRotation) > minZRotation)
        {
            if (zRotation > minZRotation)
            {
                zRotation -= minZRotation;
                currentTricks.Add(BaseTricks.FLIP);

                OnFlipCompleted.Invoke();

                VFX.GetComponent<CFXR_ParticleText>().UpdateText("FLIP");
                GameObject localVFX = Instantiate(VFX, transform.position + Vector3.up * 3, Quaternion.identity);
                localVFX.GetComponent<CFXR_ParticleText>().UpdateText("FLIP");
            }

            if (zRotation < -minZRotation)
            {
                zRotation += minZRotation;
                currentTricks.Add(BaseTricks.FLIP);

                OnFlipCompleted.Invoke();

                VFX.GetComponent<CFXR_ParticleText>().UpdateText("FLIP");
                GameObject localVFX = Instantiate(VFX, transform.position + Vector3.up * 3, Quaternion.identity);
                localVFX.GetComponent<CFXR_ParticleText>().UpdateText("FLIP");
            }
        }
    }
}