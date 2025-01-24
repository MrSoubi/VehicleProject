using System.Collections;
using UnityEngine;

public class LSDEffectHandler : MonoBehaviour
{
    // Événement/objet qui déclenche le boost
    [SerializeField] private RSE_BoostActivated boostActivated;

    // Objet à instancier
    [SerializeField] private GameObject LSD_Effect;

    // Référence au Rigidbody dont on veut suivre la distance parcourue
    [SerializeField] private Rigidbody targetRigidbody;

    // Distance à parcourir avant d’instancier le LSD_Effect
    [SerializeField] private float spawnDistance = 5f;

    // Durée totale (en secondes) pendant laquelle on va générer l'effet après un boost
    [SerializeField] private float effectDuration = 10f;

    private Coroutine spawnCoroutine;

    // Variables utilisées pour calculer la distance
    private Vector3 lastPosition;
    private float distanceAccumulated;

    private void OnEnable()
    {
        boostActivated.trigger += HandleBoostActivation;
    }

    private void OnDisable()
    {
        boostActivated.trigger -= HandleBoostActivation;
    }

    /// <summary>
    /// Fonction appelée lorsque le boost est activé
    /// </summary>
    private void HandleBoostActivation()
    {
        // Si un Coroutine existe déjà, on l'arrête pour éviter de le lancer plusieurs fois
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
        }

        // On lance la routine qui va contrôler l'instanciation basée sur la distance parcourue
        spawnCoroutine = StartCoroutine(SpawnLSDEffectByDistanceRoutine());
    }

    /// <summary>
    /// Coroutine qui instancie le LSD_Effect à chaque fois qu'une certaine distance est parcourue
    /// </summary>
    private IEnumerator SpawnLSDEffectByDistanceRoutine()
    {
        // On initialise la dernière position à celle du rigidbody au démarrage
        lastPosition = targetRigidbody.position;
        // On réinitialise la distance accumulée
        distanceAccumulated = 0f;

        float elapsedTime = 0f;

        // Tant que la durée n'est pas écoulée, on continue
        while (elapsedTime < effectDuration)
        {
            // Distance parcourue depuis la dernière frame
            float distanceThisFrame = Vector3.Distance(targetRigidbody.position, lastPosition);

            // On cumule la distance
            distanceAccumulated += distanceThisFrame;

            // On met à jour la dernière position
            lastPosition = targetRigidbody.position;

            // Si on a dépassé la distance requise pour générer l'effet
            if (distanceAccumulated >= spawnDistance)
            {
                Instantiate(LSD_Effect, transform.position, transform.rotation);
                // On remet le compteur à zéro
                distanceAccumulated = 0f;
            }

            // On incrémente le temps écoulé et on attend la prochaine frame
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
