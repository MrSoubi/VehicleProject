using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class FOVBoxVolume : MonoBehaviour
{
    [SerializeField] private List<Volume> volume;
    [SerializeField] private BoostController boostController;

    void Update()
    {
        if (boostController.isBoosting)
        {
            for (int i = 0; i < volume.Count; i++)
            {
                volume[i].enabled = true;
            }

        }
        else
        {
            for (int i = 0; i < volume.Count; i++)
            {
                volume[i].enabled = false;
            }
        }
    }
}
