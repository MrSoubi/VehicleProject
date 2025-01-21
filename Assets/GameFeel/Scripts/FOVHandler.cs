using System.Collections;
using UnityEngine;

public class FOVHandler : MonoBehaviour
{
    [SerializeField] private float baseFOV = 60f;
    [SerializeField] private float boostFOV = 90f;

    // Durées (en secondes) pour la transition
    [SerializeField] private float zoomInDuration = 0.5f;
    [SerializeField] private float zoomOutDuration = 0.5f;

    // Référence vers l'événement/objet qui déclenche le boost
    [SerializeField] private RSE_BoostActivated boostActivated;

    private Camera cam;

    // On garde une référence vers la coroutine en cours
    private Coroutine fovRoutine;

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
        // Si une coroutine est déjà en cours, on la stoppe
        if (fovRoutine != null)
        {
            StopCoroutine(fovRoutine);
        }

        // On lance une nouvelle coroutine qui part du FOV actuel
        fovRoutine = StartCoroutine(FOVRoutine());
    }

    private IEnumerator FOVRoutine()
    {
        float currentFOV = cam.fieldOfView;

        // 1) Transition du FOV actuel vers le boostFOV
        float elapsed = 0f;
        while (elapsed < zoomInDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / zoomInDuration;
            cam.fieldOfView = Mathf.Lerp(currentFOV, boostFOV, t);
            yield return null;
        }

        // 2) Transition du boostFOV vers le baseFOV
        elapsed = 0f;
        while (elapsed < zoomOutDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / zoomOutDuration;
            cam.fieldOfView = Mathf.Lerp(boostFOV, baseFOV, t);
            yield return null;
        }

        // Fin de la transition : on remet la référence de coroutine à null
        fovRoutine = null;
    }
}
