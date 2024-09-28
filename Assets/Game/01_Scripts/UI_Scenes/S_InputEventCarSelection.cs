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
    private Dictionary<InputDevice, PlayerInfo> players => _carSelectionManager.ReturnPlayerInfo();

    private void Awake()
    {
        playerInputManager.onPlayerJoined += OnPlayerJoined;

       
    }

    public void OnDestroy()
    {
    }

    private void OnPlayerJoined(PlayerInput playerInput)
    {
        InputDevice playerDevice = playerInput.devices[0];
        _carSelectionManager.OnSouthButtonPress(playerDevice, playerInput);
        playerInput.actions["MoveSelection"].performed += carSwitchManager.SwitchCar;
        playerInput.actions["Select"].performed += context => carSwitchManager.OnValidateButtonPress(playerInput, context);
        playerInput.actions["Back"].performed += context => _carSelectionManager.BackToSelection(playerInput, context);

    }
    

    public void DisablePlayerInputEndSelection(PlayerInput playerInput)
    {
        playerInput.actions["MoveSelection"].performed -= carSwitchManager.SwitchCar;
        playerInput.actions["Select"].performed -= context => carSwitchManager.OnValidateButtonPress(playerInput, context);

    }

    public void EnablePlayerInputEndSelection()
    {
        foreach (var player in players)
        {
            player.Value._playerInput.actions["MoveSelection"].performed += carSwitchManager.SwitchCar;
            player.Value._playerInput.actions["Select"].performed += context => carSwitchManager.OnValidateButtonPress(player.Value._playerInput, context);
        }
        

    }

    
}

