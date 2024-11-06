using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class S_InputEventCarSelection : MonoBehaviour
{

    [SerializeField] private S_CarSelection _carSelectionManager;
    [SerializeField] private S_CarSwitch carSwitchManager;
    [SerializeField] private PlayerInputManager playerInputManager;
    [SerializeField] PlayersData playerData;
    private Dictionary<InputDevice, PlayerInfo> _players => playerData.players;

    private void Awake()
    {
        playerInputManager.onPlayerJoined += OnPlayerJoined;

        //DontDestroyOnLoad(gameObject);
    }

    public void OnDestroy()
    {
    }

    public void Start()
    {
        DisableAllInputCarSelection();
        ClearExistingPlayers();
        var playerinput = GameObject.FindGameObjectsWithTag("Player");
        _players.Clear();
        foreach (var player in playerinput)
        {
            PlayerInput PlayerInput = player.GetComponent<PlayerInput>();
            PlayerInput.DeactivateInput();
            //PlayerInput.SwitchCurrentActionMap(null);
            Destroy(player);
        }


    }
    private void OnEnable()
    {
        playerInputManager.onPlayerJoined += OnPlayerJoined;
        playerInputManager.onPlayerLeft += OnPlayerLeft;
    }

   
    private void OnDisable()
    {
        DisableAllInputCarSelection();
        playerInputManager.onPlayerJoined -= OnPlayerJoined;
        playerInputManager.onPlayerLeft -= OnPlayerLeft;
    }

    //Quand le joueur rejoint avec le boutton sud de la manette lui assigne ses touches disponible avec leurs actions
    private void OnPlayerJoined(PlayerInput playerInput)
    {
        InputDevice playerDevice = playerInput.devices[0];
        _carSelectionManager.OnSouthButtonPress(playerDevice, playerInput);
        playerInput.actions["MoveSelection"].performed += carSwitchManager.SwitchCar;
        playerInput.actions["Select"].performed += context => carSwitchManager.OnValidateButtonPress(playerInput, context);
        playerInput.actions["Back"].performed += context => _carSelectionManager.BackToSelection(playerInput, context);

    }

    private void OnPlayerLeft(PlayerInput playerInput)
    {
        playerInput.actions["MoveSelection"].performed -= carSwitchManager.SwitchCar;
        playerInput.actions["Select"].performed -= context => carSwitchManager.OnValidateButtonPress(playerInput, context);
        playerInput.actions["Back"].performed -= context => _carSelectionManager.BackToSelection(playerInput, context);
        if (playerInput != null)
        {
            playerInput.actions["ValidateButton"].performed -= context => carSwitchManager.OnValidateButtonPress(playerInput, context); ;
        }
    }


    public void DisablePlayerInputEndSelection(PlayerInput playerInput)
    {
        playerInput.actions["MoveSelection"].performed -= carSwitchManager.SwitchCar;
        playerInput.actions["Select"].performed -= context => carSwitchManager.OnValidateButtonPress(playerInput, context);

    }

    public void EnableAllPlayersInputEndSelection()
    {
        foreach (var player in _players)
        {
            player.Value._playerInput.actions["MoveSelection"].performed += carSwitchManager.SwitchCar;
            player.Value._playerInput.actions["Select"].performed += context => carSwitchManager.OnValidateButtonPress(player.Value._playerInput, context);
        }
        

    }

    public void EnablePlayerInputEndSelection(InputDevice playerDevice)
    {

        _players[playerDevice]._playerInput.actions["MoveSelection"].performed += carSwitchManager.SwitchCar;
        _players[playerDevice]._playerInput.actions["Select"].performed += context => carSwitchManager.OnValidateButtonPress(_players[playerDevice]._playerInput, context);
        


    }

    public void DisableAllInputCarSelection()
    {
        foreach (var player in _players)
        {
            player.Value._playerInput.actions["MoveSelection"].performed -= carSwitchManager.SwitchCar;
            player.Value._playerInput.actions["MoveSelection"].Disable();

            player.Value._playerInput.actions["Select"].performed -= context => carSwitchManager.OnValidateButtonPress(player.Value._playerInput, context);
            player.Value._playerInput.actions["Select"].Disable();

            player.Value._playerInput.actions["Back"].performed -= context => _carSelectionManager.BackToSelection(player.Value._playerInput, context);
            player.Value._playerInput.actions["Back"].Disable();
        }

    }


    public void RemovePlayer(PlayerInput playerInput)
    {
        if (playerInput != null)
        {
            playerInput.actions["Select"].performed -= context => carSwitchManager.OnValidateButtonPress(playerInput, context);

            Destroy(playerInput.gameObject);
        }
    }
    public void ClearExistingPlayers()
    {
        foreach (PlayerInput playerInput in FindObjectsOfType<PlayerInput>())
        {
            RemovePlayer(playerInput);
        }
    }
}

