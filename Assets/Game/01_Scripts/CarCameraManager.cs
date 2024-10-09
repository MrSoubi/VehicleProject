using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCameraManager : MonoBehaviour
{
    [SerializeField] CarController carController;

    [SerializeField] Camera cam;

    [SerializeField] CinemachineVirtualCamera groundCam;
    [SerializeField] CinemachineVirtualCamera reverseCam;
    [SerializeField] CinemachineVirtualCamera airCam;
    [SerializeField] CinemachineVirtualCamera leftCam;
    [SerializeField] CinemachineVirtualCamera rightCam;
    [SerializeField] CinemachineVirtualCamera deathCam;

    private void Start()
    {
        carController.OnTakeOff.AddListener(ActivateAirCamera);
        carController.OnJump.AddListener(ActivateAirCamera);
        carController.OnLanding.AddListener(ActivateGroundCamera);
        carController.OnKilled.AddListener(ActivateDeathCamera);
        carController.OnForward.AddListener(ActivateGroundCamera);
        carController.OnReverse.AddListener(ActivateReverseCamera);

        reverseCam.enabled = false;
        airCam.enabled = false;
        leftCam.enabled = false;
        rightCam.enabled = false;
        deathCam.enabled = false;
    }

    public void ActivateReverseCamera()
    {
        if (reverseCam.enabled) return;

        reverseCam.enabled = true;

        groundCam.enabled = false;
        airCam.enabled = false;
        leftCam.enabled = false;
        rightCam.enabled = false;
        deathCam.enabled = false;
    }

    public void ActivateGroundCamera()
    {
        if (groundCam.enabled) return;

        groundCam.enabled = true;

        reverseCam.enabled = false;
        airCam.enabled = false;
        leftCam.enabled = false;
        rightCam.enabled = false;
        deathCam.enabled = false;
    }

    public void ActivateAirCamera()
    {
        if (airCam.enabled) return;

        airCam.enabled = true;

        groundCam.enabled = false;
        reverseCam.enabled = false;
        leftCam.enabled = false;
        rightCam.enabled = false;
        deathCam.enabled = false;
    }

    public void ActivateLeftCamera()
    {
        if (leftCam.enabled) return;
        leftCam.enabled = true;

        groundCam.enabled = false;
        reverseCam.enabled = false;
        airCam.enabled = false;
        rightCam.enabled = false;
        deathCam.enabled = false;
    }

    public void ActivateRightCamera()
    {
        if (rightCam.enabled) return;
        rightCam.enabled = true;

        groundCam.enabled = false;
        reverseCam.enabled = false;
        airCam.enabled = false;
        leftCam.enabled = false;
        deathCam.enabled = false;
    }

    public void ActivateDeathCamera()
    {
        if (deathCam.enabled) return;
        deathCam.enabled = true;

        groundCam.enabled = false;
        reverseCam.enabled = false;
        airCam.enabled = false;
        leftCam.enabled = false;
        rightCam.enabled = false;
    }
}

