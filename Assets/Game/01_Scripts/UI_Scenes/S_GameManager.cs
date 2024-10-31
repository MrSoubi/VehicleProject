using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;


public class S_GameManager : MonoBehaviour
{
    [SerializeField] PlayersData _playersData;
    private Dictionary<InputDevice, PlayerInfo> _players => _playersData.players;

    
    [SerializeField] private float _playTime;
    [SerializeField] EventChannel _onTimerEnd;
    [SerializeField] TextMeshProUGUI _textPlayTimeTimer;
    [SerializeField] private EventChannel _gameOverEvent;


    private bool _isOnePlayerAlive = false;
    private int _aliveCount = 0;
    private bool _isPlayTimeEnd = false;
    private bool _endGameCondition => _isOnePlayerAlive == true || _isPlayTimeEnd == true;
    private bool _isTimerRunning = false;
    private Coroutine _coroutine;

    private void OnEnable()
    {
        _onTimerEnd.onEventTriggered.AddListener(StartPlayTimeTimer);
        _gameOverEvent.onEventTriggered.AddListener(CheckAlivePlayers);

    }
    void OnDisable()
    {
        _onTimerEnd.onEventTriggered.RemoveListener(StartPlayTimeTimer);
        _gameOverEvent.onEventTriggered.RemoveListener(CheckAlivePlayers);


    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    public void StartPlayTimeTimer()
    {
        _coroutine = StartCoroutine(StartTimer());
    }

    private IEnumerator StartTimer()
    {
        _isTimerRunning = true;

        _textPlayTimeTimer.gameObject.SetActive(true);

        float timeRemaining = _playTime;

        while (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;

            if (_textPlayTimeTimer != null)
            {
                int minutes = Mathf.FloorToInt(timeRemaining / 60f);
                int seconds = Mathf.FloorToInt(timeRemaining % 60f);
                _textPlayTimeTimer.text = $"{minutes:00}:{seconds:00}";
            }

            yield return null;
        }
        _textPlayTimeTimer.text = "00:00";

        _isPlayTimeEnd = true;
        _isTimerRunning = true;

        GameFinish();

    }


    public void CheckAlivePlayers()
    {
        

        foreach (var player in _players)
        {
            if(player.Value.isAlive == true)
            {
                _aliveCount++;
            }
        }

        CheckAliveCondition();
        Debug.Log("PlayerAlive: "+_aliveCount);

        _aliveCount = 0;

        GameFinish();
    }

    public void CheckAliveCondition()
    {
        if (_aliveCount  == 1)
        {
            _isOnePlayerAlive = true;
        }
        StopCoroutine(_coroutine);
    }
    public void GameFinish()
    {
        if (_endGameCondition == true)
        {
            Debug.Log("GameFinish: ");
            DisablePlayersInput();
        }
    }

    private void DisablePlayersInput()
    {
        foreach (var player in _players)
        {
            player.Value._playerInput.DeactivateInput();
        }
        

    }
}
