using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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

    [SerializeField] private GameObject _defaultMapObject;

    [SerializeField] private PlayersData _playerData;
    [SerializeField] private TMP_Text _textMapName;

    private Vector3 _firstMapPosition = new Vector3(-5f, -1, -10f);
    private Vector3 _secondMapPosition = new Vector3(7.5f, -1f, 5.5f);
    private Vector3 _mapBeforeposition = new Vector3(-17.5f, -1f, 5.5f);
    private float _distanceXBetweenMap = 15f;
    private Dictionary<InputDevice, PlayerInfo> _players => _playerData.players;


    private int _currentMapIndex = 0;

    private void Awake()
    {
        foreach (var mapData in _mapsData)
        {
            if (mapData.MapObject == null)
            {
                var mapObject = Instantiate(_defaultMapObject);
                mapData.MapObject = mapObject;
            }
        }
    }
    private void Start()
    {
        MapObjectSetup();
    }

    private void MapObjectSetup()
    {
        _textMapName.text = _mapsData[_currentMapIndex].MapName;
        Debug.Log("Map index" + _currentMapIndex);

        Debug.Log(_mapsData.Count);

        if (_mapsData != null)
        {
            _mapsData[0].MapObject.transform.position = _firstMapPosition;

            if (_mapsData.Count > 1)
            {
                _mapsData[1].MapObject.transform.position = _secondMapPosition;
                if (_mapsData.Count > 2)
                {
                    for (int i = 2; i < _mapsData.Count; i++)
                    {
                        _mapsData[i].MapObject.transform.position = new Vector3(_secondMapPosition.x + _distanceXBetweenMap * (i - 1), _secondMapPosition.y, _secondMapPosition.z);
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
       
        _textMapName.text = _mapsData[_currentMapIndex].MapName;

        foreach (var mapData in _mapsData)
        {
            mapData.MapObject.transform.position = new Vector3(mapData.MapObject.transform.position.x - _distanceXBetweenMap, mapData.MapObject.transform.position.y);
        }

        _mapsData[_currentMapIndex].MapObject.transform.position = _firstMapPosition;

        if (_currentMapIndex < _mapsData.Count - 1)
        {
            _mapsData[_currentMapIndex + 1].MapObject.transform.position = _secondMapPosition;
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

        _textMapName.text = _mapsData[_currentMapIndex].MapName;

        foreach (var mapData in _mapsData)
        {
            mapData.MapObject.transform.position = new Vector3(mapData.MapObject.transform.position.x + _distanceXBetweenMap, mapData.MapObject.transform.position.y);
        }

        _mapsData[_currentMapIndex].MapObject.transform.position = _firstMapPosition;

        if (_currentMapIndex - 1 >= 0)
        {
            _mapsData[_currentMapIndex - 1].MapObject.transform.position = _mapBeforeposition;
        }
        else
        {
            Debug.Log("out of map");
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
            SceneManager.LoadScene(_mapsData[_currentMapIndex].MapName);
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
