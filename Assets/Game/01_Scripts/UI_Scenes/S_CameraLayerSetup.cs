using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_CameraLayerSetup : MonoBehaviour
{
    [SerializeField] private List<GameObject> _virtualCameras;
    [SerializeField] private Camera _Camera;
    [SerializeField] private Canvas _Canvas;
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

        foreach (var cam in _virtualCameras)
        {
            cam.layer = _layerMask;
        }
        
        _Camera.gameObject.layer = _layerMask;
        _Camera.cullingMask &= ~((1 << 7) | (1 << 8) | (1 << 9) | (1 << 10)); //Enleve les layers des joueurs sur le culling mask de la camera
        _Camera.cullingMask |= LayerMask.GetMask($"PlayerCam{_playerID + 1}"); //Rajoute le layer du joueur

/*        _Canvas.gameObject.layer = _layerMask;
        foreach(Transform t in _Canvas.transform)
        {
            t.gameObject.layer = _layerMask;
        }*/
    }

}
