using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class FOVBoxVolume : MonoBehaviour
{
    [SerializeField] private List<Volume> volumes;
    [SerializeField] private BoostController boostController;
    [SerializeField] private float boostValueLens;
    [SerializeField] private float standardValueLens;
    [SerializeField] private float duration = 0.5f;

    List<LensDistortion> lensesDistortion = new List<LensDistortion>();

    private void Start()
    {
        boostController.OnBoostActivation.AddListener(ActivateDistortion);
        boostController.OnBoostDeactivation.AddListener(DeactivateDistortion);

        foreach(Volume volume in volumes)
        {
            LensDistortion localLense;
            volume.profile.TryGet(out localLense);
            lensesDistortion.Add(localLense);
            lensesDistortion[^1] = localLense;
        }
    }

    void ActivateDistortion()
    {
        //StopAllCoroutines();

        foreach (Volume volume in volumes)
        {
            LensDistortion localLens;
            volume.profile.TryGet(out localLens);

            localLens.intensity.value = standardValueLens;
            StartCoroutine(LerpDistortion(localLens, standardValueLens, boostValueLens));
        }
    }

    void DeactivateDistortion()
    {
        //StopAllCoroutines();

        foreach (Volume volume in volumes)
        {
            LensDistortion localLens;
            volume.profile.TryGet(out localLens);

            localLens.intensity.value = boostValueLens;
            StartCoroutine(LerpDistortion(localLens, boostValueLens, standardValueLens));
        }
    }

    IEnumerator LerpDistortion(LensDistortion lense, float from, float to)
    {
        float time = 0;
        while (time < duration)
        {
            yield return new WaitForEndOfFrame();
            lense.intensity.value = Mathf.Lerp(from, to, time / duration);
            time += Time.deltaTime;
        }
        yield return null;
    }
}
