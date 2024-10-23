using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CarUIManager : MonoBehaviour
{
    [SerializeField] BoostController boostController;
    [SerializeField] PlayerLifeManager playerLifeManager;

    [SerializeField] GameObject boostGauge;
    [SerializeField] TextMeshProUGUI percentageText;

    // Start is called before the first frame update
    void Start()
    {
        boostController.OnBoostValueChanged.AddListener(UpdateBoostUI);
        playerLifeManager.OnPercentageValueChanged.AddListener(UpdateDamageUI);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateBoostUI()
    {
        boostGauge.transform.localScale = new Vector3(boostController.currentBoostAmount / boostController.maxBoostAmount, 1, 1);
    }

    void UpdateDamageUI()
    {
        percentageText.text = playerLifeManager.percentage.ToString();
    }
}
