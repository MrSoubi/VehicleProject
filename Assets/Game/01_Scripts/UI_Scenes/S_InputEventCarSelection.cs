using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class S_InputEventCarSelection : MonoBehaviour
{
    public S_CarSelection _carSelectionManager;
    public S_CarSwitch carSwitchManager;
    public PlayerInput playerInput;
    private Dictionary<InputDevice, PlayerInfo> players => _carSelectionManager.ReturnPlayerInfo();

    private void Awake()
    {
        playerInput.actions["Steer"].performed += carSwitchManager.SwitchCar;
        playerInput.actions["Join"].performed += _carSelectionManager.OnSouthButtonPress;
        playerInput.actions["Jump"].performed += carSwitchManager.OnValidateButtonPress;
    }

    //public void OnDestroy()
    //{
    //    UnsubscribeFromAll();
    //}

    //public void SubscribeToCarSwitching()
    //{
    //    playerInput.actions["Steer"].performed += carSwitchManager.SwitchCar;
    //    playerInput.actions["Jump"].performed += _carSelectionManager.OnSouthButtonPress;
    //}

    //public void DisablePlayerInputEnterParty(InputDevice playerDevice)
    //{
    //    if (players.ContainsKey(playerDevice))
    //    {
    //        var playerInfo = players[playerDevice];
    //        PlayerInput playerInput = PlayerInput.GetPlayerByIndex(playerInfo.playerId);

    //        if (playerInput != null)
    //        {
    //            //playerInput.actions["Jump"].performed -= _carSelectionManager.OnSouthButtonPress;
    //            //playerInput.actions["Jump"].performed += carSwitchManager.OnValidateButtonPress;

    //            Debug.Log($"Actions d�sactiv�es pour le joueur {playerInfo.playerId}");
    //        }
    //        else
    //        {
    //            Debug.LogError($"PlayerInput pour le joueur {playerInfo.playerId} est null.");
    //        }
    //    }
    //    else
    //    {
    //        Debug.LogError($"Aucun joueur trouv� avec le dispositif {playerDevice.displayName}.");
    //    }
    
    //}

    //public void EnablePlayerInputEnterParty(InputDevice playerDevice)
    //{
    //    if (players.ContainsKey(playerDevice))
    //    {
    //        var playerInfo = players[playerDevice];
    //        PlayerInput playerInput = PlayerInput.GetPlayerByIndex(playerInfo.playerId);

    //        if (playerInput != null)
    //        {
    //            playerInput.actions["Jump"].performed += _carSelectionManager.OnSouthButtonPress;
    //            playerInput.actions["Jump"].performed -= carSwitchManager.OnValidateButtonPress;
    //            Debug.Log($"Actions r�activ�es pour le joueur {playerInfo.playerId}");
    //        }
    //    }
    //}

    //public void DisablePlayerInputEndSelection(InputDevice playerDevice)
    //{
    //    if (players.ContainsKey(playerDevice))
    //    {
    //        var playerInfo = players[playerDevice];
    //        PlayerInput playerInput = PlayerInput.GetPlayerByIndex(playerInfo.playerId);

    //        if (playerInput != null)
    //        {
    //            playerInput.actions["Steer"].performed -= carSwitchManager.SwitchCar;
    //            playerInput.actions["Jump"].performed -= carSwitchManager.OnValidateButtonPress;
    //            Debug.Log($"Actions d�sactiv�es pour le joueur {playerInfo.playerId}");
    //        }
    //    }
    //}

    //public void EnablePlayerInputEndSelection(InputDevice playerDevice)
    //{
    //    if (players.ContainsKey(playerDevice))
    //    {
    //        var playerInfo = players[playerDevice];
    //        PlayerInput playerInput = PlayerInput.GetPlayerByIndex(playerInfo.playerId);

    //        if (playerInput != null)
    //        {
    //            playerInput.actions["Steer"].performed += carSwitchManager.SwitchCar;
    //            playerInput.actions["Jump"].performed += carSwitchManager.OnValidateButtonPress;
    //            Debug.Log($"Actions r�activ�es pour le joueur {playerInfo.playerId}");
    //        }
    //    }
    //}

    //public void UnsubscribeFromAll()
    //{
    //    playerInput.actions["Steer"].performed -= carSwitchManager.SwitchCar;
    //    playerInput.actions["Jump"].performed -= _carSelectionManager.OnSouthButtonPress;
    //    playerInput.actions["Jump"].performed -= carSwitchManager.OnValidateButtonPress;
    //}
}

