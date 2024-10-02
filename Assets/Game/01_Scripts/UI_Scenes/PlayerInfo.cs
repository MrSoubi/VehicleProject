using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class PlayerInfo
{
    public int playerId;
    public int panelIndex;
    public int carIDSelected;
    public bool isRotating;
    public bool isValidateSelection = false;
    public PlayerInput _playerInput;

}