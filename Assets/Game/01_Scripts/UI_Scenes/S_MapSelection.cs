using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class S_MapSelection : MonoBehaviour
{
    [SerializeField] private GameObject[] _maps;
    [SerializeField] private PlayersData _playerData;
    private Vector3 _firstMapPosition = new Vector3(-5f, -1, -10f);
    private Vector3 _secondMapPosition = new Vector3(7.5f, -1f, 5.5f);
    private Vector3 _mapBeforeposition = new Vector3(-17.5f, -1f, 5.5f);
    private float _distanceXBetweenMap = 15f;
    private Dictionary<InputDevice, PlayerInfo> _players => _playerData.players;


    private int _currentMapIndex = 0;

    private void Start()
    {
        _maps[0].transform.position = _firstMapPosition;
        _maps[1].transform.position = _secondMapPosition;
        for (int i = 2; i < _maps.Length; i++)
        {
            _maps[i].transform.position = new Vector3 (_secondMapPosition.x + _distanceXBetweenMap * (i - 1), _secondMapPosition.y, _secondMapPosition.z);
        }

        Debug.Log(_maps.Length);
    }

    //Switch sur la prochaine map en deplacant leur position tant qu'on ne sort pas de l'index
    private void NextMap()
    {
        
        _currentMapIndex++;
       

        foreach (var map in _maps)
        {
            map.transform.position = new Vector3(map.transform.position.x - _distanceXBetweenMap, map.transform.position.y);
        }

        _maps[_currentMapIndex].transform.position = _firstMapPosition;

        if (_currentMapIndex < _maps.Length - 1)
        {
            _maps[_currentMapIndex + 1].transform.position = _secondMapPosition;
        }
        else
        {
            Debug.Log("out of map");
        }
    }

    //Switch sur la map d'aavnt en deplacant leur position tant qu'on ne sort pas de l'index
    private void PreviousMap()
    {
        _currentMapIndex--;
        
        foreach (var map in _maps)
        {
            map.transform.position = new Vector3(map.transform.position.x + _distanceXBetweenMap, map.transform.position.y);
        }

        _maps[_currentMapIndex].transform.position = _firstMapPosition;

        if (_currentMapIndex - 1 >= 0)
        {
            _maps[_currentMapIndex - 1].transform.position = _mapBeforeposition;
        }
        else
        {
            Debug.Log("out of map");
        }
        
    }

    

    //private void DisableOtherPlayersControls()
    //{
    //    foreach (var player in _players.Values)
    //    {
    //        if (player.playerId != 0)
    //        {
    //            var input = player._playerInput;
    //            input.actions.Disable();
    //        }
    //    }
    //}


    //Recupere l'action du oystick gauche du joueur
    public void SelectMap(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            float input = context.ReadValue<float>();
            if (input > 0 && _currentMapIndex < _maps.Length - 1)
            {
                NextMap();
                Debug.Log("current map: " + _currentMapIndex);
            }
            else if (input < 0 && _currentMapIndex > 0)
            {
                PreviousMap();
                Debug.Log("current map: " + _currentMapIndex);
            }
        }
    }

    //Valide la selection en recuperent l'action map pour controller les voiture
    public void ValidateSelection(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ChangeActionMap();
            //SceneManager.LoadScene("Map_"+ _currentMapIndex.ToString());//Load scene to test
            SceneManager.LoadScene("BasicLevel");//Load scene to test
        }
    }

    public void CancelMapSelection(InputAction.CallbackContext context)
    {
        if (context.performed)
        {

        }

    }

    
    //Change l'action map de tous les joueurs en CarControl
    public void ChangeActionMap()
    {
        foreach (var player in _players)
        {
            player.Value._playerInput.SwitchCurrentActionMap("CarControl");
            Debug.Log(player.Value._playerInput.currentActionMap.ToString());
        }
    }

   
}
