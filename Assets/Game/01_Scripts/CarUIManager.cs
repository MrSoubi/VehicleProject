using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class CarUIManager : MonoBehaviour
{
    [SerializeField] BoostController boostController;
    [SerializeField] PlayerLifeManager playerLifeManager;
    [SerializeField] CarController carController;
    [SerializeField] private EventChannel _gameOverEvent;
    [SerializeField] MultipleEventChannel _getPlayerId;
    [SerializeField] PlayersData _playersData;


    [SerializeField] GameObject boostGauge;
    [SerializeField] TextMeshProUGUI percentageText;
    [SerializeField] TextMeshProUGUI jumpText;
    [SerializeField] GameObject _gameOverPanel; 

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
