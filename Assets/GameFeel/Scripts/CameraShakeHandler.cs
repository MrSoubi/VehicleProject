using System.Collections;
using UnityEngine;

public class CameraShakeHandler : MonoBehaviour
{
    [Tooltip("Durée totale de l'effet de Camera Shake")]
    public float shakeDuration = 0.5f;

    [Tooltip("Courbe permettant de définir l'évolution de l'intensité du Camera Shake en fonction du temps normalisé (0 à 1)")]
    public AnimationCurve shakeIntensityCurve;

    [Tooltip("Amplitude maximale du mouvement de position (en unités)")]
    public float maxPositionOffset = 0.5f;

    [Tooltip("Amplitude maximale du mouvement de rotation (en degrés)")]
    public float maxRotationOffset = 5f;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Coroutine shakeCoroutine;

    public RSE_BoostActivated boostActivation;

    private void Awake()
    {
        // Sauvegarder la position et la rotation initiales
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;
    }

    private void OnEnable()
    {
        boostActivation.trigger += TriggerShake;
    }

    private void OnDisable()
    {
        boostActivation.trigger -= TriggerShake;
    }

    public void TriggerShake()
    {
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
        }

        shakeCoroutine = StartCoroutine(HandleCameraShake());
    }

    private IEnumerator HandleCameraShake()
    {
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            // Calcul de la progression normalisée (entre 0 et 1)
            float t = elapsedTime / shakeDuration;

            // Évaluation de la courbe pour l'intensité
            float intensity = shakeIntensityCurve.Evaluate(t);

            // Calcul des offsets de position et de rotation
            Vector3 positionOffset = new Vector3(
                Random.Range(-1f, 1f) * maxPositionOffset * intensity,
                Random.Range(-1f, 1f) * maxPositionOffset * intensity,
                Random.Range(-1f, 1f) * maxPositionOffset * intensity
            );

            Vector3 rotationOffset = new Vector3(
                Random.Range(-1f, 1f) * maxRotationOffset * intensity,
                Random.Range(-1f, 1f) * maxRotationOffset * intensity,
                Random.Range(-1f, 1f) * maxRotationOffset * intensity
            );

            // Appliquer les offsets
            transform.localPosition = originalPosition + positionOffset;
            transform.localRotation = originalRotation * Quaternion.Euler(rotationOffset);

            // Incrémentation du temps
            elapsedTime += Time.deltaTime;

            yield return null; // Attendre la prochaine frame
        }

        // Réinitialisation de la position et de la rotation
        transform.localPosition = originalPosition;
        transform.localRotation = originalRotation;

        shakeCoroutine = null;
    }
}
