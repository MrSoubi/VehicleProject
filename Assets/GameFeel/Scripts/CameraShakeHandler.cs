using Sirenix.OdinInspector;
using UnityEngine;

public class AdvancedCameraShake : MonoBehaviour
{
    [Header("Réglages du Shake")]
    [Tooltip("Amplitude du tremblement (intensité).")]
    public float intensity = 0.3f;

    [Tooltip("Durée de l'effet de tremblement (en secondes).")]
    public float duration = 0.5f;

    [Tooltip("Axe de tremblement (ex: (1,1,0) pour secouer en X et Y uniquement).")]
    public Vector3 axis = Vector3.one;

    [Tooltip("Vitesse à laquelle le tremblement s'atténue (relâche).")]
    public float release = 1.0f;

    // Timer interne pour suivre le tremblement en cours
    private float shakeTimer = 0f;

    // Position initiale de la caméra
    private Vector3 initialPosition;

    void Awake()
    {
        // On stocke la position de départ
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        if (shakeTimer > 0f)
        {
            // Proportion de temps restant (0 à 1)
            float progress = shakeTimer / duration;

            // Génération d'un offset aléatoire sur les axes choisis
            Vector3 randomOffset = new Vector3(
                axis.x * Random.Range(-1f, 1f),
                axis.y * Random.Range(-1f, 1f),
                axis.z * Random.Range(-1f, 1f)
            ) * intensity * progress;

            // Application de l'offset à la position d'origine
            transform.localPosition = initialPosition + randomOffset;

            // On décrémente le timer en tenant compte de la valeur "release"
            shakeTimer -= Time.deltaTime * release;
        }
        else
        {
            // Quand le tremblement est terminé, on remet la caméra à sa position initiale
            shakeTimer = 0f;
            transform.localPosition = initialPosition;
        }
    }

    [Button]
    public void TriggerShake()
    {
        shakeTimer = duration;
    }

    /// <summary>
    /// Lance le tremblement avec des paramètres personnalisés.
    /// </summary>
    /// <param name="customIntensity">Amplitude du tremblement.</param>
    /// <param name="customDuration">Durée du tremblement (en secondes).</param>
    /// <param name="customAxis">Axe de tremblement (ex: Vector3.one = X, Y et Z).</param>
    /// <param name="customRelease">Vitesse à laquelle le tremblement s'atténue.</param>
    public void TriggerShake(float customIntensity, float customDuration, Vector3 customAxis, float customRelease)
    {
        intensity = customIntensity;
        duration = customDuration;
        axis = customAxis;
        release = customRelease;

        // On remet le timer à la nouvelle durée
        shakeTimer = duration;
    }
}
