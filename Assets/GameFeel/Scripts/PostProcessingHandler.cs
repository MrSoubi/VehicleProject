using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingHandler : MonoBehaviour
{
    [Tooltip("Durée totale de l'effet")]
    public float effectDuration = 0.5f;

    [Tooltip("Courbe permettant de définir l'évolution de l'intensité du Motion Blur en fonction du temps normalisé (0 à 1)")]
    public AnimationCurve motionBlurCurve;

    [Tooltip("Courbe permettant de définir l'évolution de l'intensité de la Lens Distortion en fonction du temps normalisé (0 à 1)")]
    public AnimationCurve lensDistortionCurve;

    [Tooltip("Courbe permettant de définir l'évolution de l'intensité de la Chromatic Aberration en fonction du temps normalisé (0 à 1)")]
    public AnimationCurve chromaticAberrationCurve;

    [Tooltip("Référence au Global Volume contenant les effets")]
    public Volume globalVolume;

    public RSE_BoostActivated boostActivation;

    private MotionBlur motionBlur;
    private LensDistortion lensDistortion;
    private ChromaticAberration chromaticAberration;

    private void Awake()
    {
        if (globalVolume == null)
        {
            Debug.LogError("Global Volume non assigné !");
            return;
        }

        // Récupérer Motion Blur depuis le Volume Profile
        if (!globalVolume.profile.TryGet(out motionBlur))
        {
            Debug.LogError("Motion Blur non trouvé dans le Volume Profile !");
        }

        // Récupérer Lens Distortion depuis le Volume Profile
        if (!globalVolume.profile.TryGet(out lensDistortion))
        {
            Debug.LogError("Lens Distortion non trouvé dans le Volume Profile !");
        }

        // Récupérer Chromatic Aberration depuis le Volume Profile
        if (!globalVolume.profile.TryGet(out chromaticAberration))
        {
            Debug.LogError("Chromatic Aberration non trouvé dans le Volume Profile !");
        }
    }

    private void OnEnable()
    {
        boostActivation.trigger += HandleCarLanding;
    }

    private void OnDisable()
    {
        boostActivation.trigger -= HandleCarLanding;
    }

    private void HandleCarLanding()
    {
        StartCoroutine(HandlePostProcessingEffects());
    }

    private IEnumerator HandlePostProcessingEffects()
    {
        if (motionBlur == null || lensDistortion == null || chromaticAberration == null)
        {
            yield break;
        }

        float elapsedTime = 0f;

        while (elapsedTime < effectDuration)
        {
            // Calcul de la progression normalisée (entre 0 et 1)
            float t = elapsedTime / effectDuration;

            // Évaluation des courbes
            float motionBlurValue = motionBlurCurve.Evaluate(t);
            float lensDistortionValue = lensDistortionCurve.Evaluate(t);
            float chromaticAberrationValue = chromaticAberrationCurve.Evaluate(t);

            // Application des valeurs
            motionBlur.intensity.Override(motionBlurValue);
            lensDistortion.intensity.Override(lensDistortionValue);
            chromaticAberration.intensity.Override(chromaticAberrationValue);

            // Incrémentation avec le temps non affecté par Time.timeScale
            elapsedTime += Time.unscaledDeltaTime;

            yield return null; // Attente de la frame suivante
        }

        // Réinitialisation des valeurs après l'effet
        motionBlur.intensity.Override(0f);
        lensDistortion.intensity.Override(0f);
        chromaticAberration.intensity.Override(0f);
    }
}
