using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class MotionBlurHandler : MonoBehaviour
{
    [Tooltip("Durée totale du ralentissement du temps")]
    public float blurDuration = 0.5f;

    [Tooltip("Courbe permettant de définir l'évolution de l'intensité du Motion Blur en fonction du temps normalisé (0 à 1)")]
    public AnimationCurve motionBlurCurve;

    [Tooltip("Référence au Global Volume contenant le Motion Blur")]
    public Volume globalVolume;

    public RSE_BoostActivated boostActivation;

    private MotionBlur motionBlur;
    private float originalTimeScale;

    private void Awake()
    {
        if (globalVolume == null)
        {
            Debug.LogError("Global Volume non assigné !");
            return;
        }

        // On récupère le Motion Blur depuis le Volume Profile
        if (!globalVolume.profile.TryGet(out motionBlur))
        {
            Debug.LogError("Motion Blur non trouvé dans le Volume Profile !");
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
        StartCoroutine(FreezeTime());
    }

    private IEnumerator FreezeTime()
    {
        if (motionBlur == null)
        {
            yield break;
        }

        originalTimeScale = Time.timeScale;

        float elapsedTime = 0f;

        while (elapsedTime < blurDuration)
        {
            // Calcul de la progression normalisée (entre 0 et 1)
            float t = elapsedTime / blurDuration;

            float motionBlurValue = motionBlurCurve.Evaluate(t);

            motionBlur.intensity.Override(motionBlurValue);

            // Incrémentation avec le temps non affecté par Time.timeScale
            elapsedTime += Time.unscaledDeltaTime;

            yield return null; // Attente de la frame suivante
        }
        motionBlur.intensity.Override(0f); // Par sécurité, on désactive le Motion Blur après l'effet
    }
}
