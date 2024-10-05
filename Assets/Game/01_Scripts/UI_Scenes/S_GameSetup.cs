using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class S_GameSetup : MonoBehaviour
{
    [SerializeField] private PlayersData _playersData;
    [SerializeField] private List<GameObject> _carsPrefabs; //Faut que cela corresponde a l'ordre de voiture dans la scne Car Selection
    [SerializeField] private List<Vector3> _playersSpawnPosition;
    [SerializeField] private List<Quaternion> _playersSpawnRotation;
    [SerializeField] private DisplayManager _displayManager;
    private Dictionary<InputDevice, PlayerInfo> players => _playersData.players;
    void Awake()
    {
        foreach (var player in players)
        {
            GameObject carPrefab = GetCarPrefabID(player.Value.carIDSelected);
            GameObject carInstance = Instantiate(carPrefab, GetSpawnPosition(player.Value.playerId), GetSpawnRotation(player.Value.playerId));

            PlayerInput playerInput = player.Value._playerInput;
           

            S_CarInputEvent s_CarInputEvent = carInstance.GetComponent<S_CarInputEvent>();

            S_CameraLayerSetup s_CameraLayerSetup = carInstance.GetComponentInChildren<S_CameraLayerSetup>();
            s_CameraLayerSetup.SetPlayerID(player.Value.playerId);
            //CarController carController = carInstance.GetComponent<CarController>();
            //_displayManager.ReturnCarControllerList().Add(carController);
            

            s_CarInputEvent.Initialize(playerInput);

        }
        _displayManager.SetupCamera();
    }

    

    GameObject GetCarPrefabID(int carID)
    {
        return _carsPrefabs[carID];
    }

    Vector3 GetSpawnPosition(int playerID)
    {
        return _playersSpawnPosition[playerID];
    }

    Quaternion GetSpawnRotation(int playerID)
    {
        return _playersSpawnRotation[playerID];
    }
}
