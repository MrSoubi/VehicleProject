using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class S_GameSetup : MonoBehaviour
{
    [SerializeField] private PlayersData _playersData;
    [SerializeField] private List<GameObject> _carsPrefabs; //Faut que cela corresponde a l'ordre de voiture dans la scne Car Selection

    [SerializeField] private List<Transform> _playerSpawnTransforms;
    [SerializeField] private DisplayManager _displayManager;
    private Dictionary<InputDevice, PlayerInfo> players => _playersData.players;
    void Start()
    {
        //Creer tous les voitures des players dans le players data en focntion de son carIDSelected selectioné dans la scene CarSelction et setup les cameras du splitscreen
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
            

            s_CarInputEvent.Initialize(playerInput); //Donne le player input a la voiture assigner au joueur et lui donne ses actions
            s_RumbleManager.Init(playerInput);
        }
        _displayManager.SetupCamera();
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
