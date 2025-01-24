using UnityEngine;
using System.Collections;

public class TimeFreeze : MonoBehaviour
{
    [Tooltip("Durée totale du ralentissement du temps")]
    public float freezeDuration = 0.5f;

    [Tooltip("Courbe permettant de définir l'évolution du timeScale en fonction du temps normalisé (0 à 1)")]
    public AnimationCurve freezeCurve;

    public RSE_BoostActivated boostActivation;

    private void OnEnable()
    {
        boostActivation.trigger += HandleCarLanding;
    }

    private void OnDisable()
    {
        boostActivation.trigger -= HandleCarLanding;
    }

    void HandleCarLanding()
    {
        StartCoroutine(FreezeTime());
    }

    private IEnumerator FreezeTime()
    {
        // On enregistre le timeScale actuel pour le restaurer à la fin
        float originalTimeScale = Time.timeScale;

        float elapsedTime = 0f;

        // On fait varier Time.timeScale en fonction de la freezeCurve pendant freezeDuration secondes
        while (elapsedTime < freezeDuration)
        {
            // t est la fraction de progression entre 0 et 1
            float t = elapsedTime / freezeDuration;

            // On évalue la courbe à cette fraction
            float curveValue = freezeCurve.Evaluate(t);

            // On applique la valeur au timeScale
            // Par exemple : on prend le timeScale d'origine multiplié par la valeur de la courbe
            Time.timeScale = originalTimeScale * curveValue;

            // On utilise Time.unscaledDeltaTime pour ne pas être affecté par le changement de Time.timeScale
            elapsedTime += Time.unscaledDeltaTime;

            yield return null; // attendre la frame suivante
        }

        // Par sécurité, on s'assure de remettre le timeScale d'origine
        Time.timeScale = originalTimeScale;
    }
}
