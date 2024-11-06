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


    [SerializeField] private PlayerInput _playerInput;
    private void Awake()
    {
        Debug.Log(_players.Count);

        if (_players.Count == 0)
        {
            Debug.Log("NoPlayerInput");
            EnableInputMapSelectionForTest();
        }
        else
        {
            EnableInputMapSelection();
        }
    }
    void Update()
    {
        
    }

    void OnDisable()
    {
        if (_players.Count == 0)
        {
            Debug.Log("NoPlayerInput");
            DesableInputMapSelectionForTest();
        }
        else
        {
            DesableInputMapSelection();
        }
        //DesableInputMapSelectionForTest();
        DesableInputMapSelection();

    }

    public void EnableInputMapSelectionForTest()
    {
        _playerInput.actions["MoveSelection"].performed += _mapSelection.SelectMap;
        _playerInput.actions["Select"].performed += _mapSelection.ValidateSelection;
    }
    public void DesableInputMapSelectionForTest()
    {
        _playerInput.actions["MoveSelection"].performed -= _mapSelection.SelectMap;
        _playerInput.actions["Select"].performed -= _mapSelection.ValidateSelection;
    }

    //Donne l'action seulementau joueur 0 pour choisir la map
    public void EnableInputMapSelection()
    {
        _players[_players.FirstOrDefault(x => x.Value.playerId == 0).Key]._playerInput.actions["MoveSelection"].Enable();
        _players[_players.FirstOrDefault(x => x.Value.playerId == 0).Key]._playerInput.actions["MoveSelection"].performed += _mapSelection.SelectMap;
        _players[_players.FirstOrDefault(x => x.Value.playerId == 0).Key]._playerInput.actions["Select"].Enable();

        _players[_players.FirstOrDefault(x => x.Value.playerId == 0).Key]._playerInput.actions["Select"].performed += _mapSelection.ValidateSelection;

    }

    public void DesableInputMapSelection()
    {
        _players[_players.FirstOrDefault(x => x.Value.playerId == 0).Key]._playerInput.actions["MoveSelection"].performed -= _mapSelection.SelectMap;
        _players[_players.FirstOrDefault(x => x.Value.playerId == 0).Key]._playerInput.actions["Select"].performed -= _mapSelection.ValidateSelection;
        _players[_players.FirstOrDefault(x => x.Value.playerId == 0).Key]._playerInput.actions["MoveSelection"].Disable();
        _players[_players.FirstOrDefault(x => x.Value.playerId == 0).Key]._playerInput.actions["Select"].Disable();



    }


}
