using Sirenix.OdinInspector;
using UnityEngine;

public class AdvancedCameraShake : MonoBehaviour
{
    private CameraShakeDefinition currentShakeDefinition;

    public CameraShakeDefinition landingShakeDefinition;
    public CameraShakeDefinition boostShakeDefinition;

    public RSE_CarLanding carLanding;
    public RSE_BoostActivated boostActivated;

    float intensity = 0.3f;

    // Timer interne pour suivre le tremblement en cours
    private float shakeTimer = 0f;

    // Position initiale de la caméra
    private Vector3 initialPosition;
    private void OnEnable()
    {
        carLanding.trigger += TriggerLandingShake;
        boostActivated.trigger += TriggerBoostShake;
    }

    private void OnDisable()
    {
        carLanding.trigger -= TriggerLandingShake;
        boostActivated.trigger -= TriggerBoostShake;
    }


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
            float progress = shakeTimer / currentShakeDefinition.duration;

            // Génération d'un offset aléatoire sur les axes choisis
            Vector3 randomOffset = new Vector3(
                currentShakeDefinition.axis.x * Random.Range(-1f, 1f),
                currentShakeDefinition.axis.y * Random.Range(-1f, 1f),
                currentShakeDefinition.axis.z * Random.Range(-1f, 1f)
            ) * intensity * progress;

            // Application de l'offset à la position d'origine
            transform.localPosition = initialPosition + randomOffset;

            // On décrémente le timer en tenant compte de la valeur "release"
            shakeTimer -= Time.deltaTime * currentShakeDefinition.release;
        }
        else
        {
            // Quand le tremblement est terminé, on remet la caméra à sa position initiale
            shakeTimer = 0f;
            transform.localPosition = initialPosition;
        }
    }

    void TriggerLandingShake(float intensity, float airTime)
    {
        if (airTime > 30)
        {
            currentShakeDefinition = landingShakeDefinition;
            this.intensity = intensity;
            shakeTimer = currentShakeDefinition.duration;
        }
    }

    void TriggerBoostShake()
    {
        currentShakeDefinition = boostShakeDefinition;
        this.intensity = 0.5f;
        shakeTimer = currentShakeDefinition.duration;
    }
}
