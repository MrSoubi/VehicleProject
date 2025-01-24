using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingHandler : MonoBehaviour
{
    [Tooltip("Durée totale de l'effet Motion Blur")]
    public float motionBlurDuration = 0.5f;

    [Tooltip("Durée totale de l'effet Lens Distortion")]
    public float lensDistortionDuration = 0.5f;

    [Tooltip("Durée totale de l'effet Chromatic Aberration")]
    public float chromaticAberrationDuration = 0.5f;

    [Tooltip("Durée totale de l'effet Bloom")]
    public float bloomDuration = 0.5f;

    [Tooltip("Durée totale de l'effet Film Grain")]
    public float grainDuration = 0.5f;

    [Tooltip("Courbe permettant de définir l'évolution de l'intensité du Motion Blur en fonction du temps normalisé (0 à 1)")]
    public AnimationCurve motionBlurCurve;

    [Tooltip("Courbe permettant de définir l'évolution de l'intensité de la Lens Distortion en fonction du temps normalisé (0 à 1)")]
    public AnimationCurve lensDistortionCurve;

    [Tooltip("Courbe permettant de définir l'évolution de l'intensité de la Chromatic Aberration en fonction du temps normalisé (0 à 1)")]
    public AnimationCurve chromaticAberrationCurve;

    [Tooltip("Courbe permettant de définir l'évolution de l'intensité du Bloom en fonction du temps normalisé (0 à 1)")]
    public AnimationCurve bloomCurve;

    [Tooltip("Courbe permettant de définir l'évolution de l'intensité du Film Grain en fonction du temps normalisé (0 à 1)")]
    public AnimationCurve grainCurve;

    [Tooltip("Référence au Global Volume contenant les effets")]
    public Volume globalVolume;

    public RSE_BoostActivated boostActivation;

    private MotionBlur motionBlur;
    private LensDistortion lensDistortion;
    private ChromaticAberration chromaticAberration;
    private Bloom bloom;
    private FilmGrain grain;

    private void Awake()
    {
        if (globalVolume == null)
        {
            Debug.LogError("Global Volume non assigné !");
            return;
        }

        // Récupérer les effets depuis le Volume Profile
        if (!globalVolume.profile.TryGet(out motionBlur))
            Debug.LogError("Motion Blur non trouvé dans le Volume Profile !");
        if (!globalVolume.profile.TryGet(out lensDistortion))
            Debug.LogError("Lens Distortion non trouvé dans le Volume Profile !");
        if (!globalVolume.profile.TryGet(out chromaticAberration))
            Debug.LogError("Chromatic Aberration non trouvé dans le Volume Profile !");
        if (!globalVolume.profile.TryGet(out bloom))
            Debug.LogError("Bloom non trouvé dans le Volume Profile !");
        if (!globalVolume.profile.TryGet(out grain))
            Debug.LogError("Film Grain non trouvé dans le Volume Profile !");
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
        StartCoroutine(HandleMotionBlur());
        StartCoroutine(HandleLensDistortion());
        StartCoroutine(HandleChromaticAberration());
        StartCoroutine(HandleBloom());
        StartCoroutine(HandleFilmGrain());
    }

    private IEnumerator HandleMotionBlur()
    {
        if (motionBlur == null)
            yield break;

        float elapsedTime = 0f;
        while (elapsedTime < motionBlurDuration)
        {
            float t = elapsedTime / motionBlurDuration;
            float intensity = motionBlurCurve.Evaluate(t);
            motionBlur.intensity.Override(intensity);

            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        motionBlur.intensity.Override(0f);
    }

    private IEnumerator HandleLensDistortion()
    {
        if (lensDistortion == null)
            yield break;

        float elapsedTime = 0f;
        while (elapsedTime < lensDistortionDuration)
        {
            float t = elapsedTime / lensDistortionDuration;
            float intensity = lensDistortionCurve.Evaluate(t);
            lensDistortion.intensity.Override(intensity);

            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        lensDistortion.intensity.Override(0f);
    }

    private IEnumerator HandleChromaticAberration()
    {
        if (chromaticAberration == null)
            yield break;

        float elapsedTime = 0f;
        while (elapsedTime < chromaticAberrationDuration)
        {
            float t = elapsedTime / chromaticAberrationDuration;
            float intensity = chromaticAberrationCurve.Evaluate(t);
            chromaticAberration.intensity.Override(intensity);

            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        chromaticAberration.intensity.Override(0f);
    }

    private IEnumerator HandleBloom()
    {
        if (bloom == null)
            yield break;

        float elapsedTime = 0f;
        while (elapsedTime < bloomDuration)
        {
            float t = elapsedTime / bloomDuration;
            float intensity = bloomCurve.Evaluate(t);
            bloom.intensity.Override(intensity);

            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        bloom.intensity.Override(0f);
    }

    private IEnumerator HandleFilmGrain()
    {
        if (grain == null)
            yield break;

        float elapsedTime = 0f;
        while (elapsedTime < grainDuration)
        {
            float t = elapsedTime / grainDuration;
            float intensity = grainCurve.Evaluate(t);
            grain.intensity.Override(intensity);

            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        grain.intensity.Override(0f);
    }
}
