using UnityEngine;
using System.Collections;

public class TimeFreeze : MonoBehaviour
{
    [Tooltip("Durée du freeze en secondes")]
    public float freezeDuration = 0.5f;
    public RSE_CarLanding carLanding;

    private void OnEnable()
    {
        carLanding.trigger += HandleCarLanding;
    }

    private void OnDisable()
    {
        carLanding.trigger -= HandleCarLanding;
    }

    void HandleCarLanding(float velocity, float airTime)
    {
        if (airTime > 2)
        {
            StartCoroutine(FreezeTime());
        }
    }

    private IEnumerator FreezeTime()
    {
        // On enregistre le timeScale actuel pour le restaurer plus tard
        float originalTimeScale = Time.timeScale;

        // Mise en pause du temps
        Time.timeScale = 0f;

        // On attend freezeDuration secondes "réelles" (sans être affecté par le timeScale)
        yield return new WaitForSecondsRealtime(freezeDuration);

        // Restauration du timeScale d’origine
        Time.timeScale = originalTimeScale;
    }
}