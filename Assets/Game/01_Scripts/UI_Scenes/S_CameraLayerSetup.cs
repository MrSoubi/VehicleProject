using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_CameraLayerSetup : MonoBehaviour
{
    [SerializeField] private GameObject _virtualCamera;
    [SerializeField] private Camera _Camera;
    int _layerMask = 0;
    int _playerID;
    [SerializeField] int _firstPlayerlayerIndex;

    private void Awake()
    {
        //_layerMask = _firstPlayerlayerIndex + _playerID;
        //_virtualCamera.layer = _layerMask;
        //_Camera.gameObject.layer = _layerMask;
    }
    
    public void SetPlayerID(int playerID)
    {
        _playerID = playerID;
        _layerMask = _firstPlayerlayerIndex + _playerID;
        _virtualCamera.layer = _layerMask;
        _Camera.gameObject.layer = _layerMask;
    }

}
