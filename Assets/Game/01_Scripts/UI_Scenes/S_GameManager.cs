using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.Events;


public class S_GameManager : MonoBehaviour
{
    [SerializeField] PlayersData _playersData;
    private Dictionary<InputDevice, PlayerInfo> _players => _playersData.players;

    
    [SerializeField] private float _playTime;
    [SerializeField] EventChannel _onTimerEnd;
    [SerializeField] TextMeshProUGUI _textPlayTimeTimer;
    [SerializeField] private EventChannel _gameOverEvent;

    [SerializeField] GameObject _panelGameStats;
    [SerializeField] EventChannel _gameLoopEnd;
    [SerializeField] UnityEvent OnEndGame;
    [SerializeField] List<TextMeshProUGUI> _textOk = new List<TextMeshProUGUI>();


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

        foreach (var player in _players)
        {
            player.Value._playerInput.actions["Select"].performed -= ValidateCheckStats;
            //player.Value._playerInput.SwitchCurrentActionMap(null);
            player.Value._playerInput.DeactivateInput();
        }
        _players.Clear();

        var playerinput = GameObject.FindGameObjectsWithTag("Player");
        foreach (var player in playerinput)
        {
            PlayerInput PlayerInput = player.GetComponent<PlayerInput>();
            PlayerInput.DeactivateInput();
            //PlayerInput.SwitchCurrentActionMap(null);
            Destroy(player);
        }
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
        if (_aliveCount  <= 1)
        {
            _isOnePlayerAlive = true;
            StopCoroutine(_coroutine);
        }
    }
    public void GameFinish()
    {
        if (_endGameCondition == true)
        {
            Debug.Log("GameFinish: ");
            _gameLoopEnd.onEventTriggered?.Invoke();
            StartCoroutine(ActiveGameStats());
        }
    }

    public IEnumerator ActiveGameStats()
    {
        OnEndGame.Invoke();
        yield return new WaitForSeconds(0.5f);
        _panelGameStats.SetActive(true);
        DisablePlayersInput();

    }

    private void DisablePlayersInput()
    {
        foreach (var player in _players)
        {
            player.Value._playerInput.SwitchCurrentActionMap("MenuSelection");
            player.Value._playerInput.actions["Select"].Enable();
            //player.Value._playerInput.actions["Select"].Enable();

            player.Value._playerInput.actions["Select"].performed += ValidateCheckStats;

        }
    }


    private void ValidateCheckStats(InputAction.CallbackContext context)
    {
        _players.FirstOrDefault(x => x.Key == context.control.device).Value.isCheckStats = true;
        
        _textOk[_players.FirstOrDefault(x => x.Key == context.control.device).Value.playerId].gameObject.SetActive(true);
        CheckStatsValidation();

    }

    private void CheckStatsValidation()
    {
        int countCheck = 0;
        foreach (var player in _players)
        {
            if(player.Value.isCheckStats == true)
            {
                countCheck++;
            }

        }

        if (countCheck == _players.Count())
        {

            //_gameLoopEnd.onEventTriggered.Invoke();
            
            SceneManager.LoadScene("MainMenu");
            //SceneManager.LoadScene("CarSelection");

        }
    }

}
