using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExhaustLightsHandler : MonoBehaviour
{
    [SerializeField] private RSE_BoostActivated boostActivated;

    [Header("GameObjects à gérer")]
    [Tooltip("Liste des GameObjects à activer/désactiver.")]
    public List<GameObject> gameObjects = new List<GameObject>();

    [Header("Durées aléatoires")]
    [Tooltip("Durée minimale d'activation.")]
    public float minActivationTime = 1f;

    [Tooltip("Durée maximale d'activation.")]
    public float maxActivationTime = 5f;

    [Header("Durée maximale de gestion")]
    [Tooltip("Durée totale après laquelle la coroutine s'arrête.")]
    public float maxRunTime = 30f;

    private void OnEnable()
    {
        boostActivated.trigger += HandleBoostActivation;
    }

    private void OnDisable()
    {
        boostActivated.trigger -= HandleBoostActivation;
    }

    private void Start()
    {
        // Désactiver tous les GameObjects au démarrage
        foreach (GameObject go in gameObjects)
        {
            if (go != null)
            {
                go.SetActive(false);
            }
        }
    }

    Coroutine boostCoroutine;

    private void HandleBoostActivation()
    {
        if (boostCoroutine != null)
        {
            StopCoroutine(boostCoroutine);
        }

        boostCoroutine = StartCoroutine(ManageGameObjects());
    }

    private IEnumerator ManageGameObjects()
    {
        float elapsedTime = 0f;

        while (elapsedTime < maxRunTime)
        {
            if (gameObjects.Count > 0)
            {
                // Sélectionner un GameObject aléatoire dans la liste
                int randomIndex = Random.Range(0, gameObjects.Count);
                GameObject selectedGO = gameObjects[randomIndex];

                if (selectedGO != null)
                {
                    // Activer le GameObject
                    selectedGO.SetActive(true);

                    // Attendre une durée aléatoire
                    float randomDuration = Random.Range(minActivationTime, maxActivationTime);
                    yield return new WaitForSeconds(randomDuration);

                    // Désactiver le GameObject
                    selectedGO.SetActive(false);

                    // Incrémenter le temps écoulé
                    elapsedTime += randomDuration;
                }
            }

            // Optionnel : petite pause avant de passer au prochain GameObject
            yield return new WaitForSeconds(0.1f);
            elapsedTime += 0.1f;
        }

        Debug.Log("La gestion des GameObjects est terminée (durée maximale atteinte).");
    }
}
