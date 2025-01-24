using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Android;

public class BoostSparksHandler : MonoBehaviour
{
    [SerializeField] private float baseEmissionRate = 10f;
    [SerializeField] private float boostEmissionRate = 100f;

    // Durées (en secondes) pour la transition "in / out"
    [SerializeField] private float inDuration = 0.5f;
    [SerializeField] private float outDuration = 0.5f;

    // Référence vers l'événement/objet qui déclenche le boost
    [SerializeField] private RSE_BoostActivated boostActivated;

    private ParticleSystem particleSystemRef;
    private Coroutine emissionRoutine;

    private void Awake()
    {
        // Récupération du ParticleSystem sur l'objet courant
        particleSystemRef = GetComponent<ParticleSystem>();
    }

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
        ParticleSystem.EmissionModule emission = particleSystemRef.emission;
        emission.rateOverTime = new ParticleSystem.MinMaxCurve(baseEmissionRate);
    }

    private void HandleBoostActivation()
    {
        // Si une coroutine est déjà en cours, on la stoppe
        if (emissionRoutine != null)
        {
            StopCoroutine(emissionRoutine);
        }

        // On lance une nouvelle coroutine
        emissionRoutine = StartCoroutine(EmissionRoutine());
    }

    private IEnumerator EmissionRoutine()
    {
        // On récupère le module Emission de la ParticleSystem
        ParticleSystem.EmissionModule emission = particleSystemRef.emission;

        // Détermine la valeur de départ (là où on en est actuellement)
        float currentRate = emission.rateOverTime.constant;

        // ------------------------
        // 1) Transition du taux actuel vers boostEmissionRate
        // ------------------------
        float elapsed = 0f;
        while (elapsed < inDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / inDuration;

            float newRate = Mathf.Lerp(currentRate, boostEmissionRate, t);
            // On affecte la nouvelle valeur d'émission
            emission.rateOverTime = new ParticleSystem.MinMaxCurve(newRate);

            yield return null;
        }

        // ------------------------
        // 2) Transition du boostEmissionRate vers baseEmissionRate
        // ------------------------
        elapsed = 0f;
        while (elapsed < outDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / outDuration;

            float newRate = Mathf.Lerp(boostEmissionRate, baseEmissionRate, t);
            emission.rateOverTime = new ParticleSystem.MinMaxCurve(newRate);

            yield return null;
        }

        // Fin de la transition : la coroutine est terminée
        emissionRoutine = null;
    }
}
