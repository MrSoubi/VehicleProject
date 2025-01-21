using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StretchHandler : MonoBehaviour
{
    [SerializeField] private float baseScale;
    [SerializeField] private float boostScale;

    // Durées (en secondes) pour la transition "zoom in/out"
    [SerializeField] private float zoomInDuration = 0.5f;
    [SerializeField] private float zoomOutDuration = 0.5f;

    // Référence vers l'événement/objet qui déclenche le boost
    [SerializeField] private RSE_BoostActivated boostActivated;

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
        StopAllCoroutines();
        StartCoroutine(StretchRoutine());
    }

    private IEnumerator StretchRoutine()
    {
        // 1) Transition de baseScale vers boostScale
        float elapsed = 0f;
        while (elapsed < zoomInDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / zoomInDuration;

            // On modifie le scale local en créant d'abord une copie du Vector3
            Vector3 currentScale = transform.localScale;
            currentScale.z = Mathf.Lerp(baseScale, boostScale, t);
            transform.localScale = currentScale;

            yield return null;
        }

        // 2) Transition de boostScale vers baseScale
        elapsed = 0f;
        while (elapsed < zoomOutDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / zoomOutDuration;

            Vector3 currentScale = transform.localScale;
            currentScale.z = Mathf.Lerp(boostScale, baseScale, t);
            transform.localScale = currentScale;

            yield return null;
        }
    }
}
