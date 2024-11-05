using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.Windows;


public class S_CarSelection : MonoBehaviour
{
    [SerializeField] private GameObject[] _carSelectionPanels;
    [SerializeField] private GameObject[] _textPressATojoin;
    [SerializeField] private GameObject[] _textReady;
    [SerializeField] private TextMeshProUGUI _timerText;

    private List<int> _availablePanels = new List<int>();
    [SerializeField] private PlayersData _playerData;
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
        var playerinput = GameObject.FindGameObjectsWithTag("Player");

        foreach (var player in playerinput)
        {
            PlayerInput PlayerInput = player.GetComponent<PlayerInput>();
            PlayerInput.DeactivateInput();
            //PlayerInput.SwitchCurrentActionMap(null);
            Destroy(player);
        }
    }

    //Quand le joueur appuie sur le bouton sud et rejoint la partie puis lui assigne un panel de l'ecran pour selectionnaer sa voiture
    public void OnSouthButtonPress(InputDevice playerDevice, PlayerInput playerInput)
    {
        Debug.Log("PressEnter");//Verification que la touche soit bien desactive

        if (playerDevice != null && !_players.ContainsKey(playerDevice))
        {
            if (_availablePanels.Count > 0)
            {
                int assignedPanel = _availablePanels[0];
                _availablePanels.RemoveAt(0);

                playerInput.SwitchCurrentActionMap("MenuSelection");

                PlayerInfo newPlayer = new PlayerInfo
                {
                    playerId = _nextPlayerId++,
                    panelIndex = assignedPanel,
                    _playerInput = playerInput, 
                };

                _players.Add(playerDevice, newPlayer);
                Debug.Log(playerInput.ToString());
                _carSelectionPanels[assignedPanel].SetActive(true);
                _textPressATojoin[assignedPanel].SetActive(false);


                Debug.Log($"Joueur {newPlayer.playerId} avec {playerDevice.name} a été assigné au cadrant {assignedPanel + 1}");
                OnPlayerJoinedOrButtonbackPressed();
            }
        }
        
    }

    public Dictionary<InputDevice, PlayerInfo> ReturnPlayerInfo()
    {
        return _players;
    }
    
    //Si un joueur rejoins pendant que cela valide la partie stop la coroutine de lancement de la scene 
    public void OnPlayerJoinedOrButtonbackPressed()
    {
        
        if (_isLoadingScene)
        {
            StopCoroutine(_loadSceneCoroutine);
            _isLoadingScene = false;
            _timerText.gameObject.SetActive(false);
            _inputEvent.EnableAllPlayersInputEndSelection();
            UnValideSelection();
        }
    }

    //Cancel la selction du joueur
    public void UnValideSelection()
    {
        foreach (var player in _players.Keys)
        {
            _players[player].isValidateSelection = false;
        }

        for (int i = 0; i < _textReady.Length; i++) 
        {
            _textReady[i].gameObject.SetActive(false);
        }
    }

    //Verifie que tout les joueur ont choisi leur voiture puis lance la coroutine pour la selection de la map
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

    //Load la scene ArenaSelection apres 3s et l'affiche sur l'ecran le delay si auun joueur de rejoin ou cancel pour changer sa selection
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
        
        _inputEvent.DisableAllInputCarSelection();
        SceneManager.LoadScene("ArenaSelection");
        
    }

    //Cancel la selection
    public void BackToSelection(PlayerInput playerInput, InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (playerInput == _players.FirstOrDefault(x => x.Key == context.control.device).Value._playerInput)
            {
                _players.FirstOrDefault(x => x.Key == context.control.device).Value.isValidateSelection = false;
                OnPlayerJoinedOrButtonbackPressed();
                _inputEvent.EnablePlayerInputEndSelection(context.control.device);
                _textReady[_players.FirstOrDefault(x => x.Key == context.control.device).Value.playerId].gameObject.SetActive(false);
            }
        }
       
    }


}
    
