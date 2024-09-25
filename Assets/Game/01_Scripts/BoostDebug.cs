using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BoostDebug : MonoBehaviour
{
    [SerializeField] BoostController controller;
    [SerializeField] TextMeshProUGUI textMeshPro;

    void Update()
    {
        textMeshPro.text = controller.currentBoostAmount.ToString();
    }
}
