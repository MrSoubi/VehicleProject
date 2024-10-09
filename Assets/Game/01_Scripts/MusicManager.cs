using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] FMOD.Studio.EventInstance musicEvent;

    private void Awake()
    {
        musicEvent = FMODUnity.RuntimeManager.CreateInstance("event:/Music/BackGroundMusic");
    }

    private void Start()
    {
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(musicEvent, transform);
        musicEvent.start();
    }

    private void OnDestroy()
    {
        musicEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }
}