using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrailEffectHandler : MonoBehaviour
{
    // Événement qui déclenche le boost
    [SerializeField] private RSE_BoostActivated boostActivated;

    // Liste de systèmes de particules à activer/désactiver
    [SerializeField] private List<ParticleSystem> FireTrail_Particle;

    // Durée pendant laquelle l’émission reste active
    [SerializeField] private float effectDuration = 3f;

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
        // Lance la coroutine qui active et désactive l’émission
        StartCoroutine(ActivateEmissionForDuration(effectDuration));
    }

    private IEnumerator ActivateEmissionForDuration(float duration)
    {
        // 1) Activer l’émission pour tous les ParticleSystems
        foreach (var ps in FireTrail_Particle)
        {
            ps.Play();
        }

        // 2) Attendre la durée spécifiée
        yield return new WaitForSeconds(duration);

        // 3) Désactiver l’émission pour tous les ParticleSystems
        foreach (var ps in FireTrail_Particle)
        {
            ps.Stop();
        }
    }
}
