using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVHandler : MonoBehaviour
{
    [SerializeField] private float baseFOV = 60f;
    [SerializeField] private float boostFOV = 90f;

    // Duration (seconds) for transitioning FOV in/out
    [SerializeField] private float zoomInDuration = 0.5f;
    [SerializeField] private float zoomOutDuration = 0.5f;

    // Reference to whatever triggers the boost
    // Make sure it's set, or found in Start() if needed
    [SerializeField] private RSE_BoostActivated boostActivated;

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void OnEnable()
    {
        boostActivated.trigger += HandleBoostActivation;
    }

    private void OnDisable()
    {
        boostActivated.trigger -= HandleBoostActivation;
    }

    private void HandleBoostActivation()
    {
        // Start a coroutine that handles the FOV animation
        StartCoroutine(FOVRoutine());
    }

    private IEnumerator FOVRoutine()
    {
        // 1) Transition from baseFOV to boostFOV
        float elapsed = 0f;
        while (elapsed < zoomInDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / zoomInDuration;
            cam.fieldOfView = Mathf.Lerp(baseFOV, boostFOV, t);
            yield return null;
        }

        // 2) Transition back from boostFOV to baseFOV
        elapsed = 0f;
        while (elapsed < zoomOutDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / zoomOutDuration;
            cam.fieldOfView = Mathf.Lerp(boostFOV, baseFOV, t);
            yield return null;
        }
    }
}
