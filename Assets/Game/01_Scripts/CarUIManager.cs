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
    [SerializeField] private EventChannel _gameOverEvent;
    [SerializeField] MultipleEventChannel _getPlayerId;
    [SerializeField] PlayersData _playersData;


    [SerializeField] GameObject boostGauge;
    [SerializeField] DamageDisplay percentageText;
    [SerializeField] TextMeshProUGUI jumpText;
    [SerializeField] GameObject _gameOverPanel;

    Vector3 boostGaugeInitialPosition;

    void OnEnable()
    {
        _gameOverEvent.onEventTriggered.AddListener(GameOver);
    }

    void OnDisable()
    {
        _gameOverEvent.onEventTriggered.RemoveListener(GameOver);
    }
    void Start()
    {
        boostController.OnBoostValueChanged.AddListener(UpdateBoostUI);
        playerLifeManager.OnPercentageValueChanged.AddListener(UpdateDamageUI);
        carController.OnJumpEnabled.AddListener(SetJumpIndicatorEnabled);
        carController.OnJumpDisabled.AddListener(SetJumpIndicatorDisabled);

        boostGaugeInitialPosition = boostGauge.transform.localPosition;
    }

    void UpdateBoostUI()
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

    void GameOver()
    {
        //Destroy(gameObject);
        int id = _getPlayerId.RaiseIntEventReturn();
        RectTransform panelRectTransform = _gameOverPanel.GetComponent<RectTransform>();
        Rect rect = _playersData.players.FirstOrDefault(x => x.Value.playerId == id).Value.rect;
        rect = panelRectTransform.rect;
        _gameOverPanel.SetActive(true);
        Debug.Log("DeathGameOver");
    }
}
