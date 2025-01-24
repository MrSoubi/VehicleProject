using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GameFeelEffectsManager : MonoBehaviour
{
    public FOVHandler fovEffect;
    public TimeFreeze timeFreeze;
    public LSDEffectHandler lsdEffect;
    public CameraShakeHandler cameraShake;
    public SuspensionEffect suspensions;
    public BoostLightHandler boostLight;
    public List<BoostSparksHandler> boostSparks; // Liste des GameObjects pour BoostSparks
    public FireBoostEffectHandler fireBoost;
    public ExhaustLightsHandler exhaustLights;
    public StretchHandler carStretch;
    public StretchHandler halo;

    public Volume globalVolume; // Référence au Global Volume dans la scène

    private MotionBlur motionBlur;
    private DepthOfField depthOfField;
    private LensDistortion lensDistortion;
    private ChromaticAberration chromaticAberration;
    private FilmGrain filmGrain;
    private Bloom bloom;

    void Start()
    {
        if (globalVolume == null)
        {
            Debug.LogError("Global Volume is not assigned!");
            return;
        }

        // Récupération des effets dans le VolumeProfile
        VolumeProfile profile = globalVolume.profile;

        if (profile == null)
        {
            Debug.LogError("Volume Profile is missing on the Global Volume!");
            return;
        }

        // Associer les paramètres des effets
        profile.TryGet(out motionBlur);
        profile.TryGet(out depthOfField);
        profile.TryGet(out lensDistortion);
        profile.TryGet(out chromaticAberration);
        profile.TryGet(out filmGrain);
        profile.TryGet(out bloom);
    }

    public void ToggleFOV()
    {
        fovEffect.enabled = !fovEffect.enabled;
    }

    public void ToggleTimeFreeze()
    {
        timeFreeze.enabled = !timeFreeze.enabled;
    }

    public void ToggleLSDEffect()
    {
        lsdEffect.enabled = !lsdEffect.enabled;
    }

    public void ToggleCameraShake()
    {
        cameraShake.enabled = !cameraShake.enabled;
    }

    public void ToggleSuspension()
    {
        suspensions.enabled = !suspensions.enabled;
    }

    public void ToggleBoostLight()
    {
        boostLight.enabled = !boostLight.enabled;
    }

    public void ToggleBoostSparks()
    {
        if (boostSparks != null)
        {
            foreach (BoostSparksHandler spark in boostSparks)
            {
                if (spark != null)
                {
                    spark.enabled = !spark.enabled;
                }
            }
        }
    }

    public void ToggleFireBoost()
    {
        fireBoost.enabled = !fireBoost.enabled;
    }

    public void ToggleExhaustLights()
    {
        exhaustLights.enabled = !exhaustLights.enabled;
    }

    public void ToggleCarStretching()
    {
        carStretch.enabled = !carStretch.enabled;
    }

    public void ToggleHalo()
    {
        halo.enabled = !halo.enabled;
    }

    public void ToggleMotionBlur()
    {
        if (motionBlur != null)
        {
            motionBlur.active = !motionBlur.active;
        }
    }

    public void ToggleDepthOfField()
    {
        if (depthOfField != null)
        {
            depthOfField.active = !depthOfField.active;
        }
    }

    public void ToggleLensDistortion()
    {
        if (lensDistortion != null)
        {
            lensDistortion.active = !lensDistortion.active;
        }
    }

    public void ToggleChromaticAberration()
    {
        if (chromaticAberration != null)
        {
            chromaticAberration.active = !chromaticAberration.active;
        }
    }

    public void ToggleFilmGrain()
    {
        if (filmGrain != null)
        {
            filmGrain.active = !filmGrain.active;
        }
    }

    public void ToggleBloom()
    {
        if (bloom != null)
        {
            bloom.active = !bloom.active;
        }
    }
}
