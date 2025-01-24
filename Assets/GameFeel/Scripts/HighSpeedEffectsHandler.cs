using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class HighSpeedEffectsHandler : MonoBehaviour
{
    public float motionBlurDelay = 0.5f;
    public float lensDistortionDelay = 0.5f;
    public float chromaticAberrationDelay = 0.5f;
    public float bloomDelay = 0.5f;
    public float grainDelay = 0.5f;

    public AnimationCurve motionBlurCurve;
    public AnimationCurve lensDistortionCurve;
    public AnimationCurve chromaticAberrationCurve;
    public AnimationCurve bloomCurve;
    public AnimationCurve grainCurve;

    public Volume globalVolume;
    public RSO_CarBoosting boostState;

    private MotionBlur motionBlur;
    private LensDistortion lensDistortion;
    private ChromaticAberration chromaticAberration;
    private Bloom bloom;
    private FilmGrain grain;

    private Coroutine motionBlurCoroutine;
    private Coroutine lensDistortionCoroutine;
    private Coroutine chromaticAberrationCoroutine;
    private Coroutine bloomCoroutine;
    private Coroutine grainCoroutine;

    private void Awake()
    {
        if (globalVolume == null)
        {
            Debug.LogError("Global Volume non assigné !");
            return;
        }

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
        boostState.onValueChanged += HandleBoostStateChange;
    }

    private void OnDisable()
    {
        boostState.onValueChanged -= HandleBoostStateChange;
    }

    private void HandleBoostStateChange(bool isActive)
    {
        if (isActive)
        {
            motionBlurCoroutine = StartCoroutine(HandleEffect(motionBlur, motionBlurCurve, motionBlurDelay));
            lensDistortionCoroutine = StartCoroutine(HandleEffect(lensDistortion, lensDistortionCurve, lensDistortionDelay));
            chromaticAberrationCoroutine = StartCoroutine(HandleEffect(chromaticAberration, chromaticAberrationCurve, chromaticAberrationDelay));
            bloomCoroutine = StartCoroutine(HandleEffect(bloom, bloomCurve, bloomDelay));
            grainCoroutine = StartCoroutine(HandleEffect(grain, grainCurve, grainDelay));
        }
        else
        {
            if (motionBlurCoroutine != null) StopCoroutine(motionBlurCoroutine);
            if (lensDistortionCoroutine != null) StopCoroutine(lensDistortionCoroutine);
            if (chromaticAberrationCoroutine != null) StopCoroutine(chromaticAberrationCoroutine);
            if (bloomCoroutine != null) StopCoroutine(bloomCoroutine);
            if (grainCoroutine != null) StopCoroutine(grainCoroutine);

            ResetEffects();
        }
    }

    private IEnumerator HandleEffect(VolumeComponent effect, AnimationCurve curve, float delay)
    {
        if (effect == null)
            yield break;

        float elapsedTime = 0f;

        while (boostState.Value)
        {
            if (elapsedTime < delay)
            {
                elapsedTime += Time.unscaledDeltaTime;
                yield return null;
                continue;
            }

            float t = Mathf.Clamp01((elapsedTime - delay) / delay);
            float intensity = curve.Evaluate(t);
            effect.GetType().GetProperty("intensity").SetValue(effect, new ClampedFloatParameter(intensity, 0f, 1f));

            yield return null;
        }
    }

    private void ResetEffects()
    {
        if (motionBlur != null) motionBlur.intensity.Override(0f);
        if (lensDistortion != null) lensDistortion.intensity.Override(0f);
        if (chromaticAberration != null) chromaticAberration.intensity.Override(0f);
        if (bloom != null) bloom.intensity.Override(0f);
        if (grain != null) grain.intensity.Override(0f);
    }
}
