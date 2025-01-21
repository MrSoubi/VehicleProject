using System.Collections;
using UnityEngine;

public class BoostLightHandler : MonoBehaviour
{
    // Intensité de base et intensité "boostée"
    [SerializeField] private float baseIntensity = 0f;
    [SerializeField] private float boostIntensity = 1f;

    // Durées (en secondes) pour la transition "in / out"
    [SerializeField] private float inDuration = 0.5f;
    [SerializeField] private float outDuration = 0.5f;

    // Référence vers l'événement/objet qui déclenche le boost
    [SerializeField] private RSE_BoostActivated boostActivated;

    // Assurez-vous d’avoir un composant "Light" sur le même objet
    // ou sur un enfant, puis référencez-le dans l’Inspector.
    [SerializeField] private Light pointLight;

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
        StartCoroutine(IntensityRoutine());
    }

    private IEnumerator IntensityRoutine()
    {
        // 1) Transition de baseIntensity vers boostIntensity
        float elapsed = 0f;
        while (elapsed < inDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / inDuration;
            pointLight.intensity = Mathf.Lerp(baseIntensity, boostIntensity, t);
            yield return null;
        }

        // 2) Transition de boostIntensity vers baseIntensity
        elapsed = 0f;
        while (elapsed < outDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / outDuration;
            pointLight.intensity = Mathf.Lerp(boostIntensity, baseIntensity, t);
            yield return null;
        }
    }
}
