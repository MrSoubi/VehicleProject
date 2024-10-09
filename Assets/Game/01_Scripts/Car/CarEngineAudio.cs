using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEngineAudio : MonoBehaviour
{
    [SerializeField] FMOD.Studio.EventInstance CarEngine;
    float RPM;

    [SerializeField] CarController carController;

    private void Awake()
    {
        CarEngine = FMODUnity.RuntimeManager.CreateInstance("event:/Car/Engine");
    }

    private void Start()
    {
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(CarEngine, carController.transform);
        CarEngine.start();
    }

    void Update()
    {
        CarEngine.setParameterByName("RPM", Mathf.Min(1, carController.GetSpeedRatio()) * 8500);
    }
}
