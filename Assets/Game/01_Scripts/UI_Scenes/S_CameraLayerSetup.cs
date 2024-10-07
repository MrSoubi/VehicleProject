using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_CameraLayerSetup : MonoBehaviour
{
    [SerializeField] private GameObject _virtualCamera;
    [SerializeField] private Camera _Camera;
    int _layerMask = 0;
    int _playerID = 0;
    [SerializeField] int _firstPlayerLayerIndex;

    private void Awake()
    {
        //_layerMask = _firstPlayerlayerIndex + _playerID;
        //_virtualCamera.layer = _layerMask;
        //_Camera.gameObject.layer = _layerMask;
    }
    
    public void SetPlayerID(int playerID)
    {
        _playerID = playerID;
        _layerMask = _firstPlayerLayerIndex + _playerID;
        _virtualCamera.layer = _layerMask;
        _Camera.gameObject.layer = _layerMask;
        _Camera.cullingMask &= ~((1 << 7) | (1 << 8) | (1 << 9) | (1 << 10)); //Enleve les layers des joueurs sur le culling mask de la camera
        _Camera.cullingMask |= LayerMask.GetMask($"PlayerCam{_playerID + 1}"); //Rajoute le layer du joueur
    }

}
