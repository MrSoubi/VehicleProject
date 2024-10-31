using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CarUIManager : MonoBehaviour
{
    [SerializeField] BoostController boostController;
    [SerializeField] PlayerLifeManager playerLifeManager;
    [SerializeField] CarController carController;
    [SerializeField] PlayersData _playersData;


    [SerializeField] GameObject boostGauge;
    [SerializeField] DamageDisplay percentageText;
    [SerializeField] TextMeshProUGUI jumpText;
    [SerializeField] GameObject _gameOverPanel;

    Vector3 boostGaugeInitialPosition;

    void OnEnable()
    {
    }

    void OnDisable()
    {
    }
    void Start()
    {
        boostController.OnBoostValueChanged.AddListener(UpdateBoostUI);
        playerLifeManager.OnPercentageValueChanged.AddListener(UpdateDamageUI);
        carController.OnJumpEnabled.AddListener(SetJumpIndicatorEnabled);
        carController.OnJumpDisabled.AddListener(SetJumpIndicatorDisabled);

        boostGaugeInitialPosition = boostGauge.transform.localPosition;
    }

    public void UpdateBoostUI()
    {
        boostGauge.transform.localScale = new Vector3(boostController.GetCurrentBoost() / boostController.maxBoostAmount, 1, 1);
        boostGauge.GetComponent<Image>().color = Color.Lerp(Color.red, Color.green, boostController.GetCurrentBoost() / boostController.maxBoostAmount);
        boostGauge.transform.DOShakePosition(0.03f, strength: .5f, randomnessMode: ShakeRandomnessMode.Harmonic).OnComplete(() => { boostGauge.transform.localPosition = boostGaugeInitialPosition; });
    }

    void UpdateDamageUI()
    {
        percentageText.UpdateText((int)playerLifeManager.percentage);
    }

    void SetJumpIndicatorEnabled()
    {
        jumpText.color = Color.white;
    }

    void SetJumpIndicatorDisabled()
    {
        jumpText.color = Color.grey;
    }

    public void GameOver()
    {
        _gameOverPanel.SetActive(true);
        Debug.Log("DeathGameOver");
    }
}
