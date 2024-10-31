using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
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
    [SerializeField] GameObject jumpGauge;

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
        carController.OnJump.AddListener(SetJumpIndicatorDisabled);
        carController.OnLanding.AddListener(RechargeJump);

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
        jumpGauge.transform.localScale = new(1, 1, 1);
        jumpGauge.GetComponent<Image>().color = new Color(0, 1, 1, 1);
    }

    void RechargeJump()
    {
        if (!carController.canJump)
        {
            StartCoroutine(RechargeJumpGauge());
        }
    }

    float jumpGaugeFill;
    IEnumerator RechargeJumpGauge()
    {
        jumpGaugeFill = 0;
        while (jumpGaugeFill <= 1)
        {
            yield return new WaitForEndOfFrame();
            jumpGauge.transform.localScale = new(jumpGaugeFill, 1, 1);
            jumpGaugeFill += Time.deltaTime;
        }

        SetJumpIndicatorEnabled();
    }

    void SetJumpIndicatorDisabled()
    {
        jumpGauge.transform.localScale = new(0, 1, 1);
        jumpGauge.GetComponent<Image>().color = new Color(0, 1, 1, 0.5f);
    }

    public void GameOver()
    {
        _gameOverPanel.SetActive(true);
        Debug.Log("DeathGameOver");
    }
}
