using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class S_MapSelection : MonoBehaviour
{
    [Title("MapData")]
    [Tooltip("Mettre ici tous les MapData créer dans l'ordre voulu d'apparition lord de la selection.")]
    [SerializeField] private List<MapsData> _mapsData;
    private List<GameObject> _maps = new List<GameObject>();

    [SerializeField] private GameObject _defaultMapObject;

    [SerializeField] private PlayersData _playerData;
    [SerializeField] private TMP_Text _textMapName;

    private Vector3 _firstMapPosition = new Vector3(-5f, -1, -10f);
    private Vector3 _secondMapPosition = new Vector3(7.5f, -1f, 0);
    private Vector3 _mapBeforeposition = new Vector3(-17.5f, -1f, 0);
    private float _distanceXBetweenMap = 15f;
    private Dictionary<InputDevice, PlayerInfo> _players => _playerData.players;


    private int _currentMapIndex = 0;

    private void Awake()
    {
       
    }
    private void Start()
    {
        foreach (var mapData in _mapsData)
        {
            

            if (mapData.MapObject == null)
            {
                var map = Instantiate(_defaultMapObject);
                _maps.Add(map);
            }
            else
            {
                var maps = Instantiate(mapData.MapObject);
                _maps.Add(maps);
            }
          

        }

        MapObjectSetup();
    }

    public void MapObjectSetup()
    {
        _textMapName.text = _mapsData[_currentMapIndex].MapNameDisplay;
        Debug.Log("Map index" + _currentMapIndex);

        Debug.Log("Map count" + _maps.Count);

        if (_maps != null)
        {
            _maps[0].transform.position = _firstMapPosition;

            if (_maps.Count > 1)
            {
                _maps[1].transform.position = _secondMapPosition;

                if (_maps.Count > 2)
                {
                    for (int i = 2; i < _maps.Count; i++)
                    {
                        Vector3 newPosition = new Vector3(_secondMapPosition.x + _distanceXBetweenMap * (i - 1), _secondMapPosition.y, _secondMapPosition.z);
                        _maps[i].transform.position = newPosition;

                    }
                }
            }
        }
        else
        {
            Debug.Log("There is no MapData, go create MapData via sciptableObject and put it in it");
        }
        
    }

    //Switch sur la prochaine map en deplacant leur position tant qu'on ne sort pas de l'index
    private void NextMap()
    {
        
        _currentMapIndex++;
       
        _textMapName.text = _mapsData[_currentMapIndex].MapNameDisplay;

        foreach (var mapData in _maps)
        {
            mapData.transform.position = new Vector3(mapData.transform.position.x - _distanceXBetweenMap, mapData.transform.position.y);

        }

        _maps[_currentMapIndex].transform.position = _firstMapPosition;

        if (_currentMapIndex < _mapsData.Count - 1)
        {
            _maps[_currentMapIndex + 1].transform.position = _secondMapPosition;
        }
        else
        {
            Debug.Log("out of map");
        }
        if (_maps[_currentMapIndex - 1] != null)
        {
            _maps[_currentMapIndex - 1].transform.position = _mapBeforeposition;
        }
    }

    //Switch sur la map d'aavnt en deplacant leur position tant qu'on ne sort pas de l'index
    private void PreviousMap()
    {
        _currentMapIndex--;

        _textMapName.text = _mapsData[_currentMapIndex].MapNameDisplay;

        foreach (var mapData in _maps)
        {
            mapData.transform.position = new Vector3(mapData.transform.position.x + _distanceXBetweenMap, mapData.transform.position.y);
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
        if (_maps[_currentMapIndex + 1] != null)
        {
            _maps[_currentMapIndex + 1].transform.position = _secondMapPosition;
        }
    }



    //Recupere l'action du oystick gauche du joueur
    public void SelectMap(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            float input = context.ReadValue<float>();
            if (input > 0 && _currentMapIndex < _mapsData.Count - 1)
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

    [SerializeField] List<string> arenas;
    //Valide la selection en recuperent l'action map pour controller les voiture
    public void ValidateSelection(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ChangeActionMap();
            SceneManager.LoadScene(_mapsData[_currentMapIndex].MapNameScene);
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
            player.Value._playerInput.actions["MoveSelection"].performed -= SelectMap;
            player.Value._playerInput.actions["Select"].performed -= ValidateSelection;
            player.Value._playerInput.actions["MoveSelection"].Disable();
            player.Value._playerInput.actions["Select"].Disable();
            player.Value._playerInput.actions["Back"].Disable();

            player.Value._playerInput.SwitchCurrentActionMap("CarControl");
            Debug.Log(player.Value._playerInput.currentActionMap.ToString());
        }
    }

   
}
