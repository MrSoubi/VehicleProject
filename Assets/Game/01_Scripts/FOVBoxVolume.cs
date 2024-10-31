using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class FOVBoxVolume : MonoBehaviour
{
    [SerializeField] private List<Volume> volume;
    [SerializeField] private BoostController boostController;
    [SerializeField] private float maxValueLens;
    [SerializeField] private float minValueLens;

    private LensDistortion lensDistortion;
    private float time = 0f;
    void Update()
    {
        if (boostController.isBoosting)
        {
            for (int i = 0; i < volume.Count; i++)
            {
                volume[i].profile.TryGet(out lensDistortion);
 
                lensDistortion.intensity.value = Mathf.Lerp(maxValueLens, minValueLens, time / 1f);
                time += Time.deltaTime;
            }

        }
        else
        {
            for (int i = 0; i < volume.Count; i++)
            {
                volume[i].profile.TryGet(out lensDistortion);
                lensDistortion.intensity.value = 0;
                time = 0;
            }
        }
    }
}
