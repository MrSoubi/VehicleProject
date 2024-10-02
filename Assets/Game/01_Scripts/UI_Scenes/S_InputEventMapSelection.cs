using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;


public class S_InputEventMapSelection : MonoBehaviour
{
    [SerializeField] private S_MapSelection _mapSelection;
    [SerializeField] private PlayersData _playerData;
    private Dictionary<InputDevice, PlayerInfo> _players => _playerData.players;

    private void Awake()
    {
        EnableInputMapSelection();
    }
    void Update()
    {
        
    }

    //Donne l'action seulementau joueur 0 pour choisir la map
    public void EnableInputMapSelection()
    {
        _players[_players.FirstOrDefault(x => x.Value.playerId == 0).Key]._playerInput.actions["MoveSelection"].performed += _mapSelection.SelectMap;
        _players[_players.FirstOrDefault(x => x.Value.playerId == 0).Key]._playerInput.actions["Select"].performed += _mapSelection.ValidateSelection;
        _players[_players.FirstOrDefault(x => x.Value.playerId == 0).Key]._playerInput.actions["Back"].performed += _mapSelection.CancelMapSelection;
    }

    
}
