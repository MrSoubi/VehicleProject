using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEngineAudio : MonoBehaviour
{
    [SerializeField] FMOD.Studio.EventInstance CarEngine;
    [SerializeField] FMOD.Studio.EventInstance CarCrash;

    float RPM;

    [SerializeField] CarController carController;
    [SerializeField] ImpactManager impactManager;

    private void Awake()
    {
        CarEngine = FMODUnity.RuntimeManager.CreateInstance("event:/Car/Engine");
        CarEngine = FMODUnity.RuntimeManager.CreateInstance("event:/FX/Crash");
    }

    private void Start()
    {
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(CarEngine, carController.transform);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(CarCrash, carController.transform);

        CarEngine.start();

        impactManager.OnImpactAsAVictim.AddListener(() => { CarCrash.start(); });
    }

    // TODO: utiliser l'event on ground pour faire diminuer le rpm si on ne touche pas le sol
    void Update()
    {
        CarEngine.setParameterByName("RPM", Mathf.Min(1, carController.GetSpeedRatio()) * 8500);
    }
}
