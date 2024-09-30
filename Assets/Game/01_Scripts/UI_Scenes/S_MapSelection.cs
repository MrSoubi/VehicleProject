using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class S_MapSelection : MonoBehaviour
{
    [SerializeField] private GameObject[] _maps;
    //[SerializeField] private float slideSpeed = 5f;
    [SerializeField] private PlayersData _playerData;
    private Dictionary<InputDevice, PlayerInfo> _players => _playerData.players;


    private int currentMapIndex = 0;

    private void Start()
    {
               //UpdateMapDisplay();
    }

    private void NextMap()
    {
        currentMapIndex++;
        if (currentMapIndex >= _maps.Length)
        {
            currentMapIndex = 0;
        }
        //UpdateMapDisplay();
    }

    private void PreviousMap()
    {
        currentMapIndex--;
        if (currentMapIndex < 0)
        {
            currentMapIndex = _maps.Length - 1;
        }
        //UpdateMapDisplay();
    }

    private void UpdateMapDisplay()
    {
        
        foreach (var map in _maps)
        {
            map.SetActive(false);
        }

        
        _maps[currentMapIndex].SetActive(true);

        Debug.Log("current map: " + currentMapIndex);
    }

    private void DisableOtherPlayersControls()
    {
        foreach (var player in _players.Values)
        {
            if (player.playerId != 0)
            {
                var input = player._playerInput;
                input.actions.Disable();
            }
        }
    }



    public void SelectMap(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            float input = context.ReadValue<float>();
            if (input > 0)
            {
                NextMap();
                ChangeMapDisplay(true);
            }
            else
            {
                PreviousMap();
                ChangeMapDisplay(false);
            }
        }
    }

    public void ValidateSelection(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ChangeActionMap();
            SceneManager.LoadScene("Map_"+ currentMapIndex.ToString());//Load scene to test
        }
    }

    public void CancelMapSelection(InputAction.CallbackContext context)
    {
        if (context.performed)
        {

        }

    }

    public void ChangeMapDisplay(bool switchRight)
    {

    }

    public void ChangeActionMap()
    {
        foreach (var player in _players)
        {
            player.Value._playerInput.SwitchCurrentActionMap("CarControl");
            Debug.Log(player.Value._playerInput.currentActionMap.ToString());
        }
    }

    public Dictionary<InputDevice, PlayerInfo> ReturnPlayerInfo()
    {
        return _players;
    }
}
