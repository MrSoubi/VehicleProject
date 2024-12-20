using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class S_GameSetup : MonoBehaviour
{
    [SerializeField] private PlayersData _playersData;
    [SerializeField] private List<GameObject> _carsPrefabs; //Faut que cela corresponde a l'ordre de voiture dans la scne Car Selection

    [SerializeField] private List<Transform> _playerSpawnTransforms;
    [SerializeField] private DisplayManager _displayManager;


    [SerializeField] private PlayerInput playerInputToTestFromMapSelection;

    private Dictionary<InputDevice, PlayerInfo> players => _playersData.players;

    [SerializeField] EventChannel _onGameStart;
    [SerializeField] EventChannel _onTimerEnd;

    void Start()
    {
        if (players.Count == 0)
        {
            GameObject carPrefab = GetCarPrefabID(0);
            GameObject carInstance = Instantiate(carPrefab, GetSpawnPosition(0), GetSpawnRotation(0));

            S_CarInputEvent s_CarInputEvent = carInstance.GetComponent<S_CarInputEvent>();
            S_RumbleManager s_RumbleManager = carInstance.GetComponent<S_RumbleManager>();

            S_CameraLayerSetup s_CameraLayerSetup = carInstance.GetComponentInChildren<S_CameraLayerSetup>();
            s_CameraLayerSetup.SetPlayerID(0);
            //CarController carController = carInstance.GetComponent<CarController>();
            //_displayManager.ReturnCarControllerList().Add(carController);


            s_CarInputEvent.Initialize(playerInputToTestFromMapSelection,0); //Donne le player input a la voiture assigner au joueur et lui donne ses actions
            s_RumbleManager.Init(playerInputToTestFromMapSelection);
        }
        else
        {
            foreach (var player in players)
            {
                GameObject carPrefab = GetCarPrefabID(player.Value.carIDSelected);
                GameObject carInstance = Instantiate(carPrefab, GetSpawnPosition(player.Value.playerId), GetSpawnRotation(player.Value.playerId));

                PlayerInput playerInput = player.Value._playerInput; //Assigne le player input du PlayerData au player assigné


                S_CarInputEvent s_CarInputEvent = carInstance.GetComponent<S_CarInputEvent>();
                S_RumbleManager s_RumbleManager = carInstance.GetComponent<S_RumbleManager>();

                S_CameraLayerSetup s_CameraLayerSetup = carInstance.GetComponentInChildren<S_CameraLayerSetup>();
                s_CameraLayerSetup.SetPlayerID(player.Value.playerId);
                //CarController carController = carInstance.GetComponent<CarController>();
                //_displayManager.ReturnCarControllerList().Add(carController);


                s_CarInputEvent.Initialize(playerInput, player.Value.playerId); //Donne le player input a la voiture assigner au joueur et lui donne ses actions
                s_RumbleManager.Init(playerInput);


                player.Value._playerInput.DeactivateInput();
            }
        }
        //Creer tous les voitures des players dans le players data en focntion de son carIDSelected selectioné dans la scene CarSelction et setup les cameras du splitscreen
       
        _displayManager.SetupCamera();

        _onGameStart.onEventTriggered.Invoke();
    }

    private void OnEnable()
    {
        _onTimerEnd.onEventTriggered.AddListener(ActivAtePlayersInput);
    }

    private void OnDisable()
    {
        _onTimerEnd.onEventTriggered.RemoveListener(ActivAtePlayersInput);
    }

    public void ActivAtePlayersInput()
    {
        foreach (var player in players)
        {
            player.Value._playerInput.ActivateInput();
        }
    }

    GameObject GetCarPrefabID(int carID)
    {
        return _carsPrefabs[carID];
    }

    Vector3 GetSpawnPosition(int playerID)
    {
        return _playerSpawnTransforms[playerID].position;
    }

    Quaternion GetSpawnRotation(int playerID)
    {
        return _playerSpawnTransforms[playerID].rotation;
    }
}
