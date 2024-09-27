using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class S_CarSelection : MonoBehaviour
{
    [SerializeField] private GameObject[] _carSelectionPanels;
    [SerializeField] private GameObject[] _textPressATojoin;
    [SerializeField] private TextMeshProUGUI _timerText;

    private List<int> _availablePanels = new List<int>();
    [SerializeField] private PlayerData _playerData;
    private Dictionary<InputDevice, PlayerInfo> _players => _playerData.players;

    [SerializeField] private S_InputEventCarSelection _inputEvent;

    private int _nextPlayerId = 0;

    private Coroutine _loadSceneCoroutine;
    private bool _isLoadingScene = false;
    private float _countdownTimer = 3f;

    private void Start()
    {
        //Desactive les panel de selection et ajoute des panels disponible
        for (int i = 0; i < _carSelectionPanels.Length; i++)
        {
            _carSelectionPanels[i].SetActive(false);
            _availablePanels.Add(i); 
        }

    }

    private void Update()
    {
        
    }
    private void Awake()
    {

    }

    public void OnSouthButtonPress(InputAction.CallbackContext context)
    {
        Debug.Log("PressEnter");//Verification que la touche soit bien desactive
        if (context.performed)
        {
            InputDevice playerDevice = context.control.device;

            var playerInput = PlayerInput.all.FirstOrDefault(x => x.devices.Contains(playerDevice));

            if (playerInput != null && !_players.ContainsKey(playerDevice))
            {
                if (_availablePanels.Count > 0)
                {
                    int assignedPanel = _availablePanels[0];
                    _availablePanels.RemoveAt(0);
                    var _playerInput = PlayerInput.all[_nextPlayerId];
                    PlayerInfo newPlayer = new PlayerInfo
                    {
                        playerId = _nextPlayerId++,
                        panelIndex = assignedPanel,
                        _playerInput = _playerInput, 
                    };

                    _players.Add(playerDevice, newPlayer);
                    Debug.Log(playerInput.ToString());
                    _carSelectionPanels[assignedPanel].SetActive(true);
                    _textPressATojoin[assignedPanel].SetActive(false);

                    _inputEvent.DisablePlayerInputEnterParty(playerDevice);

                    Debug.Log($"Joueur {newPlayer.playerId} avec {playerDevice.name} a été assigné au cadrant {assignedPanel + 1}");
                    //Debug.Log(PlayerInput.GetPlayerByIndex(newPlayer.playerId));
                    OnPlayerJoined();
                }
            }
        }
    }

    public Dictionary<InputDevice, PlayerInfo> ReturnPlayerInfo()
    {
        return _players;
    }
    
    //Si un joueur rejoins pendant que cela valide la partie stop la coroutine
    public void OnPlayerJoined()
    {
        
        if (_isLoadingScene)
        {
            StopCoroutine(_loadSceneCoroutine);
            _isLoadingScene = false;
            _timerText.gameObject.SetActive(false);
        }
    }

    public void CheckAllPlayersSelection()
    {      
        foreach (var playerInfo in _players.Values)
        {
            if (!playerInfo.isValidateSelection)
            {
                return;
            }
        }

        _loadSceneCoroutine = StartCoroutine(LoadNextSceneAfterDelay());
    }

    //Charge la nouvelle scene quand tout le monde a choisi sa voiture
    private IEnumerator LoadNextSceneAfterDelay()
    {
        _isLoadingScene = true;
        _timerText.gameObject.SetActive(true);
        _countdownTimer = 3f;
        while (_countdownTimer > 0)
        {
            _timerText.text = Mathf.CeilToInt(_countdownTimer).ToString();
            Debug.Log($"Chargement dans {_countdownTimer:F1} secondes...");
            yield return new WaitForSeconds(0.1f);
            _countdownTimer -= 0.1f;
        }


        SceneManager.LoadScene("ArenaSelection");
    }

   
}
    
