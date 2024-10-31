using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class S_TimerStart : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _timerText;
    [SerializeField] float _couldownTimer;
    [SerializeField] EventChannel _onGameStart;
    [SerializeField] EventChannel _onTimerEnd;


    private void OnEnable()
    {
        _onGameStart.onEventTriggered.AddListener(ActiveTheCoulDown);
    }

    private void OnDisable()
    {
        _onGameStart.onEventTriggered.RemoveListener(ActiveTheCoulDown);

    }


    void Start()
    {
        
    }

    public void ActiveTheCoulDown()
    {
        _timerText.gameObject.SetActive(true);
        StartCoroutine(StartDelay());
    }


    private IEnumerator StartDelay()
    {
        _couldownTimer = 3f;
        while (_couldownTimer > 0)
        {
            _timerText.text = Mathf.CeilToInt(_couldownTimer).ToString();
            yield return new WaitForSeconds(0.1f);
            _couldownTimer -= 0.1f;
        }
        _timerText.text = "GO!";
        _onTimerEnd.onEventTriggered.Invoke();

        yield return new WaitForSeconds(1);

        _timerText.gameObject.SetActive(false);

    }
}
